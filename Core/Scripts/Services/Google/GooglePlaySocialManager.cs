//Developed by Pavel Kravtsov.
#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Core.EventSystem;

namespace Core.GooglePlayServices
{
    public class GooglePlaySocialManager : MonoSingleton<GooglePlaySocialManager>
    {
        private bool isSaving = false;
        private bool isLoading = false;
        private bool isInited = false;
        private string currentEmail;
        public string CurrentEmail
        {
            get { return currentEmail; }
        }

        public bool IsLoading
        {
            get { return isLoading; }
        }

        public bool IsInited
        {
            get { return isInited; }
        }

        private void Awake()
        {
            if (SettingsControl.Instance.settings.saveUserDataToCloud)
            {
                Init();
            }
        }

        public void Init()
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .RequestEmail()
            .Build();
            PlayGamesPlatform.InitializeInstance(config);
            //PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            Login();
            isInited = true;
        }

        public void Login()
        {
            //Debug.Log(Social.Active.localUser.userName);
            //UIControl.Instance.ShowProcessPopUp(new UIPopUp.PopupData("Signing in...",
            //    new string[2] { GooglePlayServicesEvents.FAIL_SIGN_IN, GooglePlayServicesEvents.SIGNED_IN }), 7f);
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                Debug.Log("Already Logged In");
                PrintAll();
                currentEmail = ((PlayGamesLocalUser)Social.localUser).Email;
                if (!string.IsNullOrEmpty(currentEmail))
                    UserDataControl.Instance.UserData.EMAIL = currentEmail;
                //AnalyticsManager.Instance.OnOffAnalytics();
                EventManager.Notify(this, new GameEventArgs(Events.SocialEvents.SIGNED_IN));
                return;
            }
            else
            {
                PlayGamesPlatform.Instance.Authenticate(Social.localUser, (bool success) =>
                {
                    if (success)
                    {
                        Debug.Log("LOGIN SUCCESS!");
                        PrintAll();
                        currentEmail = ((PlayGamesLocalUser)Social.localUser).Email;
                        if (!string.IsNullOrEmpty(currentEmail))
                            UserDataControl.Instance.UserData.EMAIL = currentEmail;
                        //AnalyticsManager.Instance.OnOffAnalytics();
                        EventManager.Notify(this, new GameEventArgs(Events.SocialEvents.SIGNED_IN));
                    }
                    else
                    {
                        EventManager.Notify(this, new GameEventArgs(Events.SocialEvents.FAIL_SIGN_IN));
                        Debug.Log("LOGIN FAILED!");
                    }
                });
            }
        }

        public void SignOut()
        {
            UserDataControl.Instance.SaveData();
            PlayGamesPlatform.Instance.SignOut();
            EventManager.Notify(this, new GameEventArgs(Events.SocialEvents.SIGNED_OUT));
        }

        private void OnApplicationQuit()
        {
            if (SettingsControl.Instance.settings.saveUserDataToCloud)
                SignOut();
        }

        #region LoadData
        public void LoadData(string fileName)
        {
            if (Social.localUser.authenticated)
            {
                if (SettingsControl.Instance.settings.saveUserDataToCloud)
                {
                    UIControl.Instance.ShowProcessPopUp(new UIPopUp.PopupData("loading",
                        new string[2] { Events.SocialEvents.DATA_LOADED, Events.SocialEvents.FAIL_DATA_LOADING }));
                    isLoading = true;
                    OpenSavedGame(fileName);
                }
            }
            //else Login();
        }
        #endregion

        #region SaveData
        public void SaveData(string fileName)
        {
            if (SettingsControl.Instance.settings.saveUserDataToCloud)
            {
                isSaving = true;
                OpenSavedGame(fileName);
            }
        }
        #endregion

        #region OpenGame
        private void OpenSavedGame(string filename)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }

        public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                Debug.Log("OnSavedGameOpened SUCCESS!");
                if (isLoading)
                {
                    ReadGameData(game);
                }
                if (isSaving)
                {
                    Debug.Log("JSON TO GOOGLE: " + UserDataControl.Instance.UserData.JsonUD);
                    WriteGame(game, System.Text.Encoding.UTF8.GetBytes(UserDataControl.Instance.UserData.JsonUD));
                }
            }
            else
            {
                isLoading = false;
                isSaving = false;
                UserDataControl.Instance.isSaving = false;
                Debug.Log("OnSavedGameOpened ERROR!");
            }
        }
        #endregion

        #region WriteGame
        private void WriteGame(ISavedGameMetadata game, byte[] savedData)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            builder = builder.WithUpdatedDescription("Saved game at " + System.DateTime.Now);
            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
        }

        public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
        {
            Debug.Log($"OnSavedGameWritten {status.ToString()}");
            isSaving = false;
            UserDataControl.Instance.isSaving = false;
        }
        #endregion

        #region ReadGame
        private void ReadGameData(ISavedGameMetadata game)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
        }

        public void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                string json = System.Text.Encoding.UTF8.GetString(data);
                Debug.Log("JSON FROM GOOGLE: " + json);
                EventManager.Notify(this, new GameEventArgs(Events.SocialEvents.DATA_LOADED, json));
            }
            else
            {
                Debug.Log("OnSavedGameDataRead ERROR");
                EventManager.Notify(this, new GameEventArgs(Events.SocialEvents.FAIL_DATA_LOADING));
            }
            isLoading = false;
        }
        #endregion
        public string GetUserEMail()
        {
            return ((PlayGamesLocalUser)Social.localUser).Email;
        }

        private void PrintAll()
        {
            Debug.Log(string.Format("IS AUTHENTICATED: {0}", Social.localUser.authenticated));
            Debug.Log(string.Format("UserID: {0}", Social.localUser.id));
            Debug.Log(string.Format("E_MAIL: {0}", ((PlayGamesLocalUser)Social.localUser).Email));
        }
    }
#endif
}