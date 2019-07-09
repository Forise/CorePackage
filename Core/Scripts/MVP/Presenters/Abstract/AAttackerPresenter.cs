//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Core
{
    [System.Serializable]
    public class SerializedDictionary_Transform_Transform : SerializableDictionary<Transform, Transform> { }
    public abstract class AAttackerPresenter : Presenter
    {
        #region Fields
        [SerializeField]
        protected AttackerModel attackModel;
        public Animator animator;
        public List<ADamagePresenter> damageObjects_ToSpawn;
        /// <summary>
        /// point to spawn Damage
        /// </summary>
        public Transform spawnPoint;
        public SerializedDictionary_Transform_Transform spawnDirectionPoints = new SerializedDictionary_Transform_Transform();
        public bool useRandomSpawnPoint;
        /// <summary>
        /// point for direction of Damage object;
        /// </summary>
        public Transform directionPoint;
        public bool attackFromAnimation;
        public bool useRandomAnim;
        public float delay;
        public ABuff[] buffs;

        /// <summary>
        /// Has to be defined in animator for start attack animation by trigger!
        /// </summary>
        [SerializeField]
        protected string[] animationTriggers;
        protected bool isAnimating;
        #endregion

        #region Properties
        public float Damage { get => attackModel.Damage; set => attackModel.Damage = value; }
        public List<string> EventsToNotify_OnAttack { get; set; } = new List<string>();

        public override float TimeScale
        {
            get => base.TimeScale;
            set
            {
                base.TimeScale = value;
                if(animator)
                    animator.speed = TimeScale;
            }
        }

        public string[] AnimationTriggers
        {
            get => animationTriggers;
            protected set => animationTriggers = value;
        }
        public bool IsAnimating
        {
            get => isAnimating;
            protected set => isAnimating = value;
        }
        public float AttackSpeed { get => attackModel.AttackSpeed; set => attackModel.AttackSpeed = value; }
        #endregion Properties

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void OnDisable()
        {
            IsAnimating = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            attackModel.OnAttackSpeedChanged -= UpdateAttackSpeedView;
            IsAnimating = false;
        }

        #region Methods

        protected virtual void Init()
        {
            attackModel.OnAttackSpeedChanged += UpdateAttackSpeedView;
            if (animationTriggers == null || animationTriggers.Length <= 0)
            {
    #if DEBUG
                Debug.LogWarning("Animation trigger is NULL or EMPTY!", this.gameObject);
    #endif
            }
        }

        /// <summary>
        /// if using Attack From Animation - you shuld call SpawnDamageObj
        /// method from Animation Event
        /// </summary>
        public virtual void Attack()
        {
            StartCoroutine(AttackCoroutine(delay));
        }

        public virtual void AmplifyDamage(float time, float damage)
        {
            attackModel.Damage += damage;
            TimerController.Instance.CreateNewMonoTimer(time, () => { attackModel.Damage -= damage; });
        }

        public virtual void AmplifyDamageMultiply(float time, float multiplyer)
        {
            attackModel.Damage *= multiplyer;
            TimerController.Instance.CreateNewMonoTimer(time, () => { attackModel.Damage -= multiplyer; });
        }

        public virtual IEnumerator AttackCoroutine(float dealy)
        {
            yield return new WaitForSeconds(delay);
            if (attackFromAnimation)
            {
                if (useRandomAnim)
                    animator.SetTrigger(animationTriggers[Random.Range(0, animationTriggers.Length)]);
                else
                    animator.SetTrigger(AnimationTriggers[0]);
                //IsAnimating = true;
            }
            else
                SpawnDamageObjects();
        }

        /// <summary>
        /// This is Spine Animation event Handler for old Character. "AnimationEvent_DoDamage" - name of event in Spine animation.
        /// </summary>
        protected virtual void AnimationEvent_DoDamage()
        {
            SpawnDamageObjects();
        }
        /// <summary>
        /// This is Spine Animation event Handler for monster. "Event_Hit" - name of event in Spine animation.
        /// </summary>
        protected virtual void Event_Hit()
        {
            SpawnDamageObjects();
        }
        /// <summary>
        /// This is Spine Animation event Handler for new Character. "AnimationEvent_attack" - name of event in Spine animation.
        /// </summary>
        protected virtual void AnimationEvent_attack()
        {
            SpawnDamageObjects();
        }

        /// <summary>
        /// Has to be called from Animation Event if using attack from animation
        /// </summary>
        /// <returns></returns>
        protected abstract List<ADamagePresenter> SpawnDamageObjects();

        protected abstract void UpdateAttackSpeedView();
        #endregion
    }
}