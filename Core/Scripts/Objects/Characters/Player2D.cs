//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(HealthPresenter2D), typeof(Animator), typeof(MovementPresenter2D))]
    public abstract class Player2D : Character
    {
        public HealthPresenter2D health;
        public MovementPresenter2D movement;

        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            if (!health)
                health = GetComponent<HealthPresenter2D>();
            if (!movement)
                movement = GetComponent<MovementPresenter2D>();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            var item = collision.gameObject.GetComponent<PickableItemPresenter2D>();
            if (item)
            {
                PickUpItem(item);
            }
        }

        public abstract void PickUpItem(PickableItemPresenter2D item);
    }
}