using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Daily.Quests
{
    [System.Serializable]
    public struct DailyQuest
    {
        public string id;
        public int count;
        public Rewards.Reward reward;
        public string Event;
    }
}