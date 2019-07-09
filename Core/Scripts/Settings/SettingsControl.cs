//Developed by Pavel Kravtsov.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class SettingsControl : MonoSingleton<SettingsControl>
    {
        [SerializeField]
        public Settings settings = new Settings(true);
        private string path;

        private void Awake()
        {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        path = Application.persistentDataPath + "/Settings.json";
#elif UNITY_EDITOR
            path = Application.dataPath + "/Settings.json";
#endif
            LoadData();
            SaveData();
            Debug.Log(GetSaveDataAsJson());
        }

        public void SetLanguage(SystemLanguage language)
        {
            settings.language = language;
            NotifySettingsChanged();
        }

        public void SetMusicVolume(float volume)
        {
            settings.currentMusicVolume = volume;
            NotifySettingsChanged();
        }

        public void SetSoundsVolume(float volume)
        {
            settings.currentSoundsVolume = volume;
            NotifySettingsChanged();
        }

        public void SetMusic(bool value)
        {
            settings.isMusicOn = value;
            NotifySettingsChanged();
        }

        public void SetSounds(bool value)
        {
            settings.isSoundsOn = value;
            NotifySettingsChanged();
        }

        private void NotifySettingsChanged()
        {
            EventSystem.EventManager.Notify(this, new EventSystem.GameEventArgs(EventSystem.Events.ApplicationEvents.SETTINGS_CHANGED));
            //UserDataControl.Instance.SaveLocal();
            SaveData();
        }

        public string GetSaveDataAsJson()
        {
            return JsonUtility.ToJson(settings);
        }

        public void SaveData()
        {
            var json = JsonUtility.ToJson(settings);
            System.IO.File.WriteAllText(path, json);
            //Debug.Log(json);
        }

        private void LoadData()
        {
            if (System.IO.File.Exists(path))
            {
                var json = System.IO.File.ReadAllText(path);
                //Debug.Log("JOSN: " + json);
                settings = JsonUtility.FromJson<Settings>(json);
            }
            else
            {
                settings.language = Application.systemLanguage;
                SaveData();
                LoadData();
            }
        }
    }
}