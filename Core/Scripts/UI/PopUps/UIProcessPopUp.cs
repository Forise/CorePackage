//Developed by Pavel Kravtsov.
using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;

namespace Core
{
    public class UIProcessPopUp : UIPopUp
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private bool blockInputWhileOpened;
        [SerializeField]
        private float speed = 1f;
        private string[] closeEvents;

        protected override void ActiveStateUpdateHandler()
        {
            base.ActiveStateUpdateHandler();
            RotateImage();
        }

        protected override void OpenAnimationStateInitHandler()
        {
            base.OpenAnimationStateInitHandler();
            if (blockInputWhileOpened)
            {
                EventManager_Input.Notify(this, new GameEventArgs(EventManager_Input.BLOCK_INPUT));
            }
        }

        protected override void CloseAnimationStateCloseHandler()
        {
            base.CloseAnimationStateCloseHandler();
            if (blockInputWhileOpened)
            {
                EventManager_Input.Notify(this, new GameEventArgs(EventManager_Input.UNBLOCK_INPUT));
            }
        }

        #region Methods
        private void SubscribeAllCloseEvents()
        {
            foreach(var e in closeEvents)
            {
                EventManager_Window.Subscribe(e, Close_Handler);
                EventManager_Gameplay.Subscribe(e, Close_Handler);
                EventManager.Subscribe(e, Close_Handler);
            }
        }
        private void UnsubscribeAllCloseEvents()
        {
            foreach (var e in closeEvents)
            {
                EventManager_Window.Unsubscribe(e, Close_Handler);
                EventManager_Gameplay.Unsubscribe(e, Close_Handler);
                EventManager.Unsubscribe(e, Close_Handler);
            }
        }

        private void RotateImage()
        {
            image.transform.Rotate(Vector3.back * Time.deltaTime * speed);
        }

        protected override void SetContent()
        {
            closeEvents = popupData.closeEvents;
            SubscribeAllCloseEvents();
            base.SetContent();
        }
        #endregion

        #region Handlers
        public void Close_Handler(object sender, GameEventArgs e)
        {
            UnsubscribeAllCloseEvents();
            CloseThisWindow();        
        }
        #endregion
    }
}