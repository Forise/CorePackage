//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class DamageModel : Model
    {
        #region Fields
        [SerializeField]
        private float damage;
        [SerializeField]
        private float lifeTime;
        #endregion Fields

        #region Properties
        public float Damage
        {
            get => damage;
            set
            {
                damage = value;
                OnDamageChanged?.Invoke();
            }
        }

        public float LifeTime
        {
            get => lifeTime;
            set
            {
                lifeTime = value;
                OnLifeTimeChanged?.Invoke();
            }
        }
        #endregion Properties

        public event OnModelChangedDelegate OnDamageChanged;
        public event OnModelChangedDelegate OnLifeTimeChanged;

        public DamageModel() { }

        public DamageModel(float damage)
        {
            this.damage = damage;
        }
    }
}