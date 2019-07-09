using UnityEngine;
namespace Core
{
    [System.Serializable]
    public class HealthModel : Model
    {
        [SerializeField]
        private float current;
        [SerializeField]
        private float max;
        
        public event OnModelChangedDelegate OnCurrentHPChanged;
        public event OnModelChangedDelegate OnMaxHPChanged;

        public float CurrentHP
        {
            get => current;
            set
            {
                current = value > MaxHP ? MaxHP : value;
                if (current < 0)
                    current = 0;
                OnCurrentHPChanged?.Invoke();
            }
        }

        public float MaxHP
        {
            get => max;
            set
            {
                max = value;
                OnMaxHPChanged?.Invoke();
            }
        }

        public HealthModel()
        {
            current = max;
        }

        public HealthModel(float maxHP)
        {
            max = maxHP;
            current = max;        
        }
    }
}