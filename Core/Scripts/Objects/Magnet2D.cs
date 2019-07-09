//Developed by Pavel Kravtsov.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
    public class Magnet2D : MonoBehaviour
    {
        #region Fields
        public bool isAnable = false;
        public bool powerByDistance = false;
        public float attractionPower = 1f;
        [SerializeField]
        private string tagToDetect;
        [SerializeField]
        private CircleCollider2D col;
        [SerializeField]
        private bool isStopAttractingByExit = false;
        private Dictionary<GameObject, Coroutine> attractingObjects = new Dictionary<GameObject, Coroutine>();
        #endregion

        #region Properties
        #endregion

        #region Mono Methods
        private void Awake()
        {
            if (!col)
                col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isAnable && collision && collision.gameObject.tag.Contains(tagToDetect) && !attractingObjects.ContainsKey(collision.gameObject))
            {
                var coroutine = StartCoroutine(AttractCoroutine(collision.gameObject));
                attractingObjects.Add(collision.gameObject, coroutine);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(isStopAttractingByExit && collision && collision.gameObject.tag.Contains(tagToDetect) && attractingObjects.ContainsKey(collision.gameObject))
            {
                StopCoroutine(attractingObjects[collision.gameObject]);
                attractingObjects.Remove(collision.gameObject);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (isAnable && collision && collision.gameObject.tag.Contains(tagToDetect) && !attractingObjects.ContainsKey(collision.gameObject))
            {
                var coroutine = StartCoroutine(AttractCoroutine(collision.gameObject));
                attractingObjects.Add(collision.gameObject, coroutine);
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
        #endregion

        #region Methods
        public void SetRadius(float rad)
        {
            col.radius = rad;
        }

        public void SetTagToDetect(string tag)
        {
            tagToDetect = tag;
        }

        private IEnumerator AttractCoroutine(GameObject obj)
        {
            while(obj && obj.activeInHierarchy && isAnable)
            {
                var newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                if (powerByDistance)
                    yield return obj.transform.position = Vector3.MoveTowards(obj.transform.position, newPos,
                        attractionPower /Mathf.Abs(Vector3.Distance(transform.position, obj.transform.position))  * Time.deltaTime);
                else
                    yield return obj.transform.position = Vector3.MoveTowards(obj.transform.position, newPos, attractionPower * Time.deltaTime);
            }
            attractingObjects.Remove(obj);
        }
        #endregion
    }
}