using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace Core
{
    public class AuthManager : MonoBehaviour
    {
        public static AuthManager I { get; set; }

        [Serializable]
        public class TokenRequestResult
        {
            public string accessToken;
            public int expiresIn;
        }
        [Header("Classes")]
        public TokenRequestResult BackTokenClass = new TokenRequestResult();

        public string Udid;
        public string FCMToken;

        //[Space(10)]
        [Header("FireBase's")]
        protected Firebase.Auth.FirebaseAuth Auth;
        protected Firebase.Auth.FirebaseAuth OtherAuth;
        Firebase.DependencyStatus _dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        protected Dictionary<string, Firebase.Auth.FirebaseUser> UserByAuth = new Dictionary<string, Firebase.Auth.FirebaseUser>();
        // Options used to setup secondary authentication object.
        private Firebase.AppOptions OtherAuthOptions = new Firebase.AppOptions
        {
            ApiKey = "",
            AppId = "",
            ProjectId = ""
        };

        //[Space(10)]
        [Header("Booleans")]
        [SerializeField]
        private bool _fetchingAuthToken = false;
        [SerializeField]
        private bool _fetchingBackToken;
        [SerializeField]
        private bool _sendingFCMToken;
        [SerializeField]
        private bool _gettingUpdatesList;
        [SerializeField]
        private bool _firebasePrepareInitStarted;

        //[Space(10)]
        [Header("URL's")]
        public string AppId;
        public string URLBackToken;
        public string URLSessions;
        public string URLUpdatesList;
        public string URLUpdateInfo;
        public string URLLevelInfo;
        [Space(5)]
        public string URLInfo;

        [Header("Results")]
        [TextArea(3, 10)]
        public string AuthToken; // большой токен от файрбейз
        public string BackToken; // токен который возвращает наш сервер
        public string GettedInfo;


        [Space(10)]
        [Header("UI")]
        private string _logText = "";
        public TextMeshProUGUI LogTextElement;

        public float InternetConnectionTimer = 11;
        public float InternetConnectionTimerLimit = 10;

        void Awake()
        {
            I = this;
            DontDestroyOnLoad(this);
        }

        public virtual void Start()
        {
            BackToken = GetBackToken();
        }


        public void Update()
        {
            if (LogTextElement != null)
                LogTextElement.text = _logText;

            if (GetBackToken() == "")
            {
                if (InternetConnectionTimer > InternetConnectionTimerLimit)
                {
                    InternetConnectionTimer = 0;
                    if (CheckConnection() && !_firebasePrepareInitStarted)
                    {
                        _firebasePrepareInitStarted = true;
                        PrepareFirebaseToInit();
                    }
                }
                else
                {
                    InternetConnectionTimer += Time.deltaTime;
                }
            }

            if (AuthToken != "" && GetBackToken() == "" && !_fetchingBackToken)
            {
                //Debug.Log("try get server token");
                _fetchingBackToken = true;
                StartCoroutine(TryGetServerToken());
            }
        }


        void OnDisable()
        {
            if (Auth == null) return;

            Auth.StateChanged -= AuthStateChanged;

            Auth.IdTokenChanged -= IdTokenChanged;
            Auth = null;

            if (OtherAuth != null)
            {
                OtherAuth.StateChanged -= AuthStateChanged;
                OtherAuth.IdTokenChanged -= IdTokenChanged;
                OtherAuth = null;
            }
        }

        public void PrepareFirebaseToInit()
        {
            //Debug.Log("PrepareFirebaseToInit");
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                _dependencyStatus = task.Result;
                if (_dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                //Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
            }
            });
        }

        protected void InitializeFirebase()
        {
            //DebugLog("Setting up Firebase Auth");
            Auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            Auth.StateChanged += AuthStateChanged;
            Auth.IdTokenChanged += IdTokenChanged;
            // Specify valid options to construct a secondary authentication object.
            if (OtherAuthOptions != null &&
                !(String.IsNullOrEmpty(OtherAuthOptions.ApiKey) ||
                  String.IsNullOrEmpty(OtherAuthOptions.AppId) ||
                  String.IsNullOrEmpty(OtherAuthOptions.ProjectId)))
            {
                try
                {
                    OtherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(OtherAuthOptions, "Secondary"));
                    OtherAuth.StateChanged += AuthStateChanged;
                    OtherAuth.IdTokenChanged += IdTokenChanged;
                }
                catch (Exception)
                {
                    //DebugLog("ERROR: Failed to initialize secondary authentication object.");
                }
            }
            AuthStateChanged(this, null);

            if (Auth.CurrentUser == null)
            {
                SigninAnonymouslyAsync();
            }
        }

        // Reload the currently logged in user.
        public void ReloadUser()
        {
            if (Auth.CurrentUser == null)
            {
                //DebugLog("Not signed in, unable to reload user.");
                return;
            }
            //DebugLog("Reload User Data");
            Auth.CurrentUser.ReloadAsync().ContinueWith(task =>
            {
                if (LogTaskCompletion(task, "Reload"))
                {
                    DisplayDetailedUserInfo(Auth.CurrentUser, 1);
                }
            });
        }

        public Task SigninAnonymouslyAsync()
        {
            //Debug.Log("Attempting to sign anonymously...");
            //DebugLog("Attempting to sign anonymously...");
            return Auth.SignInAnonymouslyAsync().ContinueWith(HandleSignInWithUser);
        }

        // Called when a sign-in without fetching profile data completes.
        void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
        {
            if (LogTaskCompletion(task, "Sign-in >> "))
            {
                ////DebugLog($"{task.Result.DisplayName} signed in");
                //DebugLog(String.Format("{0} signed in", task.Result.DisplayName));
            }
        }

        // Track state changes of the auth object.
        void AuthStateChanged(object sender, System.EventArgs eventArgs)
        {
            ////Debug.Log("<font color=green>AuthStateChangedAuthStateChanged</font>");
            //DebugLog("<font color=green>AuthStateChangedAuthStateChanged</font>");

            Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
            Firebase.Auth.FirebaseUser user = null;

            if (senderAuth != null) UserByAuth.TryGetValue(senderAuth.App.Name, out user);
            if (senderAuth == Auth && senderAuth != null && senderAuth.CurrentUser != user)
            {
                bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
                if (!signedIn && user != null)
                {
                    //DebugLog("Signed out " + user.UserId);
                }
                user = senderAuth.CurrentUser;
                UserByAuth[senderAuth.App.Name] = user;
                if (signedIn)
                {
                    //DebugLog("Signed in " + user.UserId);
                    DisplayDetailedUserInfo(user, 1);
                }
            }
        }

        // Display user information.
        protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel)
        {
            //string indent = new String(' ', indentLevel * 10);
            var userProperties = new Dictionary<string, string>
        {
            {"Display Name", userInfo.DisplayName},
            {"Email", userInfo.Email},
            {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
            {"Provider ID", userInfo.ProviderId},
            {"User ID", userInfo.UserId}
        };
            foreach (var property in userProperties)
            {
                if (!String.IsNullOrEmpty(property.Value))
                {
                    //DebugLog(String.Format("{0}{1}: {2}", "   ", property.Key, property.Value));
                }
            }
        }

        // Display a more detailed view of a FirebaseUser.
        protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel)
        {
            string indent = new String(' ', indentLevel * 2);
            DisplayUserInfo(user, indentLevel);
            //DebugLog(String.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
            //DebugLog(String.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
            //DebugLog(String.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
            var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
            var numberOfProviders = providerDataList.Count;
            if (numberOfProviders > 0)
            {
                for (int i = 0; i < numberOfProviders; ++i)
                {
                    //DebugLog(String.Format("{0}Provider Data: {1}", indent, i));
                    DisplayUserInfo(providerDataList[i], indentLevel + 2);
                }
            }
        }

        // Track ID token changes.
        void IdTokenChanged(object sender, System.EventArgs eventArgs)
        {

            //Debug.Log("IdTokenChanged");
            Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
            if (senderAuth == Auth && senderAuth.CurrentUser != null && !_fetchingAuthToken)
            {
                senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
                    task => DebugLog(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));

                GetUserToken();
            }
        }

        // Called when a sign-in with profile data completes.
        void HandleSignInWithSignInResult(Task<Firebase.Auth.SignInResult> task)
        {
            if (LogTaskCompletion(task, "Sign-in"))
            {
                DisplaySignInResult(task.Result, 1);
                //_signInProgress = false;
            }
        }

        // Display user information reported
        protected void DisplaySignInResult(Firebase.Auth.SignInResult result, int indentLevel)
        {
            string indent = new String(' ', indentLevel * 2);
            DisplayDetailedUserInfo(result.User, indentLevel);
            var metadata = result.Meta;
            if (metadata != null)
            {
                //DebugLog(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
                //DebugLog(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
            }
            var info = result.Info;
            if (info != null)
            {
                //DebugLog(String.Format("{0}Additional User Info:", indent));
                //DebugLog(String.Format("{0}  User Name: {1}", indent, info.UserName));
                //DebugLog(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
                DisplayProfile<string>(info.Profile, indentLevel + 1);
            }
        }

        // Display additional user profile information.
        protected void DisplayProfile<T>(IDictionary<T, object> profile, int indentLevel)
        {
            string indent = new String(' ', indentLevel * 2);
            foreach (var kv in profile)
            {
                var valueDictionary = kv.Value as IDictionary<object, object>;
                if (valueDictionary != null)
                {
                    //DebugLog(String.Format("{0}{1}:", indent, kv.Key));
                    DisplayProfile<object>(valueDictionary, indentLevel + 1);
                }
                else
                {
                    //DebugLog(String.Format("{0}{1}: {2}", indent, kv.Key, kv.Value));
                }
            }
        }

        // Log the result of the specified task, returning true if the task completed successfully, false otherwise.
        protected bool LogTaskCompletion(Task task, string operation)
        {
            bool complete = false;
            if (task.IsCanceled)
            {
                //DebugLog(operation + " canceled.");
            }
            else if (task.IsFaulted)
            {
                //DebugLog(operation + " encounted an error.");
                foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                {
                    string authErrorCode = "";
                    Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                    if (firebaseEx != null)
                    {
                        authErrorCode = String.Format("AuthError.{0}: ", ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                    }
                    //DebugLog(authErrorCode + exception.ToString());
                }
            }
            else if (task.IsCompleted)
            {
                //DebugLog(operation + " completed");
                complete = true;
            }
            return complete;
        }

        // Sign out the current user.
        public void SignOut()
        {
            //DebugLog("Signing out.");
            Auth.SignOut();
            //DebugLog("Signed out " + (Auth.CurrentUser == null ? " is NULL" : "NOT null"));
        }

        // Fetch and display current user's auth token.
        public void GetUserToken()
        {
            if (Auth.CurrentUser == null)
            {
                //DebugLog("Not signed in, unable to get token.");
                return;
            }
            //DebugLog("Fetching user token");
            _fetchingAuthToken = true;
            Auth.CurrentUser.TokenAsync(false).ContinueWith(task =>
            {
                _fetchingAuthToken = false;
                if (LogTaskCompletion(task, "User token fetch"))
                {
                    //DebugLog("Token >> = " + task.Result.Substring(0, 30));
                    AuthToken = task.Result;
                }
            });
        }

        // Display information about the currently logged in user.
        public void GetUserInfo()
        {
            if (Auth.CurrentUser == null)
            {
                //DebugLog("Not signed in, unable to get info.");
            }
            else
            {
                //DebugLog("Current user info:");
                DisplayDetailedUserInfo(Auth.CurrentUser, 1);
            }
        }

        public void DebugLog(string s)
        {
            _logText += s + "\n";
        }

        public void ButtonSignAnonym()
        {
            SigninAnonymouslyAsync();
        }

        int errorCount;
        IEnumerator TryGetServerToken()
        {
            WWWForm form = new WWWForm();
            form.AddField("clientId", AppId);
            form.AddField("idToken", AuthToken);
            string udid = SystemInfo.deviceUniqueIdentifier;
            form.AddField("udid", udid);
            Udid = udid;
            DebugLog("<color=green>SendToURL:</color> " + URLBackToken + "\n" + string.Format(" <color=green>clientId:</color>{0}", AppId) + "\n");

            Debug.Log("GetServerToken");
            UnityWebRequest wwwRequest = UnityWebRequest.Post(URLBackToken, form);
            yield return wwwRequest.SendWebRequest();

            if (wwwRequest.error != null)
            {
                DebugLog("Eroor requesting BackTokenZ: " + wwwRequest.error + "\n");
                Debug.Log("Eroor requesting BackTokenZ: " + wwwRequest.error + "\n");
                Debug.Log("Eroor requesting BackTokenZ: " + wwwRequest.responseCode + "\n");

                if (errorCount++ < 20)
                {
                    _fetchingBackToken = false;
                }
                if (errorCount <= 1)
                {
                    //StartCoroutine(SendToTelegram("error getting back token"));
                }

            }
            else
            {
                Debug.Log(wwwRequest.responseCode + "/" + wwwRequest.downloadHandler.text + "\n");
                JsonUtility.FromJsonOverwrite(wwwRequest.downloadHandler.text, BackTokenClass);
                BackToken = BackTokenClass.accessToken;
                SaveBackToken(BackToken);
                //StartCoroutine(SendToTelegram("\n BackToken ==> " + BackToken));
                DebugLog("<color=purple>BackToken:</color> " + BackToken + "\n");
                _fetchingBackToken = false;
            }
        }


        public void SaveBackToken(String BackToken)
        {
            PlayerPrefs.SetString("BackToken", BackToken);
        }

        public string GetBackToken()
        {
            return PlayerPrefs.GetString("BackToken", "");
        }

        public bool CheckConnection()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

    }
}