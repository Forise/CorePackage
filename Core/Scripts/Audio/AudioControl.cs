//Developed by Pavel Kravtsov.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;

namespace Core.Audio
{
    public class AudioControl : MonoSingleton<AudioControl>
    {
        #region Fields
        protected static List<AudioSource> musicSources, soundsSources;

        public UnityEngine.Audio.AudioMixerGroup musicMixer, soundMixer;
        public AudioSource musicSource;
        #endregion

        private void Start()
        {
            StartCoroutine(Init());
            musicSources = new List<AudioSource>(transform.Find("Music").GetComponents<AudioSource>());
            soundsSources = new List<AudioSource>(transform.Find("Sounds").GetComponents<AudioSource>());
            SetVolumes();
            EventManager.Subscribe(Events.ApplicationEvents.SETTINGS_CHANGED, SettingsChanged_Handler);            
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        #region Methods
        private IEnumerator Init()
        {
            yield return new WaitForSeconds(0.15f);
            musicSource.enabled = true;
            yield return null;
        }

        public void PlaySound(AudioClip clip, bool loop = false)
        {
            if (clip == null /*|| CurrentUserData.SoundOff*/)
                return;

            AudioSource freeSource = GetFreeSource(soundsSources);
            if (freeSource != null)
            {
                freeSource.outputAudioMixerGroup = soundMixer;
                freeSource.loop = loop;
                freeSource.clip = clip;
                freeSource.time = 0;
                freeSource.Play();
                //Debug.Log($"Sound {freeSource.clip.name} played; is loop: {freeSource.loop}");
            }
        }

        public void PlayMusic(AudioClip clip, bool loop = false)
        {
            if (clip == null /*|| CurrentUserData.MusicOff*/)
                return;

            AudioSource freeSource = GetFreeSource(musicSources);
            if (freeSource != null)
            {
                freeSource.outputAudioMixerGroup = musicMixer;
                freeSource.loop = loop;
                freeSource.clip = clip;
                freeSource.time = 0;
                freeSource.Play();
                //Debug.Log(string.Format("Music {0} played", freeSource.clip.name));
            }
        }

        public void StopSound(string clipName)
        {
            foreach (AudioSource source in soundsSources)
            {
                if (source != null && source.clip != null && source.clip.name == clipName)
                {
                    source.loop = false;
                    source.clip = null;
                    source.Stop();

                    break;
                }
            }
        }

        public void StopMusic()
        {
            if (musicSources != null)
            {
                foreach (AudioSource source in musicSources)
                {
                    if (source)
                    {
                        source.clip = null;
                        source.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Returns free source if exist, else return new audio source.
        /// </summary>
        /// <param name="sources">sources to get from</param>
        /// <returns></returns>
        public AudioSource GetFreeSource(List<AudioSource> sources)
        {
            if (sources == null)
                return null;

            for (int i = 0; i < sources.Count; i++)
            {
                if (!sources[i].isPlaying && !sources[i].mute)
                {
                    return sources[i];
                }
            }

            AudioSource newSource = Utilities.CopyComponent<AudioSource>(soundsSources[0], soundsSources[0].gameObject);
            newSource.Stop();
            newSource.clip = null;
            newSource.loop = false;
            soundsSources.Add(newSource);

            return newSource;
        }

        /// <summary>
        /// Setup misic and sound volumes from settings.
        /// </summary>
        private void SetVolumes()
        {
            var settings = SettingsControl.Instance.settings;
            musicMixer.audioMixer.SetFloat("MusicVolume", settings.isMusicOn ? settings.currentMusicVolume : -80f);
            soundMixer.audioMixer.SetFloat("SoundsVolume", settings.isSoundsOn ? settings.currentSoundsVolume : -80f);
            //floats have to be defined in mixer!!!
        }

        /// <summary>
        /// Setup sound and music volume.
        /// </summary>
        /// <param name="volume">
        /// Min value = -80. Max value = 0.
        /// </param>
        public void SetVolumes(float volume)
        {
            musicMixer.audioMixer.SetFloat("MusicVolume", volume);
            soundMixer.audioMixer.SetFloat("SoundsVolume", volume);
        }

        /// <summary>
        /// Mute sound and music (setup volume to MIN).
        /// </summary>
        public void Mute()
        {
            musicMixer.audioMixer.SetFloat("MusicVolume", -80f);
            soundMixer.audioMixer.SetFloat("SoundsVolume", -80f);
        }

        /// <summary>
        /// Unmute sound and music (setup volume to settings value).
        /// </summary>
        public void Unmute()
        {
            SetVolumes();
        }
        #endregion

        #region Handlers
        private void SettingsChanged_Handler(object sender, GameEventArgs e)
        {
            SetVolumes();
        }
        #endregion
    }
}