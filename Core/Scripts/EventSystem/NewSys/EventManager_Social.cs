//Developed by Pavel Kravtsov.
using UnityEngine;
using System;

namespace Core.EventSystem
{
    public class EventManager_Social : MonoBehaviour
    {
        public static event EventHandler<GameEventArgs> SingedIn;
        public static event EventHandler<GameEventArgs> FailSignIn;
        public static event EventHandler<GameEventArgs> SignedOut;
        public static event EventHandler<GameEventArgs> DataLoaded;
        public static event EventHandler<GameEventArgs> FailDataLoading;

        public const string SIGNED_IN = "SignedIn";
        public const string FAIL_SIGN_IN = "FailedSignIn";
        public const string SIGNED_OUT = "SignedOut";

        public const string DATA_LOADED = "DataLoaded";
        public const string FAIL_DATA_LOADING = "FailDataLoading";

        public static void Subscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case SIGNED_IN:
                    SingedIn += handler;
                    break;
                case FAIL_SIGN_IN:
                    FailSignIn += handler;
                    break;
                case SIGNED_OUT:
                    SignedOut += handler;
                    break;
                case DATA_LOADED:
                    DataLoaded += handler;
                    break;
                case FAIL_DATA_LOADING:
                    FailDataLoading += handler;
                    break;
            }
        }

        public static void Unsubscribe(string type, EventHandler<GameEventArgs> handler)
        {
            switch (type)
            {
                case SIGNED_IN:
                    SingedIn -= handler;
                    break;
                case FAIL_SIGN_IN:
                    FailSignIn -= handler;
                    break;
                case SIGNED_OUT:
                    SignedOut -= handler;
                    break;
                case DATA_LOADED:
                    DataLoaded -= handler;
                    break;
                case FAIL_DATA_LOADING:
                    FailDataLoading -= handler;
                    break;
            }
        }

        public static void Notify(object sender, GameEventArgs e)
        {
            switch (e.type)
            {
                case SIGNED_IN:
                    SingedIn?.Invoke(sender, e);
                    break;
                case FAIL_SIGN_IN:
                    FailSignIn?.Invoke(sender, e);
                    break;
                case SIGNED_OUT:
                    SignedOut?.Invoke(sender, e);
                    break;
                case DATA_LOADED:
                    DataLoaded?.Invoke(sender, e);
                    break;
                case FAIL_DATA_LOADING:
                    FailDataLoading?.Invoke(sender, e);
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
                case SIGNED_IN:
                    SingedIn?.Invoke(null, new GameEventArgs(type));
                    break;
                case FAIL_SIGN_IN:
                    FailSignIn?.Invoke(null, new GameEventArgs(type));
                    break;
                case SIGNED_OUT:
                    SignedOut?.Invoke(null, new GameEventArgs(type));
                    break;
                case DATA_LOADED:
                    DataLoaded?.Invoke(null, new GameEventArgs(type));
                    break;
                case FAIL_DATA_LOADING:
                    FailDataLoading?.Invoke(null, new GameEventArgs(type));
                    break;
            }
        }

        public static void UnsubscribeAll()
        {
            SingedIn = null;
            FailSignIn = null;
            SignedOut = null;
            DataLoaded = null;
            FailDataLoading = null;
        }
    }
}
