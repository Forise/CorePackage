//Developed by Pavel Kravtsov.
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class SwitchSpriteToggle : Toggle
    {
        [SerializeField]
        public Image onImage;
        [SerializeField]
        public Image offImage;

        protected override void Awake()
        {
            base.Awake();
            onImage.gameObject.SetActive(isOn);
            offImage.gameObject.SetActive(!isOn);
            onValueChanged.AddListener(OnValueChanged_Handler);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onValueChanged.RemoveListener(OnValueChanged_Handler);
        }

        private void OnValueChanged_Handler(bool value)
        {
            onImage.gameObject.SetActive(value);
            offImage.gameObject.SetActive(!value);
        }
    }
}