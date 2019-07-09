//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Core
{
    public class UIInfoPopUp : UIPopUp
    {
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
            base.SetContent();
        }

        private void SetActions()
        {
            okAction = popupData.confirmAction;
        }
        #endregion

        #region Handlers
        private void OkButtonClick()
        {
            okAction?.Invoke();
            CloseThisWindow();
        }
        #endregion
    }
}