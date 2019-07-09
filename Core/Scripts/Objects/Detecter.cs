//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Collider2D))]
    public class Detecter : MonoBehaviour
    {
        #region Fiedls
        public Collider2D col;
        public string tagToDetect;
        public bool detectOnlyEnabledObjects;

        public List<GameObject> detectedObjects = new List<GameObject>();
        #endregion Fiedls
        private void Awake()
        {
            if (!col)
                col = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (detectOnlyEnabledObjects)
            {
                foreach (var o in detectedObjects)
                {
                    if (!o.activeInHierarchy)
                    {
                        detectedObjects.Remove(o);
                        break;
                    }
                }
            }
        }

        public GameObject GetObject()
        {
            if (detectedObjects.Count > 0)
                return detectedObjects[detectedObjects.Count - 1];
            else
                return null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision && (string.IsNullOrEmpty(tagToDetect) || collision.gameObject.tag == tagToDetect || collision.gameObject.tag.Contains(tagToDetect)))
            {
                detectedObjects.Add(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision)
            {
                detectedObjects.Remove(collision.gameObject);
            }
        }
    }
}