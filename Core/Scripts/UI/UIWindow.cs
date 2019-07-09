//Developed by Pavel Kravtsov.
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;

namespace Core
{
    public class UIWindow : Core.StateMachine.StateMachine
    {
        #region Fields
        [SerializeField]
        private string id;
        [SerializeField]
        private bool isBlockingInputOnOpenAndClose = true;
        [SerializeField]
        private bool openedOnStart = false;
        [SerializeField]
        private float scaleSpeed = 3f;

        public string ID { get => id; private set => id = value; }

        public enum WindowStates : uint
        {
            Initialization,
            Inactive,
            OpenAnimation,
            Active,
            Holded,
            CloseAnimation
        }

        public enum AnimationType : uint
        {
            Default,
            OnOff,
            Animator
        }

        [SerializeField]
        protected GameObject windowObject;
        [SerializeField]
        protected AnimationType animationType;
        [SerializeField]
        protected bool closeOnMissClick;
        [SerializeField]
        protected bool turnOnInHoldState;
        [SerializeField]
        protected string parentWindowID;
        protected float timeToSwitch;
        protected bool isAnimated;

        protected Image[] images;
        protected Material[] materials;
        protected Renderer[] renderers;
        protected Animator animator;
        protected BoxCollider boxColl;
        #endregion Fields

        #region Properties
        public string ParentWindowID
        {
            get { return parentWindowID; }
            protected set
            {
                parentWindowID = value;
            }
        }
        #endregion Properties

        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            InitializeMachine();
            SetState((int)WindowStates.Initialization);
            boxColl = GetComponent<BoxCollider>();
            if (boxColl)
                boxColl.enabled = CurrentStateIndex == (int)WindowStates.Active;
        }

        #region Methods
        private void SetAnimating(bool isAnimating)
        {
            isAnimated = isAnimating;
        }
        private void StopAnim()
        {
            isAnimated = false;
        }
        protected override void InitializeMachine()
        {
            CreateStates(typeof(WindowStates));
            InitializeState(
                (int)WindowStates.Initialization,
                InitializationStateInitHandler,
                InitializationStateUpdateHandler,
                null);
            InitializeState(
                (int)WindowStates.Inactive,
                InactiveStateInitHandler,
                null,
                InactiveStateCloseHandler);
            InitializeState(
                (int)WindowStates.OpenAnimation,
                OpenAnimationStateInitHandler,
                OpenAnimationStateUpdateHandler,
                OpenAnimationStateCloseHandler);
            InitializeState(
                (int)WindowStates.Active,
                ActiveStateInitHandler,
                ActiveStateUpdateHandler,
                ActiveStateCloseHandler);
            InitializeState(
                (int)WindowStates.Holded,
                HoldedStateInitHandler,
                null,
                HoldedStateCloseHandler);
            InitializeState(
                (int)WindowStates.CloseAnimation,
                CloseAnimationStateInitHandler,
                CloseAnimationStateUpdateHandler,
                CloseAnimationStateCloseHandler);
        }

        protected void CloseThisWindow()
        {
            UIControl.Instance.CloseWindow(this);
        }

        protected override void SetState(int index)
        {
            base.SetState(index);
            if (boxColl)
                boxColl.enabled = index == (int)WindowStates.Active;
        }

        protected virtual void CheckMissClick()
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButton(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 700f))
                    {
                        if (hit.collider == null || hit.transform.gameObject == null ||
                            hit.transform.gameObject.GetComponent<UIWindow>() == null || hit.transform.gameObject.GetComponent<UIWindow>().id != id)
                        {
                            CloseThisWindow();
                        }
                    }
                    else
                    {
                        CloseThisWindow();
                    }
                }
            }
        }

        #region Handlers
        #region Initialization State
        protected virtual void InitializationStateInitHandler()
        {
            if (windowObject == null)
                windowObject = gameObject;
        }

        protected virtual void InitializationStateUpdateHandler()
        {
            if (!openedOnStart)
                SetState((int)WindowStates.Inactive);
            else
                SetState((int)WindowStates.Active);
        }
        #endregion
        #region Inactive State
        protected virtual void InactiveStateInitHandler()
        {
            if (windowObject)
                windowObject.SetActive(false);
            EventManager_Window.Subscribe(EventManager_Window.OPEN_WINDOW, Handler_OpenWindow);
            EventManager_Window.Notify(this, new GameEventArgs(EventManager_Window.WINDOW_CLOSED, ID));
        }

        protected virtual void InactiveStateCloseHandler()
        {
            EventManager_Window.Unsubscribe(EventManager_Window.OPEN_WINDOW, Handler_OpenWindow);
        }
        #endregion
        #region OpenAnimation State
        protected virtual void OpenAnimationStateInitHandler()
        {
            //EventManager.Notify(new GameEvent(WindowEvents.WINDOW_START_ANIMATION, ID), this);

            if (isBlockingInputOnOpenAndClose)
                EventManager_Input.Notify(this, new GameEventArgs(EventManager_Input.BLOCK_INPUT));
            switch (animationType)
            {
                case AnimationType.Default:
                    timeToSwitch = Time.time + 0.3f;
                    StartCoroutine(Show());
                    //Debug.LogWarning("Window using default animation", this.gameObject);
                    break;
                case AnimationType.OnOff:
                    if (windowObject)
                        windowObject.SetActive(true);
                    break;
                case AnimationType.Animator:
                    SetAnimating(true);
                    animator.SetTrigger("Open");
                    break;
                default:
                    timeToSwitch = Time.time + 0.3f;
                    StartCoroutine(Show());
                    break;
            }
        }

        protected virtual void OpenAnimationStateUpdateHandler()
        {
            if (!isAnimated && Time.time >= timeToSwitch)
                SetState((int)WindowStates.Active);
        }

        protected virtual void OpenAnimationStateCloseHandler()
        {
            if (isBlockingInputOnOpenAndClose)
                EventManager_Input.Notify(this, new GameEventArgs(EventManager_Input.UNBLOCK_INPUT));
        }
        #endregion
        #region Active State
        protected virtual void ActiveStateInitHandler()
        {
            EventManager_Window.Subscribe(EventManager_Window.CLOSE_WINDOW, Handler_CloseWindow);
            EventManager_Window.Subscribe(EventManager_Window.HOLD_WINDOW, Handler_HoldWindow);
            EventManager_Window.Notify(this, new GameEventArgs(EventManager_Window.WINDOW_OPENED, ID));
        }

        protected virtual void ActiveStateCloseHandler()
        {
            EventManager_Window.Unsubscribe(EventManager_Window.CLOSE_WINDOW, Handler_CloseWindow);
            EventManager_Window.Unsubscribe(EventManager_Window.HOLD_WINDOW, Handler_HoldWindow);
        }

        protected virtual void ActiveStateUpdateHandler()
        {
            if (closeOnMissClick)
            {
                CheckMissClick();
            }
        }
        #endregion
        #region Holded State
        protected virtual void HoldedStateInitHandler()
        {
            EventManager_Window.Notify(this, new GameEventArgs(EventManager_Window.WINDOW_HOLDED, ID));
            EventManager_Window.Subscribe(EventManager_Window.CLOSE_WINDOW, Handler_CloseWindow);
            EventManager_Window.Subscribe(EventManager_Window.RESUME_WINDOW, Handler_ResumeWindow);
            if (!turnOnInHoldState)
            {
                windowObject.SetActive(false);
            }
        }

        protected virtual void HoldedStateCloseHandler()
        {
            if (!turnOnInHoldState)
            {
                windowObject.SetActive(true);
            }
            EventManager_Window.Unsubscribe(EventManager_Window.CLOSE_WINDOW, Handler_CloseWindow);
            EventManager_Window.Unsubscribe(EventManager_Window.RESUME_WINDOW, Handler_ResumeWindow);
            EventManager_Window.Notify(this, new GameEventArgs(EventManager_Window.WINDOW_RESUMED, ID));
        }
        #endregion
        #region CloseAnimation State
        protected virtual void CloseAnimationStateInitHandler()
        {
            if (isBlockingInputOnOpenAndClose)
                EventManager_Input.Notify(this, new GameEventArgs(EventManager_Input.BLOCK_INPUT));
            timeToSwitch = Time.time;
            switch (animationType)
            {
                case AnimationType.Default:
                    timeToSwitch = Time.time + 0.3f;
                    StartCoroutine(Hide());
                    //Debug.LogWarning("Window using default animation", this.gameObject);
                    break;
                case AnimationType.OnOff:
                    if (windowObject)
                        windowObject.SetActive(false);
                    break;
                case AnimationType.Animator:
                    SetAnimating(true);
                    animator.SetTrigger("Close");
                    break;
                default:
                    timeToSwitch = Time.time + 0.3f;
                    StartCoroutine(Hide());
                    break;
            }
        }

        protected virtual void CloseAnimationStateUpdateHandler()
        {
            if (!isAnimated && Time.time >= timeToSwitch)
                SetState((int)WindowStates.Inactive);
        }

        protected virtual void CloseAnimationStateCloseHandler()
        {
            if (isBlockingInputOnOpenAndClose)
                EventManager_Input.Notify(this, new GameEventArgs(EventManager_Input.UNBLOCK_INPUT));
            if (windowObject)
                windowObject.SetActive(false);
        }
        #endregion

        public virtual void Handler_OpenWindow(object sender, GameEventArgs e)
        {
            if (e.str != id)
                return;
            if (sender.GetType() != typeof(UIControl))
                Debug.LogWarning($"Sender is not UIControl! Sender is {sender.ToString()}", this);
            SetState((int)WindowStates.OpenAnimation);
        }

        public virtual void Handler_CloseWindow(object sender, GameEventArgs e)
        {
            if (e.str != id)
                return;
            if (sender.GetType() != typeof(UIControl))
                Debug.LogWarning($"Sender is not UIControl! Sender is {sender.ToString()}", this);
            SetState((int)WindowStates.CloseAnimation);
        }

        public void Handler_HoldWindow(object sender, GameEventArgs e)
        {
            if (e.str != id)
                return;
            if (sender.GetType() != typeof(UIControl))
                Debug.LogWarning($"Sender is not UIControl! Sender is {sender.ToString()}", this);
            SetState((int)WindowStates.Holded);
        }

        public virtual void Handler_ResumeWindow(object sender, GameEventArgs e)
        {
            if (e.str != id)
                return;
            if (sender.GetType() != typeof(UIControl))
                Debug.LogWarning($"Sender is not UIControl! Sender is {sender.ToString()}", this);
            SetState((int)WindowStates.Active);
        }

        #endregion

        #region Show/Hide
        private IEnumerator Show()
        {
            yield return null;
            isAnimated = true;
            float scale = 0.01f;
            //GetContent();
            ScaleContent(scale);
            //SetContentAlpha(scale);
            if (windowObject)
                windowObject.SetActive(true);
            while (scale <= 1)
            {
                ScaleContent(scale);
                SetContentAlpha(scale);
                scale += scaleSpeed * Time.unscaledDeltaTime;
                yield return null;
            }
            scale = 1;
            ScaleContent(scale);
            //SetContentAlpha(scale);
            isAnimated = false;
        }

        private IEnumerator Hide()
        {
            isAnimated = true;
            float scale = 1f;
            while (scale >= 0.01f)
            {
                ScaleContent(scale);
                //SetContentAlpha(scale);
                scale -= scaleSpeed * Time.unscaledDeltaTime;
                yield return null;
            }
            if (windowObject)
                windowObject.SetActive(false);
            scale = 1;
            ScaleContent(scale);
            //SetContentAlpha(scale);
            isAnimated = false;
        }

        private void GetContent()
        {
            images = windowObject.GetComponentsInChildren<Image>(true);
            materials = windowObject.GetComponentsInChildren<Material>(true);
        }

        private void ScaleContent(float scale)
        {
            if (windowObject)
                windowObject.transform.localScale = Vector3.one * scale;
        }

        private void SetContentAlpha(float alpha)
        {
            if (images != null)
                for (int i = 0; i < images.Length; i++)
                {
                    Color col = images[i].color;
                    col.a = alpha;
                    images[i].color = col;
                }
            if (materials != null)
                for (int i = 0; i < materials.Length; i++)
                {
                    Color col = materials[i].color;
                    col.a = alpha;
                    materials[i].color = col;
                }
        }
        #endregion Show/Hide
        #endregion Methods
    }
}