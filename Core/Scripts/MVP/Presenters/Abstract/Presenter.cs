//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public abstract class Presenter : MonoBehaviour
    {
        [SerializeField]
        protected TimeScale.Type timeScaleType = Core.TimeScale.Type.Global;

        public virtual float TimeScale { get; set; }

        protected virtual void Awake()
        {
            TimeScale = Core.TimeScale.GetTimeScale(timeScaleType);
            Core.TimeScale.OnTimeScaleChanged += TimeScaleUpdated;
        }

        protected virtual void OnDestroy()
        {
            Core.TimeScale.OnTimeScaleChanged -= TimeScaleUpdated;
        }

        protected virtual void TimeScaleUpdated()
        {
            TimeScale = Core.TimeScale.GetTimeScale(timeScaleType);
        }
    }
}