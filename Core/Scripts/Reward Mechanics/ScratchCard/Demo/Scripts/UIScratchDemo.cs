using UnityEngine;
using UnityEngine.UI;

namespace Core.Rewards.Scratch
{
    public class UIScratchDemo : MonoBehaviour
    {
        #region Fields
        public ScratchCardManager CardManager;
        public EraseProgress EraseProgress;
        public Texture[] Brushes;
        public Toggle[] BrushToggles;
        public Toggle ProgressToggle;
        public TMPro.TextMeshProUGUI ProgressText;
        #endregion Fields

        protected void Start()
        {
            Application.targetFrameRate = 60;
            ProgressToggle.isOn = PlayerPrefs.GetInt("Toggle", 0) == 0;
            EraseProgress.OnProgress += OnEraseProgress;

            for (var i = 0; i < BrushToggles.Length; i++)
            {
                BrushToggles[i].onValueChanged.AddListener(OnChange);
            }
            BrushToggles[PlayerPrefs.GetInt("Brush")].isOn = true;
        }

        #region Methods
        public void OnChange(bool val)
        {
            for (var i = 0; i < BrushToggles.Length; i++)
            {
                if (BrushToggles[i].isOn)
                {
                    CardManager.SetEraseTexture(Brushes[i]);
                    PlayerPrefs.SetInt("Brush", i);
                    break;
                }
            }
        }

        public void OnCheck(bool check)
        {
            EraseProgress.gameObject.SetActive(ProgressToggle.isOn);
            PlayerPrefs.SetInt("Toggle", ProgressToggle.isOn ? 0 : 1);
        }

        public void OnEraseProgress(float progress)
        {
            ProgressText.text = "Progress: " + Mathf.Round(progress * 100f).ToString() + " %";
        }
        #endregion Methods
    }
}