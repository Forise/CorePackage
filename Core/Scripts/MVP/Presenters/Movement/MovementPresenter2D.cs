//Developed by Pavel Kravtsov.
using System.Collections;
using UnityEngine;

namespace Core
{
    public class MovementPresenter2D : AMovementPresenter
    {
        [SerializeField]
        protected float cycleDelay;

        public virtual Vector3 Position
        {
            get => movementModel.Position;
            protected set => movementModel.Position = value;
        }
        public virtual Quaternion Rotation
        {
            get => movementModel.Rotation;
            protected set => movementModel.Rotation = value;
        }

        public virtual Vector2 Direction
        {
            get => movementModel.Direction;
            protected set => movementModel.Direction = value;
        }

        protected override void Update()
        {
            base.Update();
        }

        protected virtual void OnEnable()
        {
            SetPosition(transform.position);
            SetLocalPosition(transform.localPosition);
        }

        public override void Move()
        {
            
        }

        /// <summary>
        /// Set position.
        /// </summary>
        /// <param name="pos">Position to set.</param>
        public virtual void SetPosition(Vector3 pos)
        {
            movementModel.Position = pos;
        }

        public virtual void SetRotation(Quaternion rotation)
        {
            movementModel.Rotation = rotation;
        }

        public virtual void SetLocalPosition(Vector3 localPos)
        {
            movementModel.LocalPosition = localPos;
        }

        /// <summary>
        /// Move to position by model speed.
        /// </summary>
        /// <param name="position">End position</param>
        public virtual void MoveTo(Vector3 position, System.Action actionOnEnd = null)
        {
            StartCoroutine(MoveToCoroutine(position, actionOnEnd));
        }

        /// <summary>
        /// Move to position for a specific time
        /// </summary>
        /// <param name="position">End position</param>
        /// <param name="time">Time for move</param>
        public virtual void MoveTo(Vector3 position, float time = 0, System.Action actionOnEnd = null)
        {
            StartCoroutine(MoveToCoroutine(position, time, actionOnEnd));
        }

        /// <summary>
        /// Cycle moving between 2 positions.
        /// </summary>
        /// <param name="pos1">first position</param>
        /// <param name="pos2">second position</param>
        /// <param name="duration">time for move between 2 positions</param>
        /// <param name="delay">time delay to start</param>
        public virtual void StartCycleMoving(Vector3 pos1, Vector3 pos2, float duration, float delay = 0f)
        {
            StartCoroutine(CycleMoveCoroutine(pos1, pos2, duration, delay));
        }


        /// <summary>
        /// Follow to position
        /// </summary>
        /// <param name="position">position to follow</param>
        public virtual void FollowTo(Vector3 position)
        {
            if(movementModel.Position != position)
                movementModel.Position = Vector2.MoveTowards(movementModel.Position, position, movementModel.Speed * Time.deltaTime * TimeScale);
        }

        /// <summary>
        /// Set model speed.
        /// </summary>
        /// <param name="speed">speed value.</param>
        public virtual void SetSpeed(float speed)
        {
            movementModel.Speed = speed;
        }

        /// <summary>
        /// Set model direction.
        /// </summary>
        /// <param name="direction">direction value.</param>
        public virtual void SetDirection(Vector3 direction)
        {
            movementModel.Direction = direction;
        }

        protected override void UpdateSpeedView()
        {
            if(animator)
                animator.SetFloat("MovementSpeed", movementModel.Speed);
        }

        protected override void UpdateDirectionView()
        {
            
        }

        protected virtual IEnumerator MoveToCoroutine(Vector3 endPos, System.Action actionOnEnd = null)
        {
            isMoving = true;
            while(movementModel.Position != endPos)
            {
                yield return movementModel.Position = Vector2.MoveTowards(movementModel.Position, endPos, movementModel.Speed * Time.deltaTime * TimeScale);
            }
            actionOnEnd?.Invoke();
            isMoving = false;
            yield return null;
        }

        protected virtual IEnumerator MoveToCoroutine(Vector3 endPos, float duration, System.Action actionOnEnd = null)
        {
            isMoving = true;
            Vector3 start = transform.position;
            float elapsedTime = 0.0f;

            while (transform.position != endPos)
            {
                elapsedTime += Time.deltaTime * TimeScale;
                movementModel.Position = Vector3.Lerp(start, endPos, elapsedTime / duration);
                yield return null;
            }
            actionOnEnd?.Invoke();
            isMoving = false;
            IsAnimating = false;
        }

        protected virtual IEnumerator CycleMoveCoroutine(Vector3 pos1, Vector3 pos2, float duration, float delay = 0f)
        {
            if(delay > 0)
                yield return new WaitForSeconds(delay);
            if(Vector3.Distance(transform.position, pos1) <= Vector3.Distance(transform.position, pos2))
            {
                if (!IsMoving)
                {
                    yield return new WaitForSeconds(cycleDelay);
                    MoveTo(pos2, duration);
                    while (IsMoving) { yield return null; }
                }
            }
            else
            {
                if (!IsMoving)
                {
                    yield return new WaitForSeconds(cycleDelay);
                    MoveTo(pos1, duration);
                    while (IsMoving) { yield return null; }
                }
            }
            if (!IsMoving)
                StartCycleMoving(pos1, pos2, duration);
        }
    }
}