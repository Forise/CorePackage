//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public class CameraShake : MonoBehaviour
    {
        // Transform of the camera to shake. Grabs the gameObject's transform
        // if null.
        [SerializeField]
        private Transform camTransform;

        // How long the object should shake for.
        [SerializeField]
        private float shakeDuration = 0f;

        // Amplitude of the shake. A larger value shakes the camera harder.
        [SerializeField]
        private float shakeAmount = 0.7f;
        [SerializeField]
        private float decreaseFactor = 1.0f;
        private Vector3 originalPos;
        [SerializeField]
        private bool use2D;

        public bool IsShaking { get; set; }
        public bool UseBoolShaking { get; set; }


        void Awake()
        {
            if (camTransform == null)
            {
                camTransform = Camera.main.transform;
            }
        }

        void OnEnable()
        {
            originalPos = camTransform.localPosition;
        }

        void Update()
        {
            if (!UseBoolShaking)
            {
                if (shakeDuration > 0)
                {
                    IsShaking = true;
                    Shake();

                    shakeDuration -= Time.deltaTime * decreaseFactor;
                }
                else
                {
                    IsShaking = false;
                    shakeDuration = 0f;
                    camTransform.localPosition = originalPos;
                }
            }
            else if(IsShaking)
            {
                Shake();
            }
        }

        private void Shake()
        {
            if (!use2D)
                camTransform.localPosition = camTransform.localPosition + Random.insideUnitSphere * shakeAmount;
            else
            {
                var newPos = Random.insideUnitCircle * shakeAmount;
                var xyPos = new Vector2(originalPos.x + newPos.x, originalPos.y + newPos.y);
                camTransform.localPosition = new Vector3(xyPos.x, xyPos.y, camTransform.transform.localPosition.z);
            }
        }

        public void StartShake(float duration)
        {
            if (!UseBoolShaking)
            {
                shakeDuration = duration;
                IsShaking = true;
            }
            else
                IsShaking = true;
        }
        public void StopShake()
        {
            if (!UseBoolShaking)
            {
                shakeDuration = 0f;
                IsShaking = false;
            }
            else
                IsShaking = false;
        }
    }
}
