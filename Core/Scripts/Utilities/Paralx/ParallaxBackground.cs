//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [ExecuteInEditMode]
    public class ParallaxBackground : MonoBehaviour
    {
        public ParallaxCamera parallaxCamera;
        [SerializeField]
        List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

        void Awake()
        {
            if (parallaxCamera == null)
                parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
            SetLayers();
        }

        private void OnDestroy()
        {
            parallaxCamera.onCameraTranslate -= Move;
        }

        public void SetLayers()
        {
            parallaxCamera.onCameraTranslate -= Move;
            parallaxLayers.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                ParallaxLayer[] layers = transform.GetChild(i).GetComponentsInChildren<ParallaxLayer>();

                if (layers != null)
                {
                    foreach (var l in layers)
                    {
                        parallaxLayers.Add(l);
                    }
                }
            }
            parallaxCamera.onCameraTranslate += Move;
        }
        void Move(float delta)
        {
            foreach (ParallaxLayer layer in parallaxLayers)
            {
                layer.Move(delta);
            }
        }
    }
}