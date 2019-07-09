//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public abstract class AMovementPresenter : Presenter
    {
        #region Fields
        private bool isGrounded;
        [SerializeField]
        protected MovementModel movementModel = new MovementModel();
        protected bool isMoving;
        public Animator animator;
        public LayerMask groundLayer;
        
        protected bool isAnimating;
        [SerializeField]
        protected float gorundDetecterDist = 0.2f;
        #endregion

        #region Properties
        public override float TimeScale
        {
            get => base.TimeScale;
            set
            {
                base.TimeScale = value;
                if (animator)
                    animator.speed = TimeScale;
            }
        }

        public bool IsMoving { get => isMoving; }

        public bool IsAbleToMove { get => movementModel.IsAbleToMove; }

        public bool IsGrounded
        {
            get => isGrounded;
        }

        public bool IsAnimating
        {
            get => isAnimating;
            protected set => isAnimating = value;
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            if(animator)
                animator.speed = TimeScale;
            Init();
        }

        protected override void TimeScaleUpdated()
        {
            base.TimeScaleUpdated();
            animator.speed = TimeScale;
        }

        protected virtual void Update() { }
        protected virtual void FixedUpdate()
        {
            CheckGrounded();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            movementModel.OnPositionChanged -= UpdatePositionView;
            movementModel.OnLocalPositionChanged -= UpdateLocalPosView;
        }

        protected virtual void OnDisable()
        {
            isMoving = false;
        }

        #region Methods
        public void DisableMove()
        {
            GetComponent<Rigidbody2D>().simulated = false;
            movementModel.IsAbleToMove = false;
        }

        public void EnableMove()
        {
            GetComponent<Rigidbody2D>().simulated = true;
            movementModel.IsAbleToMove = true;
        }

        protected virtual void CheckGrounded()
        {
            isGrounded = Physics2D.Raycast(transform.position - new Vector3(0, 0.15f, 0), Vector2.down, gorundDetecterDist, groundLayer);
        }

        public virtual void StopAllMovement()
        {
            StopAllCoroutines();
            movementModel.IsAbleToMove = true;
            IsAnimating = false;
        }

        protected virtual void Init()
        {
            if (movementModel == null)
            {
                movementModel = new MovementModel();
                Debug.LogWarning("movementModel was null! Created new MovementModel()", this);
            }

            if (animator == null)
            {
                try
                {
                    animator = GetComponent<Animator>();
                    Debug.LogWarning("animator was null! animator has set from GetComponent<Animator>()", this);
                }
                catch(System.Exception ex)
                {
                    Debug.LogWarning(ex, this);
                }
            }

            movementModel.OnPositionChanged += UpdatePositionView;
            movementModel.OnLocalPositionChanged += UpdateLocalPosView;
            movementModel.OnSpeedChanged += UpdateSpeedView;
            movementModel.OnDirectionChanged += UpdateDirectionView;
            movementModel.Position = transform.position;
            movementModel.LocalPosition = transform.localPosition;
        }

        protected virtual void UpdatePositionView()
        {
            transform.position = movementModel.Position;
        }

        protected virtual void UpdateLocalPosView()
        {
            transform.localPosition = movementModel.LocalPosition;
        }

        public abstract void Move();

        protected abstract void UpdateDirectionView();

        protected abstract void UpdateSpeedView();
        #endregion
    }
}