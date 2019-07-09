using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Core
{
    public class SoundLocalization : MonoBehaviour
    {

        private AudioClip _clip;
        private string _name;


        void Start()
        {
            Initialize();

        }

        private void Initialize()
        {
            LocalizationManager.OnChangeLocalization += OnChangeLocalization;
            //_clip = GetComponent<Image>();
            //_name = _clip.sprite.name;

            OnChangeLocalization();
        }

        private void OnChangeLocalization()
        {
            LocalizeImage();
        }

        private void LocalizeImage()
        {
            SetSpriteToImage(LocalizationManager.Instance.GetSprite(_name));
        }

        private void SetSpriteToImage(Sprite sprite)
        {

            if (sprite == null) return;

            if (_clip != null)
            {
                //_clip.sprite = sprite;
            }
        }

        private void OnDestroy()
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.OnChangeLocalization -= OnChangeLocalization;
        }
    }
}