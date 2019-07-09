#define TEST
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Core.Rewards.Wheel
{
#if TEST
    [RequireComponent(typeof(CircleCollider2D))]
#endif
    public class WheelOfFortune : MonoBehaviour
    {
        #region Enums
        public enum WheelState
        {
            Idle,
            Acceleration,
            Spin,
            Braking
        }

        public enum WheelType
        {
            Time, Click
        }
        #endregion Enums

        #region Fields
        public const int FULL_CIRCLE_ANGLE = 360;
        [Tooltip("Item number to spin.")]
        public short targetItem;

        [SerializeField, Tooltip("List of rpizes and their chances.")]
        public Prize[] prizes;
        [SerializeField, Tooltip("Use 'Time' to stop after summ of curves times. Use 'Click' to stop by click. ")]
        private WheelType wheelType;


        [Header("Animation curves")]
        [SerializeField, Tooltip("Time = animation lenght. Value change speed by time = animation speed.")]
        private AnimationCurve startSpinCurve;
        [SerializeField, Tooltip("Time = animation lenght. Value change speed by time = animation speed.")]
        private AnimationCurve mainSpinCurve;
        [SerializeField, Tooltip("Time = animation lenght. Value change speed by time = animation speed.")]
        private AnimationCurve stopSpinCurve;

        [Space]
        [SerializeField, Tooltip("Loops for braking. (Also affects on wheel speed)")]
        private int BrakeLoops = 3;

        public System.Action onSpinEnd = delegate { };

        private WheelState currentState = WheelState.Idle;
        private float targetAngle;
        private float anglePerItem;
        private WeightedRandomCollection<byte> randomCollection = new WeightedRandomCollection<byte>();
        #endregion Fields

        #region Properties
        public WheelState CurrentState => currentState;
        #endregion Properties

        protected virtual void Awake()
        {
            anglePerItem = FULL_CIRCLE_ANGLE / prizes.Length;
            foreach (var p in prizes)
                randomCollection.AddEntry(p.id, p.chance);
		}

        protected virtual void OnMouseDown()
        {
            #if TEST
            switch (currentState)
            {
                case WheelState.Idle:
                    StartSpin(targetItem);
                    break;
                case WheelState.Spin:
                    if (wheelType == WheelType.Click)
                    {
                        StopSpin();
                    }
                    break;
            }
            #endif
        }

        protected virtual void OnDisable()
        {
            currentState = WheelState.Idle;

            onSpinEnd?.Invoke();
            onSpinEnd = delegate { };
            gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        #region Methods
        /// <summary>
        /// Start wheel spinning.
        /// </summary>
        /// <param name="item">If less then 0 == Random; Set the item if you want.</param>
        /// <param name="endOfSpinHandler">An action which will be awaken on end of spinning.</param>
        public void StartSpin(short item = -1, System.Action endOfSpinHandler = null)
        {
            if (currentState != WheelState.Idle)
                return;

            onSpinEnd = endOfSpinHandler;
            targetItem = item > -1 ? (byte)item : randomCollection.GetRandom();
            targetAngle = targetItem * anglePerItem;

            StartCoroutine(Spin());
        }

        public void StopSpin()
        {
            StopAllCoroutines();
            StartCoroutine(StoppingSpin());
        }

        protected IEnumerator Spin()
		{
			currentState = WheelState.Acceleration;
			float currentPositionCorrection = FULL_CIRCLE_ANGLE - gameObject.transform.localRotation.eulerAngles.z;
			float distanceToPass = BrakeLoops * FULL_CIRCLE_ANGLE + currentPositionCorrection + targetAngle;
			yield return StartCoroutine(SpinPhase(distanceToPass, startSpinCurve));

            currentState = WheelState.Spin;
            currentPositionCorrection = FULL_CIRCLE_ANGLE - gameObject.transform.localRotation.eulerAngles.z;
            distanceToPass = BrakeLoops * FULL_CIRCLE_ANGLE + currentPositionCorrection + targetAngle;
            yield return StartCoroutine(SpinPhase(distanceToPass, mainSpinCurve));

            #region IF PLAYER DIDN'T STOP THE WHEEL (Using for Spin by time)
            yield return StartCoroutine(StoppingSpin());
            #endregion IF PLAYER DIDN'T STOP THE WHEEL (Using for Spin by time)
        }

        protected IEnumerator StoppingSpin()
        {
            currentState = WheelState.Braking;
            float currentPositionCorrection = FULL_CIRCLE_ANGLE - gameObject.transform.localRotation.eulerAngles.z;
            float distanceToPass = BrakeLoops * FULL_CIRCLE_ANGLE + currentPositionCorrection + targetAngle;
            yield return StartCoroutine(SpinPhase(distanceToPass, stopSpinCurve));

            currentState = WheelState.Idle;
            onSpinEnd?.Invoke();
            onSpinEnd = delegate { };
        }

        protected IEnumerator SpinPhase(float spinDistance, AnimationCurve spinPattern)
        {
            float passedDistance = 0;
            float tempPassedDistance = 0;
            float distanceDifference = 0;

            float time = 0;

            while (time < spinPattern.keys[spinPattern.keys.Length-1].time)
            {
                time += Time.deltaTime;

                passedDistance = spinDistance * spinPattern.Evaluate(time);
                distanceDifference = passedDistance - tempPassedDistance;
                tempPassedDistance = passedDistance;

                transform.Rotate(0, 0, distanceDifference);

                yield return null;
            }
        }
        #endregion Methods
	}
}
