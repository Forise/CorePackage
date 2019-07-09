//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections.Generic;
using Core.EventSystem;

namespace Core.Audio
{
    public class AudioDispatcher : MonoBehaviour
    {    
        public List<AudioEvent> startAudioEvents = new List<AudioEvent>();
        public List<AudioEvent> stopAudioEvents = new List<AudioEvent>();

        public bool useEventManager = true;
        public bool useAudioEventManager = false;
        public bool useInputEventManager = false;
        public bool useWindowEventManager = false;
        public bool useGameplayEventManager = false;

        public enum DispatcherType
        {
            Sound, Music
        }

        private Dictionary<string, List<AudioClip>> sortedAudioEvents = new Dictionary<string, List<AudioClip>>();
        private Dictionary<string, TimedNumber> eventTimes = new Dictionary<string, TimedNumber>();
        [SerializeField]
        private DispatcherType dispatcherType;

        protected virtual void Start()
        {
            foreach (var audioEvent in startAudioEvents)
            {
                if (sortedAudioEvents.ContainsKey(audioEvent.Event))
                {
                    sortedAudioEvents[audioEvent.Event].Add(audioEvent.Clip);
                }
                else
                {
                    sortedAudioEvents.Add(audioEvent.Event, new List<AudioClip> { audioEvent.Clip });
                    eventTimes.Add(audioEvent.Event, new TimedNumber(0f, 0));

                    SubscribeStart(audioEvent.Event);
                }
            }

            foreach (var audioEvent in stopAudioEvents)
            {
                SubscribeStop(audioEvent.Event);
            }
        }

        protected virtual void SubscribeStart(string type)
        {
            if (useEventManager)
                EventManager.Subscribe(type, Handler_StartAudioEvent);
            if (useAudioEventManager)
                EventManager_Audio.Subscribe(type, Handler_StartAudioEvent);
            if (useInputEventManager)
                EventManager_Input.Subscribe(type, Handler_StartAudioEvent);
            if (useWindowEventManager)
                EventManager_Window.Subscribe(type, Handler_StartAudioEvent);
            if (useGameplayEventManager)
                EventManager_Gameplay.Subscribe(type, Handler_StartAudioEvent);
        }

        protected virtual void UnsubscribeStart(string type)
        {
            if (useEventManager)
                EventManager.Unsubscribe(type, Handler_StartAudioEvent);
            if (useAudioEventManager)
                EventManager_Audio.Unsubscribe(type, Handler_StartAudioEvent);
            if (useInputEventManager)
                EventManager_Input.Unsubscribe(type, Handler_StartAudioEvent);
            if (useWindowEventManager)
                EventManager_Window.Unsubscribe(type, Handler_StartAudioEvent);
            if (useGameplayEventManager)
                EventManager_Gameplay.Unsubscribe(type, Handler_StartAudioEvent);
        }

        protected virtual void SubscribeStop(string type)
        {
            if (useEventManager)
                EventManager.Subscribe(type, Handler_StopAudioEvent);
            if (useAudioEventManager)
                EventManager_Audio.Subscribe(type, Handler_StopAudioEvent);
            if (useInputEventManager)
                EventManager_Input.Subscribe(type, Handler_StopAudioEvent);
            if (useWindowEventManager)
                EventManager_Window.Subscribe(type, Handler_StopAudioEvent);
            if (useGameplayEventManager)
                EventManager_Gameplay.Subscribe(type, Handler_StopAudioEvent);
        }

        protected virtual void UnsubscribeStop(string type)
        {
            if (useEventManager)
                EventManager.Unsubscribe(type, Handler_StopAudioEvent);
            if (useAudioEventManager)
                EventManager_Audio.Unsubscribe(type, Handler_StopAudioEvent);
            if (useInputEventManager)
                EventManager_Input.Unsubscribe(type, Handler_StopAudioEvent);
            if (useWindowEventManager)
                EventManager_Window.Unsubscribe(type, Handler_StopAudioEvent);
            if (useGameplayEventManager)
                EventManager_Gameplay.Unsubscribe(type, Handler_StopAudioEvent);
        }

        protected virtual void OnDestroy()
        {
            foreach (var startEvent in startAudioEvents)
                UnsubscribeStart(startEvent.Event);
            foreach (var stopEvent in stopAudioEvents)
                UnsubscribeStop(stopEvent.Event);
        }

        #region Handlers
        protected virtual void Handler_StartAudioEvent(object sender, GameEventArgs e)
        {
            //Debug.Log($"sound event {e.type} notifyed");
            if (sortedAudioEvents[e.type].Count > 1)
            {
                int oldNumber = eventTimes[e.type].Number;
                do
                {
                    eventTimes[e.type].Number = Random.Range(0, sortedAudioEvents[e.type].Count);
                } while (oldNumber == eventTimes[e.type].Number);
            }

            switch(dispatcherType)
            {
                case DispatcherType.Sound:
                    AudioControl.Instance.PlaySound(sortedAudioEvents[e.type][eventTimes[e.type].Number], e.boolParam);
                    break;
                case DispatcherType.Music:
                    AudioControl.Instance.PlayMusic(sortedAudioEvents[e.type][eventTimes[e.type].Number], e.boolParam);
                    break;
            }
            
        }

        protected virtual void Handler_StopAudioEvent(object sender, GameEventArgs e)
        {
            foreach (var audioEvent in stopAudioEvents)
            {
                if (e.type == audioEvent.Event)
                {
                    switch(dispatcherType)
                    {
                        case DispatcherType.Sound:
                            AudioControl.Instance.StopSound(audioEvent.Clip.name);
                            break;
                        case DispatcherType.Music:
                            AudioControl.Instance.StopMusic();
                            break;
                    }
                    break;
                }
            }
        }
        #endregion
    }
}