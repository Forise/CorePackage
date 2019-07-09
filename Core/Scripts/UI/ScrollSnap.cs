//Developed by Pavel Kravtsov.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ScrollSnap : MonoBehaviour
    {
        public event System.EventHandler OnCurrectObjectChanged = delegate { };
        #region Fields
        public RectTransform panel;
        public RectTransform centerAnchor;
        public float distanceAccuracy;
        public float lerpSpeed = 10f;
        public float buttonLerpSpeed = 100f;
        public AnimationCurve curve;
        public Button scrollLeftButton;
        public Button scrollRightButton;

        [SerializeField]
        protected MonoBehaviour[] gameObjects;
        [SerializeField]
        protected MonoBehaviour currentGameObject;

        private float[] distances;
        private bool isDragging = false;
        private bool isButtonPressed = false;
        private float objectOffset = 0;
        private int closestObject;
        private float maxDistance;
        #endregion

        #region Properties
        public MonoBehaviour CurrentGameObject { get => currentGameObject; }
        public MonoBehaviour[] Objects { get => gameObjects; }
        public int GetCurrentObjectIDInScroll
        {
            get
            {
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    if (CurrentGameObject == gameObjects[i])
                        return i;
                }
                return -1; //Error
            }
        }
        #endregion

        private void Awake()
        {
            scrollLeftButton.onClick.AddListener(ScrollLeft);
            scrollRightButton.onClick.AddListener(ScrollRight);

            distances = new float[gameObjects.Length];
            if (gameObjects.Length > 1)
                objectOffset = Mathf.Abs(gameObjects[1].GetComponent<RectTransform>().anchoredPosition.x - gameObjects[0].GetComponent<RectTransform>().anchoredPosition.x);
            maxDistance = (distances.Length - 1) * objectOffset;
        }

        private void Start()
        {
            

            //OnCurrectObjectChanged?.Invoke(CurrentGameObject, null);
        }

        private void Update()
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                distances[i] = Mathf.Abs(centerAnchor.transform.position.x - gameObjects[i].transform.position.x);
                //float time = curve.Evaluate(distances[i] / maxDistance);
                //gameObjects[i].gameObject.transform.localScale = Vector3.one * (time);
            }

            float minDistance = Mathf.Min(distances);

            for (int j = 0; j < gameObjects.Length; j++)
            {
                if(minDistance == distances[j])
                {
                    closestObject = j;
                }
            }

            if(!isDragging && !isButtonPressed)
            {
                LerpToButton(closestObject * (int)-objectOffset);
            }

            if (currentGameObject && distances[closestObject] <= distanceAccuracy)
            {
                currentGameObject/*.sprite*/ = gameObjects[closestObject]/*.sprite*/;
                OnCurrectObjectChanged?.Invoke(CurrentGameObject, null);
            }
        }

        private void ScrollLeft()
        {
            SetPos(closestObject - 1);
        }

        private void ScrollRight()
        {
            SetPos(closestObject + 1);
        }

        private void LerpToButton(int position, float? speed = null)
        {
            float newX = Mathf.Lerp(panel.anchoredPosition.x, position, Time.deltaTime * (speed ?? lerpSpeed));
            var newPos = new Vector2(newX, panel.anchoredPosition.y);

            panel.anchoredPosition = newPos;
            isDragging = false;
            isButtonPressed = false;
        }


        public void SetPos(int pos)
        {
            isButtonPressed = true;
            LerpToButton(pos * (int)-objectOffset, buttonLerpSpeed);
        }

        public void StartDrag()
        {
            isDragging = true;
        }

        public void StopDrag()
        {
            isDragging = false;
        }
    }
}