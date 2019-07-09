//Developed by Pavel Kravtsov.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DestroyOnCollide2D : MonoBehaviour
    {
        private enum Type
        {
            This,
            CollidedObject
        }
        #region Fields
        [SerializeField]
        private Type whichObjectDestroy;
        [SerializeField]
        private List<string> tagsToDetect = new List<string>();
        [SerializeField]
        private float timeToDeactivate;
        [SerializeField]
        private bool setColorOnDestroy;
        [SerializeField]
        private Color colorBeforeDestroy;
        #endregion Fields

        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch(whichObjectDestroy)
            {
                case Type.CollidedObject:
                    if (tagsToDetect.Count == 0)
                        StartCoroutine(DeactivateAfter(timeToDeactivate, collision.gameObject));
                    else if(tagsToDetect.Contains(collision.gameObject.tag))
                        StartCoroutine(DeactivateAfter(timeToDeactivate, collision.gameObject));
                    break;
                case Type.This:
                    StartCoroutine(DeactivateAfter(timeToDeactivate, gameObject));
                    break;
            }
        }

        IEnumerator DeactivateAfter(float time, GameObject objToDestroy)
        {
            var startColor = new Color();
            if (setColorOnDestroy)
            {
                startColor = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = colorBeforeDestroy;
            }

            if (time > 0)
                yield return new WaitForSeconds(time);

            if (setColorOnDestroy)
                GetComponent<SpriteRenderer>().color = startColor;

            Destroy(objToDestroy);
            yield return null;
        }
    }
}