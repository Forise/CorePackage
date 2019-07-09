//Developed by Pavel Kravtsov.
using UnityEngine;
namespace Core
{
    [System.Serializable]
    public class AttackerModel : Model
    {
        #region Fields
        [SerializeField]
        private AttackerType type;
        //[SerializeField]
        //private float attackSpeed = 1f;
        //[SerializeField]
        //private float power = 1f;
        //[SerializeField]
        //private float damage;
        [SerializeField]
        private WeaponBalanceObject balanceObject;
        #endregion

        #region Properties
        public AttackerType Type
        {
            get => type;
            set
            {
                type = value;
                OnTypeChanged?.Invoke();
            }
        }

        public float AttackSpeed
        {
            get => balanceObject.weaponBalance.attackSpeed;
            set
            {
                balanceObject.weaponBalance.attackSpeed = value;
                OnAttackSpeedChanged?.Invoke();
            }
        }

        public float Power
        {
            get => balanceObject.weaponBalance.shootPower;
            set
            {
                balanceObject.weaponBalance.shootPower = value;
                OnPowerChanged();
            }
        }

        public float Damage
        {
            get => balanceObject.weaponBalance.damage;
            set
            {
                balanceObject.weaponBalance.damage = value;
                OnDamageChanged?.Invoke();
            }
        }

        public float CriticalChance
        {
            get => balanceObject.weaponBalance.criticalChance;
            set
            {
                balanceObject.weaponBalance.criticalChance = value;
                OnCriticalChanceChanged?.Invoke();
            }
        }

        public float CritMultiplyer
        {
            get => balanceObject.weaponBalance.critMultiplier;
            set
            {
                balanceObject.weaponBalance.critMultiplier = value;
                OnCritMultiplierChanged?.Invoke();
            }
        }
        #endregion Properties

        public event OnModelChangedDelegate OnTypeChanged;
        public event OnModelChangedDelegate OnAttackSpeedChanged;
        public event OnModelChangedDelegate OnPowerChanged;
        public event OnModelChangedDelegate OnDamageChanged;
        public event OnModelChangedDelegate OnCriticalChanceChanged;
        public event OnModelChangedDelegate OnCritMultiplierChanged;

        public AttackerModel() { }

        //public AttackerModel(AttackerType type, float attackSpeed, float power, float damage)
        //{
        //    this.type = type;
        //    this.attackSpeed = attackSpeed;
        //    this.power = power;
        //    this.damage = damage;
        //}
    }
}