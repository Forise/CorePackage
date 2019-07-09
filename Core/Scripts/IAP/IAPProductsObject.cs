//Developed by Pavel Kravtsov.
using UnityEngine;
using System.Collections.Generic;

namespace Core.IAP
{
    [CreateAssetMenu(fileName = "IAPProducts", menuName = "Core/ScriptableObjects/IAPProducts")]
    public class IAPProductsObject : ScriptableObject
    {
        [System.Serializable]
        public class Product
        {
            public string id;
            public UnityEngine.Purchasing.ProductType type;
        }
        [SerializeField]
        private List<Product> products;

        public List<Product> Products
        {
            get { return products; }
            private set { products = value; }
        }
    }
}