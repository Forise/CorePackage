using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;

namespace Core.Rewards
{
    public class SurpriseReward : MonoSingleton<SurpriseReward>
    {
        #region Fields
        [SerializeField]
        private Reward reward;
        #endregion Fields

        #region Properties
        public System.DateTime CurrentDate { get; protected set; }
        public System.DateTime NextDate { get; protected set; }
        public bool AvailableToConsume { get; protected set; }
        public System.TimeSpan CooldownTime => (NextDate - CurrentDate).TotalMilliseconds > 0 ? NextDate - CurrentDate : new System.TimeSpan(0,0,0,0);
        #endregion Properties

        private void Awake()
        {
            EventManager.Subscribe(Events.SaveData.NEW_DATA_APPLIED, OnNewDataApplied_Handler);
            EventManager.Subscribe(Events.DateTime.URMOBI_DATE_GOTTEN, OnServerDateGotten_Handler);
        }

        #region Methods
        private void UpdateDate()
        {
            CurrentDate = TimeManager.LocalUrmobiServerDT;
            AvailableToConsume = CurrentDate >= NextDate;

        }

        public bool Consume()
        {
            if (AvailableToConsume)
            {
                reward.Consume();
                if (CurrentDate >= new System.DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 6, 0, 0) && CurrentDate < new System.DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 12, 0, 0))
                {
                    NextDate = CurrentDate.AddHours(2);
                }
                else if (CurrentDate >= new System.DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 12, 0, 0) && CurrentDate < new System.DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 20, 0, 0))
                {
                    NextDate = CurrentDate.AddHours(3);
                }
                else if (CurrentDate >= new System.DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 20, 0, 0) && CurrentDate < new System.DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day + 1, 6, 0, 0))
                {
                    NextDate = CurrentDate.AddHours(6);
                }
                UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate = NextDate.ToString();
                UserDataControl.Instance.SaveData();
                AvailableToConsume = false;
                Debug.Log($"Reward claimed {reward.reward}", this);
                UIControl.Instance.ShowInfoPopUp(new UIPopUp.PopupData($"You have gotten {reward.reward}!"));
                return true;
            }
            else
            {
                Debug.Log("Consume failed", this);
                return false;
            }
        }
        #endregion Methods

        #region Handlers
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

                CurrentDate = TimeManager.LocalUrmobiServerDT;

                try
                {
                    NextDate = System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate);
                    UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate = NextDate.ToString();
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning(ex, this);
                    var t = TimeManager.LocalUrmobiServerDT;
                    UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate = new System.DateTime().ToString();
                    NextDate = System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate);
                }
                UpdateDate();
                TimeManager.TickEvent -= OnTimeTick;
                TimeManager.TickEvent += OnTimeTick;
                EventManager.Unsubscribe(Events.SaveData.NEW_DATA_APPLIED, OnNewDataApplied_Handler);
                EventManager.Unsubscribe(Events.DateTime.URMOBI_DATE_GOTTEN, OnServerDateGotten_Handler);
            });
#endif
        }

        private void OnNewDataApplied_Handler(object sender, GameEventArgs e)
        {
#if !UNITY_EDITOR
            if (!UserDataControl.Instance.UserData)
            {
                UserDataControl.Instance.UserData = new UserDataPresenter();
                UserDataControl.Instance.UserData = new UserDataPresenter();
            }

            CurrentDate = TimeManager.LocalUrmobiServerDT;

            if (UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate != null && System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextDate) != System.DateTime.MinValue)
            {
                NextDate = System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate);
                UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate = NextDate.ToString();
            }
            else
            {
                var t = TimeManager.LocalUrmobiServerDT;
                UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate = new System.DateTime().ToString();
                NextDate = System.DateTime.Parse(UserDataControl.Instance.UserData.RewardsData.nextSurprizeDate);
            }
            UpdateDate();
            TimeManager.TickEvent -= OnTimeTick;
            TimeManager.TickEvent += OnTimeTick;
            EventManager.Unsubscribe(Events.SaveData.NEW_DATA_APPLIED, OnNewDataApplied_Handler);
            EventManager.Unsubscribe(Events.DateTime.URMOBI_DATE_GOTTEN, OnServerDateGotten_Handler);
#endif
        }
        private void OnTimeTick()
        {
            UpdateDate();
        }
        #endregion Handlers
    }
}