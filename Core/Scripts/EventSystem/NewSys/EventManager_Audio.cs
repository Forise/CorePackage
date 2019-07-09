using UnityEngine;
using System;

namespace Core.EventSystem
{
    public class EventManager_Audio : MonoBehaviour
    {
        public static event EventHandler<GameEventArgs> PlayMainMusic;
        public static event EventHandler<GameEventArgs> UITapped;

        public const string PLAY_MAIN_MUSIC = "PlayMainMusic";
        public const string UI_TAPPED = "UITapped";

        public static void Subscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case PLAY_MAIN_MUSIC:
                    PlayMainMusic += handler;
                    break;
                case UI_TAPPED:
                    UITapped += handler;
                    break;
            }
        }

        public static void Unsubscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case PLAY_MAIN_MUSIC:
                    PlayMainMusic -= handler;
                    break;
                case UI_TAPPED:
                    UITapped -= handler;
                    break;
            }
        }

        public static void Notify(object sender, GameEventArgs e)
        {
            switch (e.type)
            {
                case PLAY_MAIN_MUSIC:
                    PlayMainMusic?.Invoke(sender, e);
                    break;
                case UI_TAPPED:
                    UITapped?.Invoke(sender, e);
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
                case PLAY_MAIN_MUSIC:
                    PlayMainMusic?.Invoke(null, new GameEventArgs(type));
                    break;
                case UI_TAPPED:
                    UITapped?.Invoke(null, new GameEventArgs(type));
                    break;
            }
        }

        public static void UnsubscribeAll()
        {
            PlayMainMusic = null;
            UITapped = null;
        }
    }
}