using Core.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Rewards
{
    public class SlotsController : ARewardMechanicManager<SlotsController>
    {
        #region Fields
        [SerializeField]
        private List<Slot> slots;
        #endregion Fields

        #region Properties
        override protected byte GetRewardByDay
        {
            get
            {
                if (CycleRewardsManager.Instance.CurrentSlotDay <= 3)
                    return (byte)Random.Range(0, 2);
                else if (CycleRewardsManager.Instance.CurrentSlotDay <= 5)
                    return (byte)Random.Range(2, 4);
                else if (CycleRewardsManager.Instance.CurrentSlotDay == 6)
                    return 4;
                return 0;
            }
        }
        #endregion Properties

        #region Methods
        public void SpinSlots()
        {
            if (UserDataControl.Instance.UserData.RewardsData.lastClaimedSlot != CycleRewardsManager.Instance.CurrentSlotDay)
            {
                EventManager.Notify(this, new GameEventArgs(Events.Rewards.WHEEL_STARTED));
                int winSumm = 0;
                for (int i = 0; i < slots.Count; i++)
                {
                    winSumm += slots[i].Spin(GetRewardByDay);
                }
                StartCoroutine(ShowCongratulations(winSumm));
                UserDataControl.Instance.UserData.RewardsData.lastClaimedSlot = CycleRewardsManager.Instance.CurrentSlotDay;
            }
        }

        IEnumerator ShowCongratulations(int winSumm)
        {
            foreach(var s in slots)
            {
                while (s.Spinning)
                    yield return null;
            }
            UIControl.Instance.ShowInfoPopUp(new UIPopUp.PopupData($"You won {winSumm}!"));
            EventManager.Notify(this, new GameEventArgs(Events.Rewards.WHEEL_STOPPED));
        }
        #endregion Methods
    }
}