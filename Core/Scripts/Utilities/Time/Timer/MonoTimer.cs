//Developed by Pavel Kravtsov.
using System.Collections;
using UnityEngine;

namespace Core
{
    public class MonoTimer : MonoBehaviour
    {
        #region Fields
        private float duration = 0;
        private bool isActive;
        private Coroutine timerCoroutine;
        #endregion Fields

        public event System.Action OnDestroyed;

        #region Properties
        public bool IsActive { get => isActive; }
        public float Duration { get => duration; }
        #endregion Properties

        public MonoTimer() { }

        public MonoTimer (float time, System.Action endAction = null, System.Action ticAction = null, bool destroyOnFinish = false)
        {
            StartTimer(time, endAction, ticAction, destroyOnFinish);
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
        }

        public virtual void StartTimer(float time, System.Action endAction = null, System.Action ticAction = null, bool destroyOnFinish = false)
        {
            this.duration = time;
            timerCoroutine = StartCoroutine(TimerCoroutine(endAction, ticAction, destroyOnFinish));
            isActive = true;
        }

        public virtual void StopTimer()
        {
            StopCoroutine(timerCoroutine);
            isActive = false;
        }

        public virtual void ResetTimer(float time)
        {
            if (isActive)
                StopTimer();
            this.duration = time;
        }

        public virtual IEnumerator TimerCoroutine(System.Action endAction = null, System.Action ticAction = null, bool destroyOnFinish = false)
        {
            var startTime = Time.time;
            while (duration > 0)
            {
                yield return duration -= Time.deltaTime;
                ticAction?.Invoke();
            }
            duration = 0;
            endAction?.Invoke();
            isActive = false;
            if (destroyOnFinish) Destroy(gameObject);
        }
    }
}
