//Developed by Pavel Kravtsov.
using UnityEngine;
using System;

namespace Core.EventSystem
{
    public class EventManager_Input : MonoBehaviour
    {
        public static event EventHandler<GameEventArgs> BlockInput;
        public static event EventHandler<GameEventArgs> UnblockInput;
        public static event EventHandler<GameEventArgs> BlockHUD;
        public static event EventHandler<GameEventArgs> UnblockHUD;
        public static event EventHandler<GameEventArgs> HideHUD;
        public static event EventHandler<GameEventArgs> ShowHUD;

        public const string BLOCK_HUD = "BlockHUD";
        public const string UNBLOCK_HUD = "UnblockHUD";
        public const string HIDE_HUD = "HideHUD";
        public const string SHOW_HUD = "ShowHUD";
        public const string BLOCK_INPUT = "BlockInput";
        public const string UNBLOCK_INPUT = "UnblockInput";
        public const string UI_TAPPED = "UITapped";

        public static void Subscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case BLOCK_INPUT:
                    BlockInput += handler;
                    break;
                case UNBLOCK_INPUT:
                    UnblockInput += handler;
                    break;
                case BLOCK_HUD:
                    BlockHUD += handler;
                    break;
                case UNBLOCK_HUD:
                    UnblockHUD += handler;
                    break;
                case HIDE_HUD:
                    HideHUD += handler;
                    break;
                case SHOW_HUD:
                    ShowHUD += handler;
                    break;
            }
        }

        public static void Unsubscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case BLOCK_INPUT:
                    BlockInput -= handler;
                    break;
                case UNBLOCK_INPUT:
                    UnblockInput -= handler;
                    break;
                case BLOCK_HUD:
                    BlockHUD -= handler;
                    break;
                case UNBLOCK_HUD:
                    UnblockHUD -= handler;
                    break;
                case HIDE_HUD:
                    HideHUD -= handler;
                    break;
                case SHOW_HUD:
                    ShowHUD -= handler;
                    break;
            }
        }

        public static void Notify(object sender, GameEventArgs e)
        {
            switch (e.type)
            {
                case BLOCK_INPUT:
                    BlockInput?.Invoke(sender, e);
                    break;
                case UNBLOCK_INPUT:
                    UnblockInput?.Invoke(sender, e);
                    break;
                case BLOCK_HUD:
                    BlockHUD?.Invoke(sender, e);
                    break;
                case UNBLOCK_HUD:
                    UnblockHUD?.Invoke(sender, e);
                    break;
                case HIDE_HUD:
                    HideHUD?.Invoke(sender, e);
                    break;
                case SHOW_HUD:
                    ShowHUD?.Invoke(sender, e);
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
                case BLOCK_INPUT:
                    BlockInput?.Invoke(null, new GameEventArgs(type));
                    break;
                case UNBLOCK_INPUT:
                    UnblockInput?.Invoke(null, new GameEventArgs(type));
                    break;
                case BLOCK_HUD:
                    BlockHUD?.Invoke(null, new GameEventArgs(type));
                    break;
                case UNBLOCK_HUD:
                    UnblockHUD?.Invoke(null, new GameEventArgs(type));
                    break;
                case HIDE_HUD:
                    HideHUD?.Invoke(null, new GameEventArgs(type));
                    break;
                case SHOW_HUD:
                    ShowHUD?.Invoke(null, new GameEventArgs(type));
                    break;
            }
        }

        public static void UnsubscribeAll()
        {
            BlockInput = null;
            UnblockInput = null;
            BlockHUD = null;
            UnblockHUD = null;
            ShowHUD = null;
            HideHUD = null;
        }
    }
}