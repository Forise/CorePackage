using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;

namespace Core.Daily.Quests
{
    public class DailyQuestManager : MonoSingleton<DailyQuestManager>
    {
        #region Fields
        [SerializeField]
        private DailyQuestsScriptableObject dailyQuests;
        private DailyQuest currentQuest;
        private bool isComplete = false;
        #endregion Fields

        #region Properties
        public bool IsComplete => isComplete;
        #endregion Properties

        private void Start()
        {
            Daily.Instance.Inited += OnDailyInited_Handler;
        }

        #region Handlers
        private void OnDailyInited_Handler()
        {
            var t = TimeManager.LocalUrmobiServerDT;
            if(t >= Daily.Instance.NextDate)
            {
                isComplete = false;
                currentQuest = dailyQuests.dailyQuests[Random.Range(0, dailyQuests.dailyQuests.Length - 1)];
                EventManager.Subscribe(currentQuest.Event, Event_Handler);
            }
        }

        private void Event_Handler(object sender, GameEventArgs e)
        {
            currentQuest.count -= e.intParam.Value;
            if(currentQuest.count <= 0)
            {
                currentQuest.count = 0;
                isComplete = true;
            }
        }
        #endregion Handlers
    }
}