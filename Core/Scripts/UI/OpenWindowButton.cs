//Developed by Pavel Kravtsov.
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    [RequireComponent(typeof(MyButton), typeof(Image))]
    public class OpenWindowButton : MonoBehaviour
    {
        public string windowIDToOpen;
        public MyButton button;
        public bool onlyWithInternet;
        private Image image;

        protected void Awake()
        {
            if(button == null)
                button = GetComponent<MyButton>();
            if (image == null)
                image = GetComponent<Image>();
            if (button.targetGraphic == null)
                button.targetGraphic = image;        
            button.onClick.AddListener(ButtonClick_Handler);
        }

        protected void OnDestroy()
        {
            button.onClick.RemoveListener(ButtonClick_Handler);
        }

        private void ButtonClick_Handler()
        {
            if (!onlyWithInternet || Utilities.SimpleHasConnection)
            {
                UIControl.Instance.OpenWindow(windowIDToOpen, this.GetType().Name);
            }
        }
    }
}