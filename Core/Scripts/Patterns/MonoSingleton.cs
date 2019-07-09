//Developed by Pavel Kravtsov.
using UnityEngine;
namespace Core
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        private static bool wasDestroyed;

        public static T Instance
        {
            get
            {
                if (wasDestroyed)
                    return null;

                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType(typeof(T)) as T;

                    if (instance == null)
                    {
                        GameObject gameObject = new GameObject(typeof(T).Name);
                        DontDestroyOnLoad(gameObject);

                        instance = gameObject.AddComponent(typeof(T)) as T;
                    }
                }
                return instance;
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance)
                Destroy(instance);

            instance = null;
            wasDestroyed = true;
        }
    }
}