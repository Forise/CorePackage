using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class LocalizationManager : MonoSingleton<LocalizationManager>
    {
        #region Fields
        public static string LocalizationPath = "Localization/";
        public static Action OnChangeLocalization;

        private const string DefaultLocalizationName = "English";
        private static string _localization = DefaultLocalizationName;
        private static Dictionary<string, string> localizationLibrary;
        #endregion Fields

        #region Properties
        public static string LocalizationFilePath
        {
            get { return LocalizationPath + _localization; }
        }
        public static string Localization
        {
            get { return _localization; }
            set
            {
                if (_localization == value) return;

                _localization = value;
                localizationLibrary = LoadLocalizeFileHelper();
                SetLocalization(value);

                OnChangeLocalization.SafeInvoke();
                //TODO: Notify language changed
            }
        }
        #endregion Properties

        private void Awake()
        {
            Initialize();
        }

        #region Localize Logic
        private void Initialize()
        {
            Localization = GetLocalization();
            localizationLibrary = LoadLocalizeFileHelper();
        }

        private static Dictionary<string, string> ParseLocalizeFile(string[,] grid)
        {
            var result = new Dictionary<string, string>(grid.GetUpperBound(0) + 1);

            for (int ln = 1; ln <= grid.GetUpperBound(1); ln++)
                for (int col = 1; col <= grid.GetUpperBound(0); col++)
                {
                    if (string.IsNullOrEmpty(grid[0, ln])
                        || string.IsNullOrEmpty(grid[col, ln])) continue;

                    if (!result.ContainsKey(grid[0, ln]))
                        result.Add(grid[0, ln], grid[col, ln]);
                    else
                    {
                        Debug.LogError(string.Format("Key {0} already exist", grid[0, ln]));
                    }
                }
            return result;
        }

        //get text
        public static string GetTextByKey(string key)
        {
            return GetTextByKeyWithLocalize(key, _localization, false);
        }

        public string GetDefaultLanguageTextByKey(string key)
        {
            return GetTextByKeyWithLocalize(key, DefaultLocalizationName, true);
        }

        private static string GetTextByKeyWithLocalize(string key, string localize, bool needDefaultValue)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(localize)) return "[EMPTY]";

            string keyValue;

            Dictionary<string, string> dictionary;

            if (!Application.isPlaying)
                localizationLibrary = LoadLocalizeFileHelper();

            dictionary = needDefaultValue ? LoadDefaultLocalizeFileHelper(DefaultLocalizationName) : localizationLibrary;

            if (dictionary != null && dictionary.TryGetValue(key, out keyValue))
            {
                return keyValue;
            }


            return string.Format("[ERROR KEY {0}]", key);
        }

        //get sprite
        public Sprite GetSprite(string nameFile)
        {
            return GetSpriteByName(nameFile, _localization);
        }

        private Sprite GetSpriteByName(string nameFile, string localize)
        {
            if (string.IsNullOrEmpty(nameFile) || string.IsNullOrEmpty(localize)) return null;

            string fullPathToFile = LocalizationPath + "Images/" + _localization + "/" + nameFile;
            var sprite = Resources.Load(fullPathToFile, typeof(Sprite)) as Sprite;
            if (sprite == null)
            {

                fullPathToFile = LocalizationPath + DefaultLocalizationName + "/Images/" + nameFile;
                sprite = Resources.Load(fullPathToFile, typeof(Sprite)) as Sprite;
                return sprite;
            }

            return sprite;
        }

        //get sound
        public AudioClip GetAudioClip(string nameFile)
        {

            if (string.IsNullOrEmpty(nameFile)) return null;
            string fullPathToFile = LocalizationPath + "Audio/" + _localization + "/" + nameFile;

            var audio = Resources.Load(fullPathToFile, typeof(AudioClip)) as AudioClip;
            if (audio == null)
            {
                fullPathToFile = LocalizationPath + DefaultLocalizationName + "/Audio/" + nameFile;
                audio = Resources.Load(fullPathToFile, typeof(AudioClip)) as AudioClip;
                return audio;
            }

            return audio;
        }

        // Integrate this to your PlayerPref Manager
        private string GetLocalization()
        {
            return PlayerPrefs.GetString("localization", Application.systemLanguage.ToString());
        }
        private static void SetLocalization(string localize)
        {
            PlayerPrefs.SetString("localization", localize);
        }
        #endregion Localize Logic

        #region Helpers
        public string[] GetLocalizations()
        {
            var result = new string[localizationLibrary.Count];
            var i = 0;
            foreach (var loc in localizationLibrary)
            {
                result[i] = loc.Key;
                i++;
            }
            return result;
        }

        public static Dictionary<string, string> LoadLocalizeFileHelper()
        {
            var languages = Resources.Load(LocalizationFilePath, typeof(TextAsset)) as TextAsset;
            if (languages == null)
            {
                if (Localization != DefaultLocalizationName)
                    LoadDefault();

                return null;
            }
            var resultGrid = CSVReader.SplitCsvGrid(languages.text);
            return ParseLocalizeFile(resultGrid);
        }

        public static Dictionary<string, string> LoadDefaultLocalizeFileHelper(string localeDefault)
        {
            var languages = Resources.Load(LocalizationPath + localeDefault, typeof(TextAsset)) as TextAsset;
            var resultGrid = CSVReader.SplitCsvGrid(languages.text);
            return ParseLocalizeFile(resultGrid);
        }

        private static void LoadDefault()
        {
            Localization = DefaultLocalizationName;
        }

        public static string[] GetLocalizationKeys()
        {
            var languages = Resources.Load(LocalizationFilePath, typeof(TextAsset)) as TextAsset;
            if (languages == null) return null;
            var resultGrid = CSVReader.SplitCsvGrid(languages.text);
            var localizeFile = ParseLocalizeFile(resultGrid);
            return localizeFile.Keys.ToArray();
        }

        #endregion Helpers

    }
}