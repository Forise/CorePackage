//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections.Generic;

namespace Core
{
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class DamagePresenter2D : ADamagePresenter
    {
        public List<string> exceptions = new List<string>();
        public Rigidbody2D rb;
        public bool useGravity = true;
        public bool disableOnHit = true;
        public bool destroyOnHit = true;
        protected override void Awake()
        {
            base.Awake();
            if (!rb)
                rb = GetComponent<Rigidbody2D>();
            if(rb)
                rb.isKinematic = !useGravity;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (var ex in exceptions)
            {
                if (collision.gameObject.tag.Contains(ex))
                {
                    return;
                }
            }

            var health = collision.GetComponent<AHealthPresenter>();
            var buffable = collision.GetComponent<IBuffable>();
            if (!health)
                health = collision.gameObject.GetComponent<AHealthPresenter>();

            if (buffs.Length > 0 && buffable != null && (friendlyFire || parent.GetComponent<Character>() != (object)buffable))
            {
                buffable.Buff(buffs);
            }
            if (health)
            {
                if (friendlyFire || health.gameObject != parent)
                    Hit(health);
                if (disableOnHit)
                    gameObject.SetActive(false);
                if (destroyOnHit)
                    Destroy(gameObject);
            }
        }
    }
}