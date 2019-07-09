using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Core
{
    public class PushManager : MonoSingleton<PushManager>
    {
        [SerializeField]
        private bool _fetchingFCMToken;
        [SerializeField]
        private bool _sendingFCMToken;
        [SerializeField]
        private string _FCMToken;
        [SerializeField]
        private bool _sendFCMToTelegram;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (!_fetchingFCMToken && GetFCMToken() == "" && (AuthManager.I.AuthToken != "" || AuthManager.I.BackToken != ""))
            {
                Debug.Log("Start fetch fcm");
                StartCoroutine(GetNewFCMToken());
                _fetchingFCMToken = true;
            }

            if (AuthManager.I.BackToken != "" && !IsFCMSent() && GetFCMToken() != "" && !_sendingFCMToken)
            {
                StartCoroutine(SendFCMTokenToServer(GetFCMToken()));
                _sendingFCMToken = true;
                //Debug.Log("Start send fcm");
            }

            if (_sendFCMToTelegram)
            {
                StartCoroutine(SendToTelegram("FCMToken " + _FCMToken + "\n" + SystemInfo.deviceUniqueIdentifier));
                _sendFCMToTelegram = false;
            }
        }


        public IEnumerator GetNewFCMToken()
        {
            Debug.Log("GetNewToken ");
            Task<string> t = Firebase.InstanceId.FirebaseInstanceId.DefaultInstance.GetTokenAsync();
            while (!t.IsCompleted) yield return new WaitForEndOfFrame();
            _FCMToken = t.Result;
            SaveFCMToken(_FCMToken);
        }

        public IEnumerator SendFCMTokenToServer(string token)
        {
            WWWForm form = new WWWForm();
            form.AddField("token", token);
            UnityWebRequest wwwRequest = UnityWebRequest.Post("https://api.backmobi.pro/player/register-fcm-token", form);
            wwwRequest.SetRequestHeader("Authorization", "Bearer " + AuthManager.I.BackToken);
            wwwRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return wwwRequest.SendWebRequest();
            if (!wwwRequest.isHttpError && !wwwRequest.isNetworkError && wwwRequest.responseCode == 200)
            {
                SaveFCMIsSent(true);
                //_sendFCMToTelegram = true;
            }
            else
            {
                //Debug.Log(wwwRequest.downloadHandler.text);
            }
        }

        public IEnumerator SendToTelegram(string message)
        {
            // WWWForm form = new WWWForm();
            // form.AddField("chat_id", "341374541");
            // form.AddField("text", message + "\n + #debugMessage");
            // UnityWebRequest wwwRequest = UnityWebRequest.Post("https://api.telegram.org/bot728884599:AAEGAcLn1odM3ql5Pk1cRAV-Yrrj3SkY54o/sendMessage", form);
            //Debug.Log("Test FCM token: " + message);
            yield return null;//wwwRequest.SendWebRequest();
        }

        public void SaveFCMToken(string FCMToken)
        {
            PlayerPrefs.SetString("FCMToken", FCMToken);
        }

        public string GetFCMToken()
        {
            return PlayerPrefs.GetString("FCMToken", "");
        }

        public void SaveFCMIsSent(bool isSent)
        {
            PlayerPrefs.SetInt("FCMIsSent", isSent ? 1 : 0);
        }

        public bool IsFCMSent()
        {
            return PlayerPrefs.GetInt("FCMIsSent", 0) == 1;
        }
    }
}