//Developed by Pavel Kravtsov.
using UnityEngine;
using System;

namespace Core.EventSystem
{
    public class EventManager_Window : MonoBehaviour
    {
        public static event EventHandler<GameEventArgs> OpenWindow;
        public static event EventHandler<GameEventArgs> CloseWindow;
        public static event EventHandler<GameEventArgs> HoldWindow;
        public static event EventHandler<GameEventArgs> ResumeWindow;
        public static event EventHandler<GameEventArgs> OnWindowOpened;
        public static event EventHandler<GameEventArgs> OnWindowClosed;
        public static event EventHandler<GameEventArgs> OnWindowHolded;
        public static event EventHandler<GameEventArgs> OnWindowResumed;

        public const string OPEN_WINDOW = "OpenWindow";
        public const string CLOSE_WINDOW = "CloseWindow";
        public const string HOLD_WINDOW = "HoldWindow";
        public const string RESUME_WINDOW = "ResumeWindow";
        public const string WINDOW_OPENED = "WindowOpened";
        public const string WINDOW_CLOSED = "WindowClosed";
        public const string WINDOW_HOLDED = "WindowHolded";
        public const string WINDOW_RESUMED = "WindowResumed";

        public static void Subscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case OPEN_WINDOW:
                    OpenWindow += handler;
                    break;
                case CLOSE_WINDOW:
                    CloseWindow += handler;
                    break;
                case HOLD_WINDOW:
                    HoldWindow += handler;
                    break;
                case RESUME_WINDOW:
                    ResumeWindow += handler;
                    break;
                case WINDOW_OPENED:
                    OnWindowOpened += handler;
                    break;
                case WINDOW_CLOSED:
                    OnWindowClosed += handler;
                    break;
                case WINDOW_HOLDED:
                    OnWindowHolded += handler;
                    break;
                case WINDOW_RESUMED:
                    OnWindowResumed += handler;
                    break;
            }
        }

        public static void Unsubscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case OPEN_WINDOW:
                    OpenWindow -= handler;
                    break;
                case CLOSE_WINDOW:
                    CloseWindow -= handler;
                    break;
                case HOLD_WINDOW:
                    HoldWindow -= handler;
                    break;
                case RESUME_WINDOW:
                    ResumeWindow -= handler;
                    break;
                case WINDOW_OPENED:
                    OnWindowOpened -= handler;
                    break;
                case WINDOW_CLOSED:
                    OnWindowClosed -= handler;
                    break;
                case WINDOW_HOLDED:
                    OnWindowHolded -= handler;
                    break;
                case WINDOW_RESUMED:
                    OnWindowResumed -= handler;
                    break;
            }
        }

        public static void Notify(object sender, GameEventArgs e)
        {
            switch(e.type)
            {
                case OPEN_WINDOW:
                    OpenWindow?.Invoke(sender, e);
                    break;
                case CLOSE_WINDOW:
                    CloseWindow?.Invoke(sender, e);
                    break;
                case HOLD_WINDOW:
                    HoldWindow?.Invoke(sender, e);
                    break;
                case RESUME_WINDOW:
                    ResumeWindow?.Invoke(sender, e);
                    break;
                case WINDOW_OPENED:
                    OnWindowOpened?.Invoke(sender, e);
                    break;
                case WINDOW_CLOSED:
                    OnWindowClosed?.Invoke(sender, e);
                    break;
                case WINDOW_HOLDED:
                    OnWindowHolded?.Invoke(sender, e);
                    break;
                case WINDOW_RESUMED:
                    OnWindowResumed?.Invoke(sender, e);
                    break;
            }
        }

        public static void UnsubscribeAll()
        {
            OpenWindow = null;
            CloseWindow = null;
            HoldWindow = null;
            ResumeWindow = null;
            OnWindowOpened = null;
            OnWindowClosed = null;
            OnWindowHolded = null;
            OnWindowResumed = null;
        }
    }
}