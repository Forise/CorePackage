//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;

namespace Core
{
    public class UIControl : MonoSingleton<UIControl>
    {
        #region Fields
        private UIWindow currentOpenedWindow = null;
        private List<UIWindow> openedWindows = new List<UIWindow>();
        [SerializeField]
        private UIProcessPopUp processPopUp;
        [SerializeField]
        private UIInfoPopUp infoPopUp;
        [SerializeField]
        private UIImageInfoPopUp imageInfoPopUp;
        [SerializeField]
        private UIDialoguePopUp dialoguePopUp;
        [SerializeField]
        private GameObject fps;
        #endregion

        #region Properties
        #endregion

        private void Start()
        {
            EventManager_Window.Subscribe(EventManager_Window.WINDOW_OPENED, Handler_WindowOpened);
            EventManager_Window.Subscribe(EventManager_Window.WINDOW_CLOSED, Handler_WindowClosed);
        }

        protected override void OnDestroy()
        {
            EventManager_Window.Unsubscribe(EventManager_Window.WINDOW_OPENED, Handler_WindowOpened);
            EventManager_Window.Unsubscribe(EventManager_Window.WINDOW_CLOSED, Handler_WindowClosed);
            base.OnDestroy();
        }

        #region Methods
        public bool IsOpenedWindow(string windowID)
        {
            bool result = false;
            foreach (var window in openedWindows.ToArray())
                if (window.ID == windowID)
                {
                    result = true;
                    break;
                }
            return result;
        }

        public bool IsTopWindow(string windowID)
        {
            return currentOpenedWindow.ID == windowID;
        }

        public void OpenWindow(string windowID)
        {
            EventManager_Window.Notify(this, new GameEventArgs(EventManager_Window.OPEN_WINDOW, windowID));
        }

        public void OpenWindow(UIWindow window)
        {
            EventManager_Window.Notify(this, new GameEventArgs(EventManager_Window.OPEN_WINDOW, window.ID));
        }

        public void OpenWindow(string windowID, object sender)
        {
            EventManager_Window.Notify(sender, new GameEventArgs(EventManager_Window.OPEN_WINDOW, windowID));
        }

        public void OpenWindow(UIWindow window, object sender)
        {
            EventManager_Window.Notify(sender, new GameEventArgs(EventManager_Window.OPEN_WINDOW, window.ID));
        }

        public void ShowDialogPopUp(UIPopUp.PopupData popupData)
        {
            dialoguePopUp.Open(popupData);
        }

        public void ShowProcessPopUp(UIPopUp.PopupData popupData, float time = -1f)
        {
            processPopUp.Open(popupData, time);
        }

        public void ShowInfoPopUp(UIPopUp.PopupData popupData)
        {
            infoPopUp.Open(popupData);
        }

        public void ShowImageInfoPopUp(UIPopUp.PopupData popupData)
        {
            imageInfoPopUp.Open(popupData);
        }

        public void CloseWindow(string windowID)
        {
            EventManager_Window.Notify(this, new GameEventArgs(EventManager_Window.CLOSE_WINDOW, windowID));
        }

        public void CloseWindow(UIWindow window)
        {
            EventManager_Window.Notify(this, new GameEventArgs(EventManager_Window.CLOSE_WINDOW, window.ID));
        }
        #endregion

        #region Handlers
        public void Handler_WindowOpened(object sender, GameEventArgs e)
        {
            if (sender is UIWindow/* && !(sender is UIPopUp)*/)
            {
                currentOpenedWindow = sender as UIWindow;
                openedWindows.Add(sender as UIWindow);
            }
        }
        public void Handler_WindowClosed(object sender, GameEventArgs e)
        {
            openedWindows.Remove(openedWindows.Find(window => window.ID == e.str));
            if (openedWindows.Count == 0)
                currentOpenedWindow = null;
            else
                currentOpenedWindow = openedWindows[openedWindows.Count - 1];
        }
        #endregion
    }
}