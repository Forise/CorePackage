using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Core
{
    public class TextLocalization : MonoBehaviour
    {
        public string _key;
        public bool UseDefaultLanguage;
        public List<string> ParamsForReplace = new List<string>();
        
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                Localize();
            }
        }

        public string Search;

        private Text UiText;
        private TextMesh MeshText;
        private TMPro.TextMeshProUGUI MeshTextProUGUI;
        private TMPro.TextMeshPro MeshTextPro;
        //save current color of elements (when we have error - text set a red color, and then return to normal color)
        //private Color _uiTextColor;
        //private Color _meshTextColor;

        #region Localize Logic
        private void Start()
        {
                Initialize();
        }

        public void Initialize()
        {
            LocalizationManager.OnChangeLocalization += OnChangeLocalization;

            UiText = gameObject.GetComponent<Text>();
            MeshText = gameObject.GetComponent<TextMesh>();
            MeshTextPro = gameObject.GetComponent<TMPro.TextMeshPro>();
            MeshTextProUGUI = gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            OnChangeLocalization();
        }

        private void OnChangeLocalization()
        {
            Localize();
        }

        private void Localize()
        {
            var localizedText = LocalizationManager.GetTextByKey(_key);

            SetTextValue(ReplaceParams(localizedText));
            //Debug.Log("Localized text: " + MeshTextProUGUI.text);
        }

        private string ReplaceParams(string textToLocalize)
        {
            if (ParamsForReplace.Count == 0)
                return textToLocalize;

            for (int i = 0; i < ParamsForReplace.Count; i++)
            {
                if (textToLocalize.Contains("{" + i + "}"))
                    textToLocalize = textToLocalize.Replace("{" + i + "}", ParamsForReplace[i]);
            }
            return textToLocalize;
        }

        private void SetTextValue(string text)
        {
            text = ParceText(text);

            if (UiText != null)
            {
                UiText.text = text;
            }

            if (MeshText != null)
            {
                MeshText.text = text;
            }

            if (MeshTextPro != null)
            {
                MeshTextPro.text = text;
            }

            if (MeshTextProUGUI != null)
            {
                MeshTextProUGUI.text = text;
            }

            // error check
            if (text == "[EMPTY]" || text == string.Format("[ERROR KEY {0}]", _key))
            {

                if (!UseDefaultLanguage)
                {
                    if (UiText != null)
                    {
                        UiText.color = Color.red;
                    }
                    if (MeshText != null)
                    {
                        MeshText.color = Color.red;
                    }

                    if (MeshTextPro != null)
                    {
                        MeshTextPro.color = Color.red;
                    }

                    if (MeshTextProUGUI != null)
                    {
                        MeshTextProUGUI.color = Color.red;
                    }
                } else
                {
                    text = ParceText(LocalizationManager.Instance.GetDefaultLanguageTextByKey(_key));

                    if (UiText != null)
                    {
                        UiText.text = text;
                    }

                    if (MeshText != null)
                    {
                        MeshText.text = text;
                    }
                    if (MeshTextPro != null)
                    {
                        MeshTextPro.text = text;
                    }
                    if (MeshTextProUGUI != null)
                    {
                        MeshTextProUGUI.text = text;
                    }
                }
            }
        }

        private string ParceText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text.Replace("\\n", Environment.NewLine);
        }

        private void OnDestroy()
        {
            LocalizationManager.OnChangeLocalization -= OnChangeLocalization;
        }
        #endregion Localize Logic

        #region Helpers
        public bool IsHasOutputHelper()
        {
            UiText = gameObject.GetComponent<Text>();
            MeshText = gameObject.GetComponent<TextMesh>();
            MeshTextProUGUI = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
            return UiText != null || MeshText != null || MeshTextProUGUI != null;
        }
        #endregion Helpers
    }
}