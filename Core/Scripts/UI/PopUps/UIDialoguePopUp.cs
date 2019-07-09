//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public class UIDialoguePopUp : UIPopUp
    {
        [SerializeField]
        private MyButton okButton;
        [SerializeField]
        private MyButton cancelButton;

        private System.Action okAction;
        private System.Action cancelAction;

        protected override void Start()
        {
            okButton.onClick.AddListener(OkButtonClick);
            cancelButton.onClick.AddListener(CancelButtonClick);
            base.Start();
        }

        private void OnDestroy()
        {
            okButton.onClick.RemoveListener(OkButtonClick);
            cancelButton.onClick.RemoveListener(CancelButtonClick);
        }

        protected override void ActiveStateUpdateHandler()
        {
            base.ActiveStateUpdateHandler();
            CheckMissClick(cancelAction);
        }

        protected override void InactiveStateInitHandler()
        {
            base.InactiveStateInitHandler();
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
            cancelAction = popupData.declineAction;
        }
        #endregion

        #region Handlers
        private void OkButtonClick()
        {
            okAction?.Invoke();
            CloseThisWindow();
        }
        private void CancelButtonClick()
        {
            //TODO: add check null Actions. add warning if null! 
            cancelAction?.Invoke();
            CloseThisWindow();
        }
        #endregion
    }
}