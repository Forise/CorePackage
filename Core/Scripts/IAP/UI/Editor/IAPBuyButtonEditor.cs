//Developed by Pavel Kravtsov.
using UnityEditor;

namespace Core.IAP
{
    [CustomEditor(typeof(IAPBuyButton))]
    public class IAPBuyButtonEditor : UnityEditor.UI.ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            IAPBuyButton iapBuyButton = (IAPBuyButton)target;
            iapBuyButton.productID = EditorGUILayout.TextField("Product ID", iapBuyButton.productID);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            iapBuyButton.priceText = (TMPro.TextMeshProUGUI)EditorGUILayout.ObjectField("Price Text", iapBuyButton.priceText, typeof(TMPro.TextMeshProUGUI), true);
            EditorGUILayout.EndHorizontal();
            serializedObject.Update();
        }
    }
}