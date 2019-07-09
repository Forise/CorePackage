//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Core.EventSystem;
using UnityEngine.Networking;

namespace Core
{
    public class TimeManager : MonoSingleton<TimeManager>
    {
        private static event Action tickEvent;
        public static event Action TickEvent
        {
            add
            {
                tickEvent += value;
            }
            remove
            {
                if (tickEvent != null)
                    tickEvent -= value;
            }
        }
        #region Prorerties
        public static bool ServerTimeInited { get; private set; } = false;
        public static float SlowMotionFactor { get; private set; }
        public static bool IsSlowMotion { get; private set; } = false;
        public static DateTime UrmobiServerDT { get; private set; }
        public static DateTime LocalUrmobiServerDT { get; private set; }
        public static DateTime NIST_Date { get; private set; }
        public static DateTime Local_NIST_Date { get; private set; }
        #endregion Properties

        private void Awake()
        {
            StartCoroutine(GetServerTime());
        }

        private void Update()
        {
            if (ServerTimeInited)
                Tick();
        }

        #region Methods

        private void Tick()
        {
            UrmobiServerDT = UrmobiServerDT.AddSeconds(Time.unscaledDeltaTime);
            LocalUrmobiServerDT = LocalUrmobiServerDT.AddSeconds(Time.unscaledDeltaTime);
            tickEvent?.Invoke();
        }

        public static IEnumerator GetServerTime()
        {
            string url = "https://api.backmobi.pro/session/timestamp";
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            if (!www.isNetworkError)
            {
                string time = www.downloadHandler.text;
                time = time.Remove(0, ('"'+"{timestamp"+'"'+':').Length);
                time = time.Remove(time.Length-1);
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(double.Parse(time, System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
                UrmobiServerDT = dtDateTime;
                LocalUrmobiServerDT = UrmobiServerDT.ToLocalTime();
                EventManager.Notify(null, new GameEventArgs(Events.DateTime.URMOBI_DATE_GOTTEN));
                ServerTimeInited = true;
                Debug.LogError($"Server Time: {dtDateTime}");
                Debug.LogError($"Local Time: {dtDateTime.ToLocalTime()}");
            }
            else
                Debug.LogError($"ERROR {www.error}");
        }

        public static void GetNISTDate()
        {
            try
            {
                System.Threading.Tasks.Task ss = System.Threading.Tasks.Task.Run(
                    () =>
                    {
                        System.Random ran = new System.Random(DateTime.Now.Millisecond);
                        string serverResponse = string.Empty;

                        // Represents the list of NIST servers
                        string[] servers = new string[] {
                         "129.6.15.28",
                         "129.6.15.28",
                         "132.163.96.1",
                         "132.163.96.2",
                         "64.113.32.5",
                         "64.147.116.229",
                         "64.125.78.85",
                         "128.138.140.44"
                              };

                        // Try each server in random order to avoid blocked requests due to too frequent request
                        for (int i = 0; i < 5; i++)
                        {
                            try
                            {
                                // Open a StreamReader to a random time server
                                StreamReader reader = new StreamReader(new System.Net.Sockets.TcpClient(servers[ran.Next(0, servers.Length)], 13).GetStream());
                                serverResponse = reader.ReadToEnd();
                                reader.Close();

                                // Check to see that the signiture is there
                                if (serverResponse.Length > 47 && serverResponse.Substring(38, 9).Equals("UTC(NIST)"))
                                {
                                    // Parse the date
                                    int jd = int.Parse(serverResponse.Substring(1, 5));
                                    int yr = int.Parse(serverResponse.Substring(7, 2));
                                    int mo = int.Parse(serverResponse.Substring(10, 2));
                                    int dy = int.Parse(serverResponse.Substring(13, 2));
                                    int hr = int.Parse(serverResponse.Substring(16, 2));
                                    int mm = int.Parse(serverResponse.Substring(19, 2));
                                    int sc = int.Parse(serverResponse.Substring(22, 2));

                                    if (jd > 51544)
                                        yr += 2000;
                                    else
                                        yr += 1999;

                                    NIST_Date = new DateTime(yr, mo, dy, hr, mm, sc);
                                    Local_NIST_Date = NIST_Date.ToLocalTime();

                                    EventManager.Notify(null, new GameEventArgs(Events.DateTime.NIST_DATE_GOTTEN));
                                    break;
                                }

                            }
                            catch (Exception ex)
                            {
                                Debug.Log($"Exception [{ex}]. Iteration [{i}] failed.");
                            }
                        }
                    });               
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public void DoSlowMotion(float duration, float slowFactor, TimeScale.Type type)
        {
            SlowMotionFactor = slowFactor;
            IsSlowMotion = true;
            switch(type)
            {
                case TimeScale.Type.Global:
                    TimeScale.GlobalTimeScale = 1 / slowFactor;
                    StartCoroutine(SlowMotion(duration, type));
                    break;
                case TimeScale.Type.Player:
                    TimeScale.PlayerTimeScale = 1 / slowFactor;
                    StartCoroutine(SlowMotion(duration, type));
                    break;
                case TimeScale.Type.Enemy:
                    TimeScale.EnemyTimeScale = 1 / slowFactor;
                    StartCoroutine(SlowMotion(duration, type));
                    break;
            }
        }


        private IEnumerator GetNISTDateCoroutine()
        {
            GetNISTDate();
            yield return new WaitForSecondsRealtime(60 * 5);
            yield return StartCoroutine(GetNISTDateCoroutine());
        }

        private static IEnumerator SlowMotion(float duration, TimeScale.Type type)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                yield return elapsedTime += Time.deltaTime;
            }
            switch (type)
            {
                case TimeScale.Type.Global:
                    TimeScale.GlobalTimeScale = TimeScale.DEFAULT;
                    break;
                case TimeScale.Type.Player:
                    TimeScale.PlayerTimeScale = TimeScale.DEFAULT;
                    break;
                case TimeScale.Type.Enemy:
                    TimeScale.EnemyTimeScale = TimeScale.DEFAULT;
                    break;
            }
            IsSlowMotion = false;
        }
        #endregion Methods
    }

    public static class TimeScale
    {
        public static event System.Action OnTimeScaleChanged;
        public const float DEFAULT = 1f;

        #region Fields
        private static float global = 1f;
        private static float player = 1f;
        private static float enemy = 1f;
        #endregion

        #region Properties
        public static float GlobalTimeScale { get => global; set { global = value; OnTimeScaleChanged?.Invoke(); } }
        public static float PlayerTimeScale { get => player; set { player = value; OnTimeScaleChanged?.Invoke(); } }
        public static float EnemyTimeScale { get => enemy; set { enemy = value; OnTimeScaleChanged?.Invoke(); } }

        public static float GlobalDeltaTime { get => Time.deltaTime * global; }
        public static float PlayerDeltaTime { get => Time.deltaTime * player; }
        public static float EnemyDeltaTime { get => Time.deltaTime * enemy; }
        #endregion Properties

        public static float GetDeltaTime(Type type)
        {
            switch (type)
            {
                case Type.Enemy:
                    return EnemyDeltaTime;
                case Type.Player:
                    return PlayerDeltaTime;
                case Type.Global:
                    return GlobalDeltaTime;
                default: return GlobalDeltaTime;
            }
        }

        public static float GetTimeScale(Type type)
        {
            switch(type)
            {
                case Type.Enemy:
                    return enemy;
                case Type.Player:
                    return player;
                case Type.Global:
                    return global;
                default: return DEFAULT;
            }
        }

        public enum Type
        {
            Global,
            Player,
            Enemy
        }
    }
}