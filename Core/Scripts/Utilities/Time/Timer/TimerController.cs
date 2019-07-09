//Developed by Pavel Kravtsov.
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core
{
    public class TimerController : MonoSingleton<TimerController>
    {
        public MonoTimer prefab;
        private List<MonoTimer> timers = new List<MonoTimer>();

        public MonoTimer CreateNewMonoTimer(float time, System.Action endAction = null, System.Action ticAction = null, bool destroyOnFinish = true)
        {
            //var timer = Instantiate(new MonoTimer(time, endAction, ticAction, destroyOnFinish), transform.position, Quaternion.identity, transform);
            var timer = Instantiate(prefab, transform.position, Quaternion.identity, transform); //HACK: reworked to prefab;
            timer.name = "Timer";
            timer.StartTimer(time, endAction, ticAction, destroyOnFinish);
            timers.Add(timer);
            timer.OnDestroyed += () => { timers.Remove(timer); };
            return timer;
        }

        public void ClearTimers()
        {
            foreach (var t in timers)
            {
                Destroy(t.gameObject);
            }
            timers = new List<MonoTimer>();
        }
    }
}