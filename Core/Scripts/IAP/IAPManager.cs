//Developed by Pavel Kravtsov.
using Core.EventSystem;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace Core.IAP
{
    public class IAPManager : MonoSingleton<IAPManager>, IStoreListener
    {
        public delegate void SuccessPurchaseDelegate(PurchaseEventArgs args);
        public delegate void FailedPurchaseDelegate(Product product, PurchaseFailureReason failureReason);

        public static event SuccessPurchaseDelegate OnPurchaseComplete;
        public static event FailedPurchaseDelegate PurchaseFailed;

        #region Fields
        public IAPProductsObject productsObject;

        private static IStoreController m_StoreController;
        private static IExtensionProvider m_StoreExtensionProvider;
#if UNITY_IOS
        private static IAppleExtensions m_AppleExtensions;
#endif
        private string currentProduct;
        #endregion Fields

        private void Awake()
        {
            InitializePurchasing();
        }

#region INIT
        public void InitializePurchasing()
        {
            #if !UNITY_EDITOR
                #if UNITY_IOS
                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
                builder.Configure<IAppleConfiguration>();
                #elif UNITY_ANDROID
                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
                #endif
                foreach (var product in productsObject.Products)
                {
                    builder.AddProduct(product.id, product.type, new IDs
                {
                    {product.id, AppleAppStore.Name}
                });
                }
                UnityPurchasing.Initialize(this, builder);
            #endif
        }

        public static bool IsInitialized()
        {
            if (m_StoreController != null && m_StoreExtensionProvider != null)
                return true;
            else
                Instance.InitializePurchasing();
            return true;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            #if UNITY_IOS
            m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);
            #endif
            //Debug.Log("OnInitialized: PASS");

            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError("OnInitializeFailed InitializationFailureReason:" + error);
        }
#endregion

#region PURCHASE
        public void BuyProductID(string productId)
        {
            if (IsInitialized())
            {
                currentProduct = productId;
                Product product = m_StoreController.products.WithID(productId);
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    OnPurchaseFailed(product, PurchaseFailureReason.ProductUnavailable);
                }
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
    #if UNITY_IOS
            string transactionReceipt = m_AppleExtensions.GetTransactionReceiptForProduct(args.purchasedProduct);
            //Debug.Log(transactionReceipt);
            //if (string.Equals(args.purchasedProduct.definition.id, currentProduct, System.StringComparison.Ordinal))
            //    OnSuccess(args);
            //else
            //    Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            // Send transaction receipt to server for validation

            List<string> products = new List<string>();
            foreach(var product in productsObject.Products)
            {
                products.Add(product.id);
            }

            if (productsObject.Products.Count > 0 && string.Equals(args.purchasedProduct.definition.id, products.Single(p => p == args.purchasedProduct.definition.id), System.StringComparison.Ordinal))
                OnSuccess(args);
            else Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));

            return PurchaseProcessingResult.Complete;

    #elif UNITY_ANDROID
            List<string> products = new List<string>();
            foreach (var product in productsObject.Products)
            {
                products.Add(product.id);
            }

            #region Debug
            if (productsObject.Products.Count <= 0)
                Debug.LogError("Products count 0 or less");
            Debug.LogError($"args.purchasedProduct.definition.id = [{args.purchasedProduct.definition.id}]");
            Debug.LogError($"products[0] = [{products[0]}]");
            if (args.purchasedProduct.definition.id != products[0])
                Debug.LogError($"Products are different!");
            if (!products.Contains(args.purchasedProduct.definition.id))
                Debug.LogError($"id isn't contains in products");
            if (!string.Equals(args.purchasedProduct.definition.id, products.Single(s => s == args.purchasedProduct.definition.id)))
                Debug.LogError($"Unrecognized product!");
            #endregion Debug

            if (productsObject.Products.Count > 0 && string.Equals(args.purchasedProduct.definition.id, products.Single(p => p == args.purchasedProduct.definition.id), System.StringComparison.Ordinal))
                OnSuccess(args);
            else
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            return PurchaseProcessingResult.Complete;
    #endif
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            OnFailed(product, failureReason);
        }

        protected virtual void OnSuccess(PurchaseEventArgs args)
        {
            OnPurchaseComplete?.Invoke(args);
            EventManager.Notify(this, new GameEventArgs(Events.IAPEvents.SUCCESSFULL_BOUGHT, args));
            Debug.Log(currentProduct + " Bought!");
        }

        protected virtual void OnFailed(Product product, PurchaseFailureReason failureReason)
        {
            PurchaseFailed?.Invoke(product, failureReason);
            EventManager.Notify(this, new GameEventArgs(Events.IAPEvents.FAILED_BOUGHT, failureReason.ToString(), product));
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
#endregion

        private void OnDeferred(Product item)
        {
            Debug.Log("Purchase deferred: " + item.definition.id);
        }

#region TOOLS
        /// <summary>
        /// Returns is product bought.
        /// </summary>
        /// <param name="id">Product ID</param>
        public static bool CheckBuyState(string id)
        {
            Product product = m_StoreController.products.WithID(id);
            return product.hasReceipt;
        }

        /// <summary>
        /// Returns localized price of product.
        /// </summary>
        /// <param name="id">Product ID</param>
        public static string GetProductPrice(string id)
        {
            Product product = m_StoreController.products.WithID(id);
            if(product != null)
                return product.metadata.localizedPriceString;
            else
            {
                Debug.LogError("Product is null");
                return "Null Prod";
            }
        }
#endregion
    }
}