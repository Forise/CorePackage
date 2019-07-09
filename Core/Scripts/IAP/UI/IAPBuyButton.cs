//Developed by Pavel Kravtsov.

namespace Core.IAP
{
    public class IAPBuyButton : MyButton
    {
        public string productID;
        public TMPro.TextMeshProUGUI priceText;

        protected override void Start()
        {
            base.Start();
            onClick.AddListener(OnClick);
        }

        protected override void OnDestroy()
        {
            onClick.RemoveListener(OnClick);
            base.OnDestroy();
        }

        public void SetPrice(string price)
        {
            priceText.text = price;
        }

        private void OnClick()
        {
            //TODO: change text to localization ID
            UIControl.Instance.ShowDialogPopUp(new UIPopUp.PopupData(/*"localization_ID"*/"Do you really want to buy it?",
                () => 
                {
                    IAPManager.Instance.BuyProductID(productID);
                }));
        }
    }
}