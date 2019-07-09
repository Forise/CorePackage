using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.EventSystem;
using System;
using UnityEngine.UI;

namespace Core
{
    public class InputControl : MonoSingleton<InputControl>
    {
        private List<object> blockedObjects = new List<object>();
        private void Awake()
        {
            EventManager_Input.Subscribe(EventManager_Input.BLOCK_INPUT, OnBlockInput_Handler);
            EventManager_Input.Subscribe(EventManager_Input.UNBLOCK_INPUT, OnUnblockInput_Handler);
        }

        #region Handlers
        private void OnBlockInput_Handler(object sender, GameEventArgs e)
        {
            var graphicObjects = FindObjectsOfType<Graphic>();
            foreach(var o in graphicObjects)
            {
                if (o.raycastTarget)
                {
                    o.raycastTarget = false;
                    blockedObjects.Add(o);
                }
            }
            var buttonObjects = FindObjectsOfType<Button>();
            foreach (var o in buttonObjects)
            {
                if (o.interactable)
                {
                    o.interactable = false;
                    blockedObjects.Add(o);
                }
            }
            var selectableObjects = FindObjectsOfType<Button>();
            foreach (var o in selectableObjects)
            {
                if (o.interactable)
                {
                    o.interactable = false;
                    blockedObjects.Add(o);
                }
            }
        }

        private void OnUnblockInput_Handler(object sender, GameEventArgs e)
        {
            foreach(var o in blockedObjects)
            {
                switch(o)
                {
                    case Graphic g:
                        g.raycastTarget = true;
                        break;
                    case Button b:
                        b.interactable = true;
                        break;
                    case Selectable s:
                        s.interactable = true;
                        break;
                }
            }
            blockedObjects.Clear();
        }
        #endregion Handlers
    }
}