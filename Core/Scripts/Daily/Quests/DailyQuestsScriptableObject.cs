using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Daily.Quests
{
    [CreateAssetMenu(fileName = "Daily Quests", menuName = "Core/Daily/Quests/Daily Quests")]
    public class DailyQuestsScriptableObject : ScriptableObject
    {
        public DailyQuest[] dailyQuests;
    }
}