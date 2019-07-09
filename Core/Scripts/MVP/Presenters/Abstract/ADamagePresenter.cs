//Developed by Pavel Kravtsov.
using UnityEngine;
using Core.EventSystem;

namespace Core
{
    public abstract class ADamagePresenter : Presenter
    {
        #region Fields
        [SerializeField]
        protected DamageModel damageModel;
        [SerializeField]
        protected bool friendlyFire;
        public GameObject parent;
        public bool isEnable = true;
        public ABuff[] buffs;
        #endregion

        #region Properties
        public float Damage { get => damageModel.Damage; set => damageModel.Damage = value; }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            if(damageModel.LifeTime > 0)
                Destroy(gameObject, damageModel.LifeTime);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        #region Methods
        public virtual void SetDamage(float damage)
        {
            damageModel.Damage = damage;
        }

        protected virtual void Hit(AHealthPresenter healthPresenter)
        {
            if (healthPresenter.IsHitable && isEnable)
            {
                healthPresenter.GetDamage(damageModel.Damage, parent == null ? gameObject : parent);
                if (healthPresenter.CurrentHP <= 0)
                    EventManager.Notify(this, new GameEventArgs(Events.DamageEvents.OBJECT_KILLED, healthPresenter.gameObject.tag, (object)healthPresenter.gameObject));
            }
        }
        #endregion Methods
    }
}