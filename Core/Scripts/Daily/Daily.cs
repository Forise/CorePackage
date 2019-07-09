using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;
#if CHEATS
using Core.Rewards;
#endif

namespace Core.Daily
{
    public class Daily : MonoSingleton<Daily>
    {
#region Fields
        public event System.Action Inited;
        [SerializeField]
        private byte daysCycle = 7;
#endregion Fields

#region Properties
        public bool DataInited { get; private set; }
        public System.DateTime NextDate { get; protected set; }
        public byte CurrentDailyDay { get; protected set; } = 0;
        public byte NextDailyDay { get; protected set; } = 1;
#endregion Properties

        private void Awake()
        {
            EventManager.Subscribe(Events.SaveData.NEW_DATA_APPLIED, OnNewDataApplied_Handler);
            EventManager.Subscribe(Events.DateTime.URMOBI_DATE_GOTTEN, OnServerDateGotten_Handler);
        }

#region Methods
        private void UpdateDays(bool init = false)
        {
            var t = TimeManager.LocalUrmobiServerDT;
            if (t >= NextDate.AddDays(1d))
            {
                CurrentDailyDay = 0;
                NextDailyDay = 1;
                //SetupCurrentRewards();
                NextDate = new System.DateTime(t.Year, t.Month, t.Day, 0, 0, 0).AddDays(1);
                UserDataControl.Instance.UserData.RewardsData.nextDate = NextDate.ToString();
            }
            else if (t >= NextDate && t < NextDate.AddDays(1))
            {
                CurrentDailyDay = NextDailyDay;
                NextDailyDay = NextDailyDay < daysCycle - 1 ? (byte)(CurrentDailyDay + 1) : (byte)0;
                //SetupCurrentRewards();
                NextDate = new System.DateTime(t.Year, t.Month, t.Day, 0, 0, 0).AddDays(1);
                UserDataControl.Instance.UserData.RewardsData.currentDailyDay = CurrentDailyDay;
                UserDataControl.Instance.UserData.RewardsData.nextDate = NextDate.ToString();
                EventManager.Notify(this, new GameEventArgs(Events.Daily.NEX_DAY));
            }
            else if(t >= NextDate)
            {
                //SetupCurrentRewards();
            }

            if (init)
                Inited?.Invoke();
        }

        #region CHEATS
#if CHEATS
        public void AddOneDay()
        {
            CurrentDailyDay = NextDailyDay;
            NextDailyDay = NextDailyDay < daysCycle - 1 ? (byte)(CurrentDailyDay + 1) : (byte)0;
            UserDataControl.Instance.UserData.RewardsData.currentDailyDay = CurrentDailyDay;
        }

        public void AddTwoDays()
        {
            foreach (var r in DailyRewardsManager.Instance.rewards)
            {
                r.State = ConsumeableRewardState.Inactive;
            }
            CurrentDailyDay = 0;
            NextDailyDay = 1;
            UserDataControl.Instance.UserData.RewardsData.currentDailyDay = CurrentDailyDay;
        }
#endif
        #endregion CHEATS
#endregion Methods

#region Handlers
        private void OnTimeTick()
        {
            UpdateDays();
        }

        private void OnServerDateGotten_Handler(object sender, GameEventArgs e)
        {
#if UNITY_EDITOR
            TimerController.Instance.CreateNewMonoTimer(3, () =>
            {                
                if (!UserDataControl.Instance.UserData)
                {
                    UserDataControl.Instance.UserData = new UserDataPresenter();
                    UserDataControl.Instance.UserData = new UserDataPresenter();
                }

                CurrentDailyDay = UserDataControl.Instance.UserData.RewardsData.currentDailyDay;
                NextDailyDay = CurrentDailyDay < daysCycle - 1 ? (byte)(CurrentDailyDay + 1) : (byte)0;
                //CurrentWheelDay = UserDataControl.Instance.UserData.RewardsData.currentWheelDay;
                //CurrentScratchDay = UserDataControl.Instance.UserData.RewardsData.currentScratchDay;
                //CurrentSlotDay = UserDataControl.Instance.UserData.RewardsData.currentSlotDay;
                //CurrentRewardMechanic = UserDataControl.Instance.UserData.RewardsData.currentRewardMechanic;

                try
                {
                    NextDate = System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextDate);
                    UserDataControl.Instance.UserData.RewardsData.nextDate = NextDate.ToString();
                }
                catch(System.Exception ex)
                {
                    Debug.LogWarning(ex, this);
                    var t = TimeManager.LocalUrmobiServerDT;
                    Debug.Log($"Next day on init: {new System.DateTime(t.Year, t.Month, t.Day, 0, 0, 0).AddDays(1).ToString()}");
                    UserDataControl.Instance.UserData.RewardsData.nextDate = new System.DateTime(t.Year, t.Month, t.Day, 0, 0, 0).AddDays(1).ToString();
                    Debug.Log($"Used data next day: {UserDataControl.Instance.UserData.RewardsData.nextDate}");
                    NextDate = System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextDate);
                }
                UpdateDays(true);
                TimeManager.TickEvent -= OnTimeTick;
                TimeManager.TickEvent += OnTimeTick;
                DataInited = true;
            });
#endif
        }

        private void OnNewDataApplied_Handler(object sender, GameEventArgs e)
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            Debug.Log("OnNewDataApplied_Handler");
            
            if (!UserDataControl.Instance.UserData)
            {
                UserDataControl.Instance.UserData = new UserDataPresenter();
                UserDataControl.Instance.UserData = new UserDataPresenter();
            }
            CurrentDailyDay = UserDataControl.Instance.UserData.RewardsData.currentDailyDay;
            NextDailyDay = CurrentDailyDay < daysCycle - 1 ? (byte)(CurrentDailyDay + 1) : (byte)0;
            //CurrentWheelDay = UserDataControl.Instance.UserData.RewardsData.currentWheelDay;
            //CurrentScratchDay = UserDataControl.Instance.UserData.RewardsData.currentScratchDay;
            //CurrentSlotDay = UserDataControl.Instance.UserData.RewardsData.currentSlotDay;
            //CurrentRewardMechanic = UserDataControl.Instance.UserData.RewardsData.currentRewardMechanic;

            if (UserDataControl.Instance.UserData.RewardsData.nextDate != null && System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextDate) != System.DateTime.MinValue)
            {
                Debug.Log("NEXT DATE DADA IS EMPTY!");
                NextDate = System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextDate);
                UserDataControl.Instance.UserData.RewardsData.nextDate = NextDate.ToString();
            }
            else
            {
                var t = TimeManager.UrmobiServerDT;
                Debug.Log($"Next day on init: {new System.DateTime(t.Year, t.Month, t.Day, 0, 0, 0).AddDays(1).ToString()}");
                UserDataControl.Instance.UserData.RewardsData.nextDate = new System.DateTime(t.Year, t.Month, t.Day, 0, 0, 0).AddDays(1).ToString();
                Debug.Log($"Used data next day: {UserDataControl.Instance.UserData.RewardsData.nextDate}");
                NextDate = System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextDate);
            }
            UpdateDays(true);
            TimeManager.TickEvent -= OnTimeTick;
            TimeManager.TickEvent += OnTimeTick;
            DataInited = true;
#endif
        }
#endregion Handlers
    }
}