//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public class SwipeDetector : MonoBehaviour
    {
        #region Fields
        private Vector2 fingerDownPosition;
        private Vector2 fingerUpPosition;

        [SerializeField]
        private bool detectSwipeOnlyAfterRelease = false;

        [SerializeField]
        private float minDistanceForSwipeInPixels = 20f;

        public static event System.Action<SwipeData> OnSwipe = delegate { };
        #endregion

        #region Properties

        private bool IsVerticalSwipe
        {
            get => VerticalMovementDistance > HorizontalMovementDistance;
        }

        private bool SwipeDistanceCheckMet
        {
            get => VerticalMovementDistance > minDistanceForSwipeInPixels || HorizontalMovementDistance > minDistanceForSwipeInPixels;
        }

        private float VerticalMovementDistance
        {
            get => Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
        }

        private float HorizontalMovementDistance
        {
            get => Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
        }
        #endregion

        private void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUpPosition = touch.position;
                    fingerDownPosition = touch.position;
                }

                if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDownPosition = touch.position;
                    DetectSwipe();
                }
            }
        }

        #region Methods
        private void DetectSwipe()
        {
            if (SwipeDistanceCheckMet)
            {
                if (IsVerticalSwipe)
                {
                    var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                    SendSwipe(direction);
                }
                else
                {
                    var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                    SendSwipe(direction);
                }
                fingerUpPosition = fingerDownPosition;
            }
        }

        private void SendSwipe(SwipeDirection direction)
        {
            SwipeData swipeData = new SwipeData()
            {
                Direction = direction,
                StartPosition = fingerDownPosition,
                EndPosition = fingerUpPosition
            };
            OnSwipe(swipeData);
        }
        #endregion
    }

    public struct SwipeData
    {
        public Vector2 StartPosition;
        public Vector2 EndPosition;
        public SwipeDirection Direction;
    }

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}