//Developed by Pavel Kravtsov.
#if UNITY_IOS //&& !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using Core.EventSystem;

namespace Core
{
    public class UserIdentifierIOS : IUserIdentifier
    {
        delegate void AuthenticateSuccess_Handler(string userID, string userName, string underage, string isAuthenticated, string userAlias);
        delegate void AuthenticateFailed_Handler(string value);
        delegate void AvatarDownloaded_Handler(byte[] data);
        delegate void DataLoaded_Handler(string json);
        delegate void LoadDataFailed_Handler(string error);
        delegate void DataSaved_Handler(string callbak);
        delegate void SaveDataFailed_Handler(string error);
        delegate void IsSignedIn_Handler(string isSignedIn);

        [DllImport("__Internal")]
        private static extern void GetAvatar(AvatarDownloaded_Handler AuthenticateSuccess_Callback);
        [DllImport("__Internal")]
        private static extern void AuthenticateLocalPlayer
            (AuthenticateSuccess_Handler AuthenticateSuccess_Callback,
            AuthenticateFailed_Handler AuthenticateFailed_Callback);
        [DllImport("__Internal")]
        private static extern void SaveUserDataToGameCenter
            (DataSaved_Handler dataSaved_Callback,
            SaveDataFailed_Handler saveDataFailed_Callback, string json);
        [DllImport("__Internal")]
        private static extern void LoadUserData(DataLoaded_Handler dataLoaded_Callback, LoadDataFailed_Handler loadDataFailed_Callback);
        [DllImport("__Internal")]
        private static extern string CheckSigningIn(IsSignedIn_Handler isSignedIn_Callback);

        private static string onlineId;
        private static string userName;
        private static string alias;
        private static bool canSave = false;
        private const int MAX_AVATAR_REQUEST_ATTEMPTS = 3;
        private static Texture2D avatar = new Texture2D(100, 100);

        public event UserIdentifierUpdated OnUserUpdated;

        private static bool isSignedIn = false;
        private static bool isRequestingProfile = false;
        private static bool isUnderage = false;

#region Properties
        public string Identifier
        {
            get { return onlineId; }
        }

        public string Name
        {
            get { return alias; }
            set { }
        }

        public Texture2D Avatar
        {
            get { return avatar; }
        }

        public bool CanSave
        {//TODO: fix it
            get { return true; }
        }

        public bool IsSignedIn
        {
            get
            {
                //Debug.Log("Is signed in property get started");
                //CheckSigningIn(IsSignedIn_Callback);
                //Debug.Log("Signed In Callback");
                return isSignedIn;
            }
        }

        public bool IsCheckingCanPlayOnline
        {
            get { return isRequestingProfile; }
        }

        public bool CanPlayOnline
        {
            get
            {
                if (!isSignedIn || !Utilities.HasConnection())
                    return false;
                else
                    return true;
            }
        }
#endregion

        public UserIdentifierIOS()
        {
            //Debug.Log("UserIdentifierIOS()");
            AuthenticateGameCenterUser();
        }

        public UserIdentifierIOS(string userID, string name, bool underage, bool isAuthenticated, string userAlias, bool canSave)
        {
            onlineId = userID;
            userName = name;
            isUnderage = underage;
            isSignedIn = isAuthenticated;
            alias = userAlias;
        }

        public void LoadUserDataFromGameCanter()
        {
            //Debug.Log(GetType().Name+ ": " + "Start Loading data from GC");
            //UIControl.Instance.ShowProcessPopUp(new UIPopUp.PopupData("Loading data...",))
            LoadUserData(DataLoaded_Callback, LoadDataFailed_Callback);
        }

        public void SaveUserData(string json)
        {
            SaveUserDataToGameCenter(DataSaved_Callback, SaveDataFailed_Callback, json);
        }

        private void AuthenticateGameCenterUser()
        {
#if !UNITY_EDITOR
        if (Utilities.HasConnection())
        {
            AuthenticateLocalPlayer(AuthenticateSuccess_Callback, AuthenticateFailed_Callback);
            //Debug.Log("NATIVE AUTHENTICATE IS CALLED FROM " + this.Name);
        }
#endif
        }

        public bool Equals(IUserIdentifier other)
        {
            if (other != null)
                return onlineId.Equals(other.Identifier);
            else
                return false;
        }

        private void NotifyUpdated()
        {
            if (OnUserUpdated != null)
                OnUserUpdated(this);
        }

        private void RefreshAvatar()
        {
            // first set the default avatar, so we have a fallback
            avatar = new Texture2D(100, 100);
            NotifyUpdated();
        }

#region Native code Handlers
        //private delegate void GotAuthenticationDelegate();
        //private static event GotAuthenticationDelegate GotAuth;
        [MonoPInvokeCallback(typeof(AuthenticateSuccess_Handler))]
        private static void AuthenticateSuccess_Callback(string userID, string name, string underage, string isAuthenticated, string userAlias)
        {
            //GotAuth += CreateLocalVars;
            onlineId = userID;
            userName = name;
            isUnderage = (underage == "true");
            isSignedIn = (isAuthenticated == "true");
            alias = userAlias;

            //Debug.Log("UserIdentifierIOS.AuthenticateSuccess_Callback() \n"
            //+"ID = " + userID + "\n"
            //+ "NAME = " + userName + "\n"
            //+ "UNDERAGE = " + isUnderage + "\n"
            //+ "SIGNED IG = " + isSignedIn + "\n"
            //+ "ALIAS = " + alias + "\n"
            //+ "CAN SAVE = " + canSave);
            //GetAvatar(AvatarDownloaded_Callback);
            //GotAuth();
            EventManager.Notify(new GameEvent(GameCenterEvents.SIGNED_IN), null);
        }

        //private static void CreateLocalVars()
        //{
        //    GameCenterManager.Instance.UserIdentifier = new UserIdentifierIOS(onlineId, userName, isUnderage, isSignedIn, alias, canSave);
        //    GotAuth -= CreateLocalVars;
        //}

        [MonoPInvokeCallback(typeof(AuthenticateFailed_Handler))]
        private static void AuthenticateFailed_Callback(string value)
        {
            //Debug.Log("UserIdentifierIOS.AuthenticateFailed_Callback() " + value);
        }
        [MonoPInvokeCallback(typeof(AuthenticateFailed_Handler))]
        private static void AvatarDownloaded_Callback(byte[] data)
        {
            //Debug.Log("FILE PATH = " + data);
            //byte[] imageBytes = File.ReadAllBytes(value);
            Texture2D unityTexture = new Texture2D(100, 100);
            avatar = unityTexture;
            //Debug.Log("UserIdentifierIOS.AvatarDownloaded_Callback() " + data);
        }
        [MonoPInvokeCallback(typeof(DataSaved_Handler))]
        private static void DataSaved_Callback(string callback)
        {
            //Debug.Log("DataSaved: " + callback);
        }
        [MonoPInvokeCallback(typeof(SaveDataFailed_Handler))]
        private static void SaveDataFailed_Callback(string error)
        {
            //Debug.Log("Save Data Failed: " + error);
        }
        [MonoPInvokeCallback(typeof(DataLoaded_Handler))]
        private static void DataLoaded_Callback(string json)
        {
            //Debug.Log("Loaded JSON from GC: " + json);
            EventManager.Notify(new GameEvent(GameCenterEvents.DATA_LOADED, json), null);
        }
        [MonoPInvokeCallback(typeof(LoadDataFailed_Handler))]
        private static void LoadDataFailed_Callback(string error)
        {
            //Debug.Log("Load Data Failed: " + error);
        }
        [MonoPInvokeCallback(typeof(IsSignedIn_Handler))]
        private static void IsSignedIn_Callback(string answer)
        {
            isSignedIn = (answer == "true" ? true : false);
        }
#endregion

    }
}
#endif