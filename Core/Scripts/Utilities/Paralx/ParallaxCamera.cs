﻿//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    [ExecuteInEditMode]
    public class ParallaxCamera : MonoBehaviour
    {
        public delegate void ParallaxCameraDelegate(float deltaMovement);
        public ParallaxCameraDelegate onCameraTranslate;
        private float oldPosition;
        void Awake()
        {
            oldPosition = transform.position.x;
        }
        void Update()
        {
            if (transform.position.x != oldPosition)
            {
                if (onCameraTranslate != null)
                {
                    float delta = oldPosition - transform.position.x;
                    onCameraTranslate(delta);
                }
                oldPosition = transform.position.x;
            }
        }
    }
}