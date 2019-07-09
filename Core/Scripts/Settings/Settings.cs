//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [System.Serializable, SerializeField]
    public struct Settings
    {
        public bool isMusicOn;
        public bool isSoundsOn;
        public bool saveUserDataToCloud;
        public float currentMusicVolume;
        public float currentSoundsVolume;
        public SystemLanguage language;

        public Settings(bool setDefault)
        {
            isMusicOn = setDefault;
            isSoundsOn = setDefault;
            saveUserDataToCloud = setDefault;
            currentMusicVolume = 0f;
            currentSoundsVolume = 0f;
            language = SystemLanguage.English;
        }
    }
}