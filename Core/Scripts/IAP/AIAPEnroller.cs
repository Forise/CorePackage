//Developed by Pavel Kravtsov.
using UnityEngine;
using UnityEngine.Purchasing;
using Core.EventSystem;
namespace Core.IAP
{
    public abstract class AIAPEnroller : MonoBehaviour
    {
        public string productID;

        private void Awake()
        {
            EventManager.Subscribe(Events.IAPEvents.SUCCESSFULL_BOUGHT, EnrollPurchase);
        }

        protected virtual void EnrollPurchase(object sender, Core.EventSystem.GameEventArgs args)
        {
            var boughtID = (args.objectParam as PurchaseEventArgs).purchasedProduct.definition.id;

            if (boughtID == productID)
            {
                Enroll();
            }
            else
            {
                Debug.LogError($"Receipt is not equal! /n current Receipt: {productID}. Bought Receipt {boughtID}.");
            }
        }

        protected abstract void Enroll();
    }
}