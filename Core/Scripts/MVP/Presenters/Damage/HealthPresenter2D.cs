//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public class HealthPresenter2D : AHealthPresenter
    {
        protected override void Awake()
        {
            base.Awake();
            healthModel.CurrentHP = healthModel.MaxHP;
        }

        public void AddHealth(float health)
        {
            healthModel.CurrentHP += health;
        }

        protected override void OnGottenDamage_Handler(GameObject sender)
        {
            if(healthModel.CurrentHP <= 0)
                Die();
        }

        protected override void OnMaxHPChanged_Handler() { }
        protected override void OnCurrentHPChanged_Handler() { }
    }
}