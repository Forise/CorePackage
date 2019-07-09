//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class AHealthPresenter : Presenter
    {
        public delegate void GotDamageDelegate(GameObject sender);
        public event GotDamageDelegate OnGottenDamage;
        public event System.Action OnDied = delegate { };

        #region Fields
        public string tagToDetect;
        public List<string> tagsToDetect = new List<string>();//TODO: realize

        [SerializeField]
        protected HealthModel healthModel;

        [SerializeField]
        private bool isHitable = true;
        #endregion Fields

        #region Properties
        public bool IsHitable { get => isHitable; set => isHitable = value; }
        public float CurrentHP { get => healthModel.CurrentHP; }
        public float MaxHP { get => healthModel.MaxHP; }
        #endregion Properties

        protected override void Awake()
        {
            base.Awake();
            OnGottenDamage += OnGottenDamage_Handler;
            healthModel.OnMaxHPChanged += OnMaxHPChanged_Handler;
            healthModel.OnCurrentHPChanged += OnCurrentHPChanged_Handler;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnGottenDamage -= OnGottenDamage_Handler;
            healthModel.OnMaxHPChanged -= OnMaxHPChanged_Handler;
            healthModel.OnCurrentHPChanged -= OnCurrentHPChanged_Handler;
        }

        protected virtual void OnEnable()
        {
            healthModel.CurrentHP = healthModel.MaxHP;
        }

        #region Methods

        public void Setup(float currentHP, float maxHP)
        {
            healthModel.MaxHP = maxHP;
            healthModel.CurrentHP = currentHP;
        }
        public virtual void GetDamage(float damage, GameObject sender = null)
        {
            if (string.IsNullOrEmpty(tagToDetect) || !sender || (sender.tag.Contains(tagToDetect)))
            {
                healthModel.CurrentHP -= damage;
                OnGottenDamage?.Invoke(sender);
            }
        }

        protected virtual void Die()
        {
            OnDied?.Invoke();
            gameObject.SetActive(false);
        }

        protected void NotifyDied()
        {
            OnDied?.Invoke();
        }

        /// <summary>
        /// Disable hit for a time.
        /// </summary>
        /// <param name="time">Time for disable</param>
        public void DisableHit(float time)
        {
            IsHitable = false;
            TimerController.Instance.CreateNewMonoTimer(time, () => { IsHitable = true; });
        }

        protected abstract void OnGottenDamage_Handler(GameObject sender);
        protected abstract void OnMaxHPChanged_Handler();
        protected abstract void OnCurrentHPChanged_Handler();
        #endregion
    }
}