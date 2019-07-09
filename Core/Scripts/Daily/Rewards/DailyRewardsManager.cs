using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Rewards;

namespace Core.Daily
{
    public class DailyRewardsManager : MonoSingleton<DailyRewardsManager>
    {
        public List<ConsumeableReward> rewards = new List<ConsumeableReward>();

        private void Awake()
        {
            Daily.Instance.Inited += OnDailyInited;
            foreach(var r in rewards)
            {
                r.State = ConsumeableRewardState.Inactive;
            }
        }

        public void Claim(byte id)
        {
            UserDataControl.Instance.UserData.RewardsData.lastClaimedDaily = id;
            rewards[id].Consume();
        }

        #region Handlers
        public void OnDailyInited()
        {
            for (int i = 0; i < Daily.Instance.CurrentDailyDay; i++)
            {
                rewards[i].State = ConsumeableRewardState.Consumed;
            }
            rewards[Daily.Instance.CurrentDailyDay].State = UserDataControl.Instance.UserData.RewardsData.lastClaimedDaily == Daily.Instance.CurrentDailyDay ? ConsumeableRewardState.Consumed : ConsumeableRewardState.Active;
            Debug.LogError("DAILY RAWARD MANAGER INITED");
        }
        #endregion Hanlders
    }
}