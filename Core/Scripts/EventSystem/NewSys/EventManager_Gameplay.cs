//Developed by Pavel Kravtsov.
using UnityEngine;
using System;

namespace Core.EventSystem
{
    public class EventManager_Gameplay : MonoBehaviour
    {
        public static event EventHandler<GameEventArgs> StartGame;
        public static event EventHandler<GameEventArgs> PauseGame;
        public static event EventHandler<GameEventArgs> ResumeGame;
        public static event EventHandler<GameEventArgs> StopGame;
        public static event EventHandler<GameEventArgs> RestartGame;
        public static event EventHandler<GameEventArgs> StartMiniGame;
        public static event EventHandler<GameEventArgs> EndMiniGame;

        public const string START_GAME = "StartGame";
        public const string PAUSE_GAME = "PauseGame";
        public const string RESUME_GAME = "ResumeGame";
        public const string STOP_GAME = "StopGame";
        public const string RESTART_GAME = "RestartGame";
        public const string START_MINI_GAME = "StartMiniGame";
        public const string END_MINI_GAME = "EndMiniGame";

        public static void Subscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case START_GAME:
                    StartGame += handler;
                    break;
                case PAUSE_GAME:
                    PauseGame += handler;
                    break;
                case RESUME_GAME:
                    ResumeGame += handler;
                    break;
                case STOP_GAME:
                    StopGame += handler;
                    break;
                case RESTART_GAME:
                    RestartGame += handler;
                    break;
                case START_MINI_GAME:
                    StartMiniGame += handler;
                    break;
                case END_MINI_GAME:
                    EndMiniGame += handler;
                    break;
            }
        }

        public static void Unsubscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case START_GAME:
                    StartGame -= handler;
                    break;
                case PAUSE_GAME:
                    PauseGame -= handler;
                    break;
                case RESUME_GAME:
                    ResumeGame -= handler;
                    break;
                case STOP_GAME:
                    StopGame -= handler;
                    break;
                case RESTART_GAME:
                    RestartGame -= handler;
                    break;
                case START_MINI_GAME:
                    StartMiniGame -= handler;
                    break;
                case END_MINI_GAME:
                    EndMiniGame -= handler;
                    break;
            }
        }

        public static void Notify(object sender, GameEventArgs e)
        {
            switch (e.type)
            {
                case START_GAME:
                    StartGame?.Invoke(sender, e);
                    break;
                case PAUSE_GAME:
                    PauseGame?.Invoke(sender, e);
                    break;
                case RESUME_GAME:
                    ResumeGame?.Invoke(sender, e);
                    break;
                case STOP_GAME:
                    StopGame?.Invoke(sender, e);
                    break;
                case RESTART_GAME:
                    RestartGame?.Invoke(sender, e);
                    break;
                case START_MINI_GAME:
                    StartMiniGame?.Invoke(sender, e);
                    break;
                case END_MINI_GAME:
                    EndMiniGame?.Invoke(sender, e);
                    break;
            }
        }


        /// <summary>
        /// Use it for notify from EventTrigger Component. Requires to be component on GameObject!
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        public void Notify(string type)
        {
            switch (type)
            {
                case START_GAME:
                    StartGame?.Invoke(null, new GameEventArgs(type));
                    break;
                case PAUSE_GAME:
                    PauseGame?.Invoke(null, new GameEventArgs(type));
                    break;
                case RESUME_GAME:
                    ResumeGame?.Invoke(null, new GameEventArgs(type));
                    break;
                case STOP_GAME:
                    StopGame?.Invoke(null, new GameEventArgs(type));
                    break;
                case RESTART_GAME:
                    RestartGame?.Invoke(null, new GameEventArgs(type));
                    break;
                case START_MINI_GAME:
                    StartMiniGame?.Invoke(null, new GameEventArgs(type));
                    break;
                case END_MINI_GAME:
                    EndMiniGame?.Invoke(null, new GameEventArgs(type));
                    break;
            }
        }

        public static void UnsubscribeAll()
        {
            StartGame = null;
            StopGame = null;
            PauseGame = null;
            ResumeGame = null;
            RestartGame = null;
            StartMiniGame = null;
            EndMiniGame = null;
        }
    }
}