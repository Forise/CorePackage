//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(HealthPresenter2D))]
    public class PickableItemPresenter2D : APickableItemPresenter
    {
        [SerializeField]
        protected GameObject obj;
        [SerializeField]
        protected float offset;
        protected HealthPresenter2D health;
        protected Camera cam;

        protected override void Init()
        {
            cam = Camera.main;
            base.Init();
            if (!health)
                health = GetComponent<HealthPresenter2D>();
        }

        private void Update()
        {
            if (transform.position.x < cam.transform.position.x - offset || transform.position.y > cam.transform.position.y + offset)
            {
                Destroy(gameObject);
            }
        }

        public override GameObject PickUp()
        {
            gameObject.SetActive(false);
            return obj;
        }

        public void DisableCollider()
        {
            if(coll)
                coll.enabled = false;
        }

        public void EnableCollider()
        {
            if(coll)
                coll.enabled = true;
        }

        protected override void UpdateType()
        {
        }
    }
}