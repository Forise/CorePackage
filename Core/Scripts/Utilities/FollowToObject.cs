//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core
{
    public class FollowToObject : MonoBehaviour
    {
        public GameObject objectToFollow;
        public bool relative;
        public bool XAxis;
        public bool YAxis;
        public bool ZAxis;
        public float XSafeStep;
        public float YSafeStep;
        public float ZSafeStep;

        private float distance;
        private float Xoffset;
        private float Yoffset;
        private float Zoffset;

        [SerializeField]
        private bool isFollowing = false;

        public bool IsFollowing
        {
            get => isFollowing;
        }

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (objectToFollow && isFollowing)
            {
                if (!relative)
                {
                    if (XAxis && !YAxis && !ZAxis)
                        transform.position = new Vector3(objectToFollow.transform.position.x, transform.position.y, transform.position.z);
                    else if (XAxis && YAxis && !ZAxis)
                        transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, transform.position.z);
                    else if (XAxis && !YAxis && ZAxis)
                        transform.position = new Vector3(objectToFollow.transform.position.x, transform.position.y, objectToFollow.transform.position.z);
                    else if (XAxis && YAxis && ZAxis)
                        transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, objectToFollow.transform.position.z);
                    else if (!XAxis && YAxis && !ZAxis)
                        transform.position = new Vector3(transform.position.x, objectToFollow.transform.position.y, transform.position.z);
                    else if (!XAxis && YAxis && ZAxis)
                        transform.position = new Vector3(transform.position.x, objectToFollow.transform.position.y, objectToFollow.transform.position.z);
                    else if (!XAxis && !YAxis && ZAxis)
                        transform.position = new Vector3(transform.position.x, transform.position.y, objectToFollow.transform.position.z);
                }
                else
                {
                    //distance = Mathf.Sqrt(Mathf.Pow(objectToFollow.transform.position.x - transform.position.x, 2) +
                    //    Mathf.Pow(objectToFollow.transform.position.y - transform.position.y, 2) +
                    //    Mathf.Pow(objectToFollow.transform.position.z - transform.position.z, 2));

                    if (XAxis && !YAxis && !ZAxis &&
                        (XSafeStep >= objectToFollow.transform.position.x - transform.position.x || XSafeStep <= objectToFollow.transform.position.x - transform.position.x))
                    {
                        transform.position = new Vector3(objectToFollow.transform.position.x + Xoffset,
                            transform.position.y,
                            transform.position.z);
                    }
                    else if (XAxis && YAxis && !ZAxis &&
                        (XSafeStep >= objectToFollow.transform.position.x - transform.position.x || XSafeStep <= objectToFollow.transform.position.x - transform.position.x) &&
                        (YSafeStep >= objectToFollow.transform.position.y - transform.position.y || YSafeStep <= objectToFollow.transform.position.y - transform.position.y))
                    {
                        transform.position = new Vector3(objectToFollow.transform.position.x + Xoffset,
                            objectToFollow.transform.position.y + Yoffset,
                            transform.position.z);
                    }
                    else if (XAxis && !YAxis && ZAxis &&
                        XSafeStep >= objectToFollow.transform.position.x - transform.position.x &&
                        ZSafeStep >= objectToFollow.transform.position.z - transform.position.z)
                    {
                        transform.position = new Vector3(objectToFollow.transform.position.x + Xoffset,
                            transform.position.y,
                            objectToFollow.transform.position.z + Zoffset);
                    }
                    else if (XAxis && YAxis && ZAxis &&
                        XSafeStep >= objectToFollow.transform.position.x - transform.position.x &&
                        YSafeStep >= objectToFollow.transform.position.y - transform.position.y &&
                        ZSafeStep >= objectToFollow.transform.position.z - transform.position.z)
                    {
                        transform.position = new Vector3(objectToFollow.transform.position.x + Xoffset,
                            objectToFollow.transform.position.y + Yoffset,
                            objectToFollow.transform.position.z + Zoffset);
                    }
                    else if (!XAxis && YAxis && !ZAxis &&
                        YSafeStep >= objectToFollow.transform.position.y - transform.position.y)
                    {
                        transform.position = new Vector3(transform.position.x,
                            objectToFollow.transform.position.y + Yoffset,
                            transform.position.z);
                    }
                    else if (!XAxis && YAxis && ZAxis &&
                        YSafeStep >= objectToFollow.transform.position.y - transform.position.y &&
                        ZSafeStep >= objectToFollow.transform.position.z - transform.position.z)
                    {
                        transform.position = new Vector3(transform.position.x,
                            objectToFollow.transform.position.y + Yoffset,
                            objectToFollow.transform.position.z + Zoffset);
                    }
                    else if (!XAxis && !YAxis && ZAxis &&
                        ZSafeStep >= objectToFollow.transform.position.z - transform.position.z)
                    {
                        transform.position = new Vector3(transform.position.x,
                            transform.position.y,
                            objectToFollow.transform.position.z + Zoffset);
                    }
                }
            }
            //else
            //    Debug.LogError("Object to follow is Null!", this);
        }

        public void SetupObject(GameObject obj)
        {
            StopFollow();
            objectToFollow = obj;
            Init();
            StartFollow();
        }

        private void Init()
        {
            if (objectToFollow)
            {
                Xoffset = transform.position.x - objectToFollow.transform.position.x;
                Yoffset = transform.position.y - objectToFollow.transform.position.y;
                Zoffset = transform.position.z - objectToFollow.transform.position.z;
            }
        }

        public void StartFollow()
        {
            isFollowing = true;
        }

        public void StopFollow()
        {
            objectToFollow = null;
            isFollowing = false;
        }
    }
}