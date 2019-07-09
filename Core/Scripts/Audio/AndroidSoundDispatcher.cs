//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections.Generic;
using Core.EventSystem;

namespace Core.Audio
{    
    public class AndroidSoundDispatcher : MonoSingleton<AndroidSoundDispatcher>
    {
        public List<AndroidSoundEvent> startAudioEvents = new List<AndroidSoundEvent>();
        public List<string> stopLoopedAudioEvents = new List<string>();

        private Dictionary<string, List<int>> sortedAudioEvents = new Dictionary<string, List<int>>();
        private Dictionary<string, TimedNumber> eventTimes = new Dictionary<string, TimedNumber>();
        private Dictionary<string, int> loopedSounds = new Dictionary<string, int>();

        protected virtual void Start()
        {
            AndroidNativeAudio.makePool();
            foreach (var audioEvent in startAudioEvents)
            {
                if (sortedAudioEvents.ContainsKey(audioEvent.Event))
                {
                    sortedAudioEvents[audioEvent.Event].Add(AndroidNativeAudio.load(audioEvent.Path));
                }
                else
                {
                    sortedAudioEvents.Add(audioEvent.Event, new List<int> { AndroidNativeAudio.load(audioEvent.Path) });
                    eventTimes.Add(audioEvent.Event, new TimedNumber(0f, 0));
                    EventManager.Subscribe(audioEvent.Event, Handler_StartAudioEvent);
                }
            }

            foreach (var stopAudioEvent in stopLoopedAudioEvents)
            {
                EventManager.Subscribe(stopAudioEvent, Handler_StopLoopedEvent);
            }
        }

        protected override void OnDestroy()
        {
            foreach (var audioEvent in startAudioEvents)
                EventManager.Unsubscribe(audioEvent.Event, Handler_StartAudioEvent);
            foreach (var Event in startAudioEvents)
            {
                foreach (var sound in sortedAudioEvents[Event.Event])
                {
                    AndroidNativeAudio.unload(sound);
                }
            }
            AndroidNativeAudio.releasePool();
            base.OnDestroy();
        }

        #region Handlers
        protected virtual void Handler_StartAudioEvent(object sender, GameEventArgs e)
        {
            if (sortedAudioEvents[e.type].Count > 1)
            {
                int oldNumber = eventTimes[e.type].Number;
                do
                {
                    eventTimes[e.type].Number = Random.Range(0, sortedAudioEvents[e.type].Count);
                } while (oldNumber == eventTimes[e.type].Number);

            }

            if (SettingsControl.Instance.settings.isSoundsOn)
            {
                int stream = AndroidNativeAudio.play(sortedAudioEvents[e.type][eventTimes[e.type].Number], 1f, -1f, 1, e.boolParam ? -1 : 0);
                if (e.boolParam)
                    loopedSounds.Add(e.type, stream);
            }

        }

        protected virtual void Handler_StopLoopedEvent(object sender, GameEventArgs e)
        {
            Dictionary<string, int> soundsToSave = new Dictionary<string, int>();

            foreach(var s in loopedSounds)
            {
                if(s.Key != e.type)
                {
                    AndroidNativeAudio.stop(s.Value);
                    soundsToSave.Add(s.Key, s.Value);
                }
            }
            loopedSounds = soundsToSave;            
        }
        #endregion
    }
}