//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;

namespace Core
{
    [RequireComponent(typeof(Image))]
    public class ImageEventButton : MonoBehaviour
    {
        UnityEngine.EventSystems.EventTrigger e;
        public Image image;
        public List<string> eventsToNotify;

        private void OnMouseDown()
        {
            foreach (var e in eventsToNotify)
            {
                EventManager.Notify(e, null);
            }
        }
    }
}