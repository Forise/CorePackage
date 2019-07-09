using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;

namespace Core.Rewards
{
    public class CycleRewardsManager : MonoSingleton<CycleRewardsManager>
    {
        public UIWindow wheelWindow;
        public UIWindow slotWindow;
        public UIWindow scratchWindow;
        public uint daysCycle;
        public Button openRewardWindow;
        private RewardMechanic currentRewardMechanic = 0;

        #region Properties
        public byte CurrentWheelDay { get; protected set; } = 0;
        public byte NextWheelDay { get; protected set; } = 1;
        public byte CurrentScratchDay { get; protected set; } = 0;
        public byte NextScratchDay { get; protected set; } = 1;
        public byte CurrentSlotDay { get; protected set; } = 0;
        public byte NextSlotDay { get; protected set; } = 1;
        public byte CurrentRewardMechanic { get => (byte)currentRewardMechanic; protected set => currentRewardMechanic = (RewardMechanic)value; }
        #endregion Properties

        private void Awake()
        {
            CurrentRewardMechanic = 0;
            EventManager.Subscribe(Events.SaveData.NEW_DATA_APPLIED, OnNewDataApplied_Handler);
            EventManager.Subscribe(Events.DateTime.URMOBI_DATE_GOTTEN, OnServerDateGotten_Handler);
            openRewardWindow.onClick.AddListener(OpenRewardWindow);
        }

        #region Methods
        public void OpenRewardWindow()
        {
            switch(currentRewardMechanic)
            {
                case RewardMechanic.Wheel:
                    UIControl.Instance.OpenWindow(wheelWindow);
                    break;
                case RewardMechanic.Slot:
                    UIControl.Instance.OpenWindow(slotWindow);
                    break;
                case RewardMechanic.Scratch:
                    UIControl.Instance.OpenWindow(scratchWindow);
                    break;
            }
        }

        private void SetupCurrentRewards()
        {
            switch (currentRewardMechanic)
            {
                case RewardMechanic.Wheel:
                    if (NextWheelDay < daysCycle - 1)
                    {
                        CurrentWheelDay = NextWheelDay;
                        NextWheelDay = (byte)(CurrentWheelDay + 1);
                    }
                    else
                    {
                        CurrentWheelDay = 0;
                        NextWheelDay = 1;
                        CurrentRewardMechanic = (byte)RewardMechanic.Slot;
                    }
                    UserDataControl.Instance.UserData.RewardsData.currentWheelDay = CurrentWheelDay;
                    break;
                case RewardMechanic.Slot:
                    if (NextSlotDay < daysCycle - 1)
                    {
                        CurrentSlotDay = NextSlotDay;
                        NextSlotDay = (byte)(CurrentSlotDay + 1);
                    }
                    else
                    {
                        CurrentSlotDay = 0;
                        NextSlotDay = 1;
                        CurrentRewardMechanic = (byte)RewardMechanic.Scratch;
                    }
                    UserDataControl.Instance.UserData.RewardsData.currentSlotDay = CurrentSlotDay;
                    break;
                case RewardMechanic.Scratch:
                    if (NextScratchDay < daysCycle - 1)
                    {
                        CurrentScratchDay = NextScratchDay;
                        NextScratchDay = (byte)(CurrentScratchDay + 1);
                    }
                    else
                    {
                        CurrentScratchDay = 0;
                        NextScratchDay = 1;
                        CurrentRewardMechanic = (byte)RewardMechanic.Wheel;
                    }
                    UserDataControl.Instance.UserData.RewardsData.currentScratchDay = CurrentScratchDay;
                    break;
            }

            UserDataControl.Instance.UserData.RewardsData.currentRewardMechanic = CurrentRewardMechanic;
        }

        private void UpdateDays()
        {
            var t = TimeManager.LocalUrmobiServerDT;
            if (t >= Daily.Daily.Instance.NextDate)
            {
                SetupCurrentRewards();
            }
        }

#if CHEATS
        public void AddWheelDay()
        {
            CurrentWheelDay = NextWheelDay;
            NextWheelDay = NextWheelDay < daysCycle - 1 ? (byte)(CurrentWheelDay + 1) : (byte)0;
            UserDataControl.Instance.UserData.RewardsData.currentWheelDay = CurrentWheelDay;
        }
        public void AddScratchDay()
        {
            CurrentScratchDay = NextScratchDay;
            NextScratchDay = NextScratchDay < daysCycle - 1 ? (byte)(CurrentScratchDay + 1) : (byte)0;
            UserDataControl.Instance.UserData.RewardsData.currentScratchDay = CurrentScratchDay;
        }
        public void AddSlotDay()
        {
            CurrentSlotDay = NextSlotDay;
            NextSlotDay = NextSlotDay < daysCycle - 1 ? (byte)(CurrentSlotDay + 1) : (byte)0;
            UserDataControl.Instance.UserData.RewardsData.currentSlotDay = CurrentSlotDay;
        }
#endif
        #endregion Methods

        #region Handlers
        private void OnNewDataApplied_Handler(object sender, GameEventArgs e)
        {
            CurrentWheelDay = UserDataControl.Instance.UserData.RewardsData.currentWheelDay;
            NextWheelDay = CurrentWheelDay < daysCycle - 1 ? (byte)(CurrentWheelDay + 1) : (byte)0;

            CurrentScratchDay = UserDataControl.Instance.UserData.RewardsData.currentScratchDay;
            NextScratchDay = CurrentScratchDay < daysCycle - 1 ? (byte)(CurrentScratchDay + 1) : (byte)0;

            CurrentSlotDay = UserDataControl.Instance.UserData.RewardsData.currentSlotDay;
            NextSlotDay = CurrentSlotDay < daysCycle - 1 ? (byte)(CurrentSlotDay + 1) : (byte)0;

            CurrentRewardMechanic = UserDataControl.Instance.UserData.RewardsData.currentRewardMechanic;
            TimeManager.TickEvent -= UpdateDays;
            TimeManager.TickEvent += UpdateDays;
        }
        private void OnServerDateGotten_Handler(object sender, GameEventArgs e)
        {
#if UNITY_EDITOR
            TimerController.Instance.CreateNewMonoTimer(3, () =>
            {
                CurrentWheelDay = UserDataControl.Instance.UserData.RewardsData.currentWheelDay;
                NextWheelDay = CurrentWheelDay < daysCycle - 1 ? (byte)(CurrentWheelDay + 1) : (byte)0;

                CurrentScratchDay = UserDataControl.Instance.UserData.RewardsData.currentScratchDay;
                NextScratchDay = CurrentScratchDay < daysCycle - 1 ? (byte)(CurrentScratchDay + 1) : (byte)0;

                CurrentSlotDay = UserDataControl.Instance.UserData.RewardsData.currentSlotDay;
                NextSlotDay = CurrentSlotDay < daysCycle - 1 ? (byte)(CurrentSlotDay + 1) : (byte)0;

                CurrentRewardMechanic = UserDataControl.Instance.UserData.RewardsData.currentRewardMechanic;
                TimeManager.TickEvent -= UpdateDays;
                TimeManager.TickEvent += UpdateDays;
            });
#endif
        }
#endregion Handlers
    }
}