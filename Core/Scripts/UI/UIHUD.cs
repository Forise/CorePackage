//Developed by Pavel Kravtsov.
using System.Collections;
using UnityEngine;
using Core.EventSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Core
{
    public class UIHUD<T> : MonoSingleton<T> where T : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        protected GameObject[] hudObjects;
        [SerializeField]
        protected GameObject topContent;
        [SerializeField]
        protected GameObject botContent;
        [SerializeField]
        private AnimationCurve hideAnimation;
        [SerializeField]
        private AnimationCurve showAnimation;
        #endregion

        protected virtual void Awake()
        {
            EventManager_Input.Subscribe(EventManager_Input.BLOCK_HUD, BlockHUD);
            EventManager_Input.Subscribe(EventManager_Input.UNBLOCK_HUD, UnblockHUD);
            EventManager_Input.Subscribe(EventManager_Input.HIDE_HUD, HideHUD);
            EventManager_Input.Subscribe(EventManager_Input.SHOW_HUD, ShowHUD);
        }

        protected override void OnDestroy()
        {
            EventManager_Input.Unsubscribe(EventManager_Input.BLOCK_HUD, BlockHUD);
            EventManager_Input.Unsubscribe(EventManager_Input.UNBLOCK_HUD, UnblockHUD);
            EventManager_Input.Unsubscribe(EventManager_Input.HIDE_HUD, HideHUD);
            EventManager_Input.Unsubscribe(EventManager_Input.SHOW_HUD, ShowHUD);
            base.OnDestroy();
        }

        #region Methods
        protected virtual GameObject[] GetChildren()
        {
            GameObject[] gameObjects = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                gameObjects[i] = transform.GetChild(i).gameObject;
            }
            return gameObjects;
        }

        protected virtual void BlockHUD(object sender, GameEventArgs args)
        {
            foreach (var o in hudObjects)
            {
                var graphic = o.gameObject.GetComponent<Graphic>();
                if (graphic)
                    graphic.raycastTarget = false;
                    
                var selectable = o.gameObject.GetComponent<Selectable>();
                if (selectable)
                    selectable.interactable = false;

                var button = o.gameObject.GetComponent<Button>();
                if (button)
                    button.interactable = false;

                var eventTrigger = o.gameObject.GetComponent<EventTrigger>();
                if (eventTrigger)
                    eventTrigger.enabled = false;
            }
        }

        protected virtual void UnblockHUD(object sender, GameEventArgs args)
        {
            foreach (var o in hudObjects)
            {
                var graphic = o.gameObject.GetComponent<Graphic>();
                if (graphic)
                    graphic.raycastTarget = true;

                var selectable = o.gameObject.GetComponent<Selectable>();
                if (selectable)
                    selectable.interactable = true;

                var button = o.gameObject.GetComponent<Button>();
                if (button)
                    button.interactable = true;

                var eventTrigger = o.gameObject.GetComponent<EventTrigger>();
                if(eventTrigger)
                    eventTrigger.enabled = true;
            }
        }

        protected virtual void HideHUD(object sender, GameEventArgs args)
        {
            //HACK: temp hiding;
            topContent.SetActive(false);
            botContent.SetActive(false);
            //StartCoroutine(HideAnimationCoroutine());
        }

        protected virtual void ShowHUD(object sender, GameEventArgs args)
        {
            //HACK: temp showing;
            topContent.SetActive(true);
            botContent.SetActive(true);
            //StartCoroutine(ShowAnimationCoroutine());
        }

        protected virtual IEnumerator HideAnimationCoroutine()
        {
            yield return null;
        }

        protected virtual IEnumerator ShowAnimationCoroutine()
        {
            yield return null;
        }
        #endregion
    }
}