//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using System.Reflection;
using System.Net;
using UnityEngine;
namespace Core
{
    public static class Utilities
    {
        public static bool SimpleHasConnection
        {
            get
            {
                if (Application.internetReachability != NetworkReachability.NotReachable)
                    return true;
                else
                {
                    ShowNoInternetConnectionPopUp();
                    return false;
                }
            }
        }

        public static bool HasConnection
        {
            get
            {
                try
                {
                    using (var stream = new WebClient().OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
                catch
                {
                    Debug.Log("HAS NO INTERNET!");
                    ShowNoInternetConnectionPopUp();
                    return false;
                }
            }
        }

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }

            return copy as T;
        }

        public static void ShowNoInternetConnectionPopUp()
        {
            UIControl.Instance.ShowInfoPopUp(new UIPopUp.PopupData("no_internet"));
        }

        public static bool CheckConnectionToURL(string URL)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = 5000;
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK) return true;
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCollisionTag(Collider2D collision, string tag)
        {
            return collision.gameObject.tag.Contains(tag);
        }

        public static bool CheckCollisionTag(Collider2D collision, List<string> tags)
        {
            foreach(var t in tags)
            {
                if (collision.gameObject.tag.Contains(t))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// If returns "-1" - ERROR
        /// </summary>
        public static float GetDistance(GameObject from, GameObject to, bool x = true, bool y = true, bool z = true)
        {
            if (x && y && z)
                return Mathf.Sqrt(Mathf.Pow(from.transform.position.x - to.transform.position.x, 2) + Mathf.Pow(from.transform.position.y - to.transform.position.y, 2) + Mathf.Pow(from.transform.position.z - to.transform.position.z, 2));
            else if (x && y && !z)
                return Mathf.Sqrt(Mathf.Pow(from.transform.position.x - to.transform.position.x, 2) + Mathf.Pow(from.transform.position.y - to.transform.position.y, 2));
            else if (x && !y && z)
                return Mathf.Sqrt(Mathf.Pow(from.transform.position.x - to.transform.position.x, 2) + Mathf.Pow(from.transform.position.z - to.transform.position.z, 2));
            else if (!x && y && z)
                return Mathf.Sqrt(Mathf.Pow(from.transform.position.y - to.transform.position.y, 2) + Mathf.Pow(from.transform.position.z - to.transform.position.z, 2));
            else if (x && !y && !z)
                return Mathf.Sqrt(Mathf.Pow(from.transform.position.x - to.transform.position.x, 2));
            else if (!x && y && !z)
                return Mathf.Sqrt(Mathf.Pow(from.transform.position.y - to.transform.position.y, 2));
            else if (!x && !y && z)
                return Mathf.Sqrt(Mathf.Pow(from.transform.position.z - to.transform.position.z, 2));
            else return -1f; //-1 error
        }
    }
}