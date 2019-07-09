//Developed by Pavel Kravtsov.
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Core
{
    public class UIImageInfoPopUp : UIPopUp
    {
        public TextMeshProUGUI title;
        public TextLocalization titleLocalization;
        public Image image;
        public TextMeshProUGUI countText;
        [SerializeField]
        private MyButton okButton;
        private System.Action okAction;

        protected override void Start()
        {
            okButton.onClick.AddListener(OkButtonClick);
            base.Start();
        }

        private void OnDestroy()
        {
            okButton.onClick.RemoveListener(OkButtonClick);
        }

        protected override void ActiveStateUpdateHandler()
        {
            base.ActiveStateUpdateHandler();
            CheckMissClick(okAction);
        }

        #region Methods

        protected override void SetContent()
        {
            SetActions();
            image.sprite = popupData.sprite;
            base.SetContent();
        }

        private void SetActions()
        {
            okAction = popupData.confirmAction;
        }

        protected override void SetTexts()
        {
            base.SetTexts();
            titleLocalization.Key = popupData.title;
            countText.text = popupData.intParam.ToString();
        }
        #endregion

        #region Handlers
        private void OkButtonClick()
        {
            okAction.Invoke();
            CloseThisWindow();
        }
        #endregion
    }
}