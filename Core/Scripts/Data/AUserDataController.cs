//Developed by Pavel Kravtsov.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;
using Core.GooglePlayServices;
using System.Threading.Tasks;

namespace Core
{
    public abstract class AUserDataController<K> : MonoSingleton<AUserDataController<K>> where K : AUserDataPresenter
    {
        [SerializeField]
        protected K userData;

        protected string path;
        public const string fileName = "UserData.json";

        protected bool isLoading;
        public bool isSaving;

        public bool DataWasLoaded
        { get; private set; }

        public virtual K UserData
        {
            get
            {
               return userData;
            }
            set
            {
                userData = value;
            }
        }

        protected virtual void Awake()
        {
            Init();
        }

        private void Start()
        {
            UserData.OnDataChanged += SaveLocal;
        }

        protected virtual void Init()
        {            

#if UNITY_ANDROID
            EventManager.Subscribe(Events.SocialEvents.DATA_LOADED, DataLoadedFromGoogle);
            EventManager.Subscribe(Events.SocialEvents.SIGNED_IN, UserSignedInGoogle);
#elif UNITY_IOS
            EventManager.Subscribe(Events.SocialEvents.DATA_LOADED, DataLoadedFromGameCenter);
            EventManager.Subscribe(Events.SocialEvents.SIGNED_IN, UserSignedInGameCenter);
#endif

#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            path = Application.persistentDataPath + "/" + fileName;
#elif UNITY_EDITOR
            path = Application.dataPath + "/" + fileName;
#endif
            LoadLocal();

            Debug.Log(UserData.JsonUD);
        }

        public void SaveLocal()
        {
            if (System.IO.File.Exists(path))
            {
                UserData.StringDateTime = System.DateTime.Now.ToString();
            }
            else
            {
                UserData.StringDateTime = System.DateTime.MinValue.ToString();
            }
            var json = UserData.JsonUD;
            System.IO.File.WriteAllText(path, json);
        }

        public void SaveData()
        {
            if (!isSaving)
            {
                isSaving = true;
                SaveLocal();
                //Debug.Log("Local Data Saved");
                ////Debug.Log(json);
#region SaveToCloud
                if (SettingsControl.Instance.settings.saveUserDataToCloud)
                {
#if UNITY_IOS && !UNITY_EDITOR
                    try
                    {        
                        if (GameCenterManager.Instance.UserIdentifier == null)
                            GameCenterManager.Instance.UserIdentifier = new UserIdentifierIOS();
                        //Debug.Log("Start Checking for save to GC");
                        if (GameCenterManager.Instance.UserIdentifier.IsSignedIn && System.DateTime.Parse(UserData.StringDateTime) > System.DateTime.MinValue)
                        {
                            //Debug.Log("Start Saving user data to game center from: " + this.GetType().Name);
                            var json = JsonUtility.ToJson(userData);
                            //Debug.Log("JSON to save: " + json);
                            GameCenterManager.Instance.UserIdentifier.SaveUserData(json);
                        }
                    }
                    catch(System.Exception ex)
                    {
                        Debug.Log(ex, this);
                    }

#elif UNITY_ANDROID && !UNITY_EDITOR
                    try
                    {
                        Debug.Log("Saving to google");
                        if (Social.localUser.authenticated && System.DateTime.Parse(userData.StringDateTime) != System.DateTime.MinValue)
                        {
                            Task saveDataTask = Task.Run(() =>
                            {
                                GooglePlaySocialManager.Instance.SaveData(fileName);

                                if (Application.internetReachability != NetworkReachability.NotReachable && Social.localUser.authenticated && System.DateTime.Parse(userData.StringDateTime) != System.DateTime.MinValue)
                                {
                                    GooglePlaySocialManager.Instance.SaveData(fileName);
                                }
                            });
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex, this);
                    }
#endif
                }
                #endregion

#if UNITY_EDITOR
                isSaving = false;
#endif
            }
        }

        public string LoadJson()
        {
            if (System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllText(path);
            }
            else
                return string.Empty;
        }

        public abstract void LoadLocal();

        protected abstract void SetNewData(string json);

        #region Handlers
        protected void FailDataLoading(object sender, GameEventArgs e)
        {
            isLoading = false;
            Debug.Log(e.str);
        }

        protected void UserSignedInGoogle(object sender, GameEventArgs e)
        {
            if (!isLoading)
            {
                isLoading = true;
#if UNITY_ANDROID && !UNITY_EDITOR
                //Debug.Log("Event notyfied: " + e.type);
                if(!DataWasLoaded)
                    GooglePlaySocialManager.Instance.LoadData(fileName);
#endif
                isLoading = false;
            }
        }

        protected void UserSignedInGameCenter(object sender, GameEventArgs e)
        {
            if (!isLoading)
            {
                isLoading = true;
#if UNITY_IOS && !UNITY_EDITOR
            //Debug.Log("Event notyfied: " + e.type);
            if (!DataWasLoaded)
                GameCenterManager.Instance.UserIdentifier.LoadUserDataFromGameCanter();
#endif
            }
        }

        protected void DataLoadedFromGameCenter(object sender, GameEventArgs e)
        {
#if UNITY_IOS
            //Debug.Log("Event notyfied: " + e.type);
            var json = e.str;
            if (!string.IsNullOrEmpty(json))
            {
                var newUserData = JsonUtility.FromJson<UserDataModel>(json);

                //Debug.Log(string.Format("Old time stamp: {0}; New time stamp: {1}", UserData.stringDateTime, newUserData.stringDateTime));

                if (System.DateTime.Parse(newUserData.StringDateTime) > System.DateTime.Parse(UserData.StringDateTime))
                {
                    SetNewData(json);
                    DataWasLoaded = true;
                }
                else
                    SaveData();
            }
            else
            {
                //Debug.Log("JSON is null or empty");
                SaveData();
            }
#endif
            EventManager.Notify(this, new GameEventArgs(Events.SaveData.NEW_DATA_APPLIED));
            isLoading = false;
        }
        protected void DataLoadedFromGoogle(object sender, GameEventArgs e)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var json = e.str;
            if (!string.IsNullOrEmpty(json))
            {
                var newUserData = JsonUtility.FromJson<UserDataModel>(json);
                Debug.Log(string.Format("Old time stamp: {0}; New time stamp: {1}", UserData.StringDateTime, newUserData.StringDateTime));
                if (System.DateTime.Parse(newUserData.StringDateTime) > System.DateTime.Parse(UserData.StringDateTime))
                {
                    SetNewData(json);
                    DataWasLoaded = true;
                }
                else
                    SaveData();
            }
            else
                SaveData();
#endif
            isLoading = false;
            EventManager.Notify(this, new GameEventArgs(Events.SaveData.NEW_DATA_APPLIED));
        }
        #endregion Handlers
    }
}