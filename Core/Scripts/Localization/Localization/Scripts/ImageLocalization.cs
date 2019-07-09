using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ImageLocalization : MonoBehaviour
    {

        private Image _image;
        private string _name;


        void Start()
        {
            Initialize();

        }

        private void Initialize()
        {
            LocalizationManager.OnChangeLocalization += OnChangeLocalization;
            _image = GetComponent<Image>();
            _name = _image.sprite.name;

            OnChangeLocalization();
        }

        private void OnChangeLocalization()
        {
            LocalizeImage();
        }

        private void LocalizeImage()
        {
            if(LocalizationManager.Instance != null)
            SetSpriteToImage(LocalizationManager.Instance.GetSprite(_name));
        }

        private void SetSpriteToImage(Sprite sprite)
        {

            if (sprite == null) return;

            if (_image != null)
            {
                _image.sprite = sprite;
            }
        }

        private void OnDestroy()
        {
            //if (LocalizationManager.I != null)
                LocalizationManager.OnChangeLocalization -= OnChangeLocalization;
        }
    }
}