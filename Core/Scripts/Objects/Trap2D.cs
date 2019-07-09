//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Collider2D), typeof(HealthPresenter2D), typeof(Collider2D))]
    public class Trap2D : MonoBehaviour
    {
        public bool isEnable = true;
        public string tagToDetect;
        public HealthPresenter2D health;
        public Animator animator;
        public ADamagePresenter damage;

        private void Awake()
        {
            if (!animator)
                animator = GetComponent<Animator>();
            if (!health)
                health = GetComponent<HealthPresenter2D>();
        }

        private void OnEnable()
        {
            isEnable = true;
            animator.SetTrigger("Play");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Utilities.CheckCollisionTag(collision, tagToDetect))
            {
                damage.isEnable = false;
                isEnable = false;
                animator.SetTrigger("Stop");
            }
        }
    }
}