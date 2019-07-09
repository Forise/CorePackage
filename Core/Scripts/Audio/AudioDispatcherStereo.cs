//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections.Generic;
using Core.EventSystem;

namespace Core.Audio
{
    public class AudioDispatcherStereo : MonoBehaviour
    {
        public List<StereoAudioEvent> startEvents = new List<StereoAudioEvent>();
        public List<StereoAudioEvent> stopEvents = new List<StereoAudioEvent>();

        private Dictionary<string, List<AudioSource>> sortedAudioEvents = new Dictionary<string, List<AudioSource>>();
        private Dictionary<string, TimedNumber> eventTimes = new Dictionary<string, TimedNumber>();

        private void Start()
        {
            foreach (var audioEvent in startEvents)
            {
                if (sortedAudioEvents.ContainsKey(audioEvent.Event))
                {
                    sortedAudioEvents[audioEvent.Event].Add(audioEvent.Sourse);
                }
                else
                {
                    sortedAudioEvents.Add(audioEvent.Event, new List<AudioSource> { audioEvent.Sourse });
                    eventTimes.Add(audioEvent.Event, new TimedNumber(0f, 0));
                    SubscribeStart(audioEvent.Event);
                }
            }

            foreach (var audioEvent in stopEvents)
            {
                SubscribeStop(audioEvent.Event);
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var startEvent in startEvents)
                UnsubscribeStart(startEvent.Event);
            foreach (var stopEvent in stopEvents)
                UnsubscribeStop(stopEvent.Event);
        }

        protected virtual void SubscribeStart(string type)
        {
            EventManager.Subscribe(type, Handler_StartAudioEvent);
        }
        protected virtual void SubscribeStop(string type)
        {
            EventManager.Subscribe(type, Handler_StopAudioEvent);
        }

        private void UnsubscribeStop(string type)
        {
            EventManager.Unsubscribe(type, Handler_StopAudioEvent);
        }

        private void UnsubscribeStart(string type)
        {
            EventManager.Unsubscribe(type, Handler_StartAudioEvent);
        }

        #region Handlers
        private void Handler_StartAudioEvent(object sender, GameEventArgs e)
        {
            if (sortedAudioEvents[e.type].Count > 1)
            {
                int oldNumber = eventTimes[e.type].Number;
                do
                {
                    eventTimes[e.type].Number = Random.Range(0, sortedAudioEvents[e.type].Count);
                } while (oldNumber == eventTimes[e.type].Number);
            }

            foreach (var i in sortedAudioEvents[e.type])
            {
                i.Play();
            }
        }

        private void Handler_StopAudioEvent(object sender, GameEventArgs e)
        {
            foreach (var i in stopEvents)
            {
                if (i.Event == e.type)
                    i.Sourse.Stop();
            }
        }
        #endregion Handlers
    }

    [System.Serializable]
    public class StereoAudioEvent
    {
        public string Event;
        public AudioSource Sourse;
    }
}