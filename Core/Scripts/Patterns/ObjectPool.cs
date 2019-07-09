//Developed by Pavel Kravtsov.
using System.Collections.Generic;
using UnityEngine;
namespace Core
{
    [System.Serializable]
    public class ObjectPool<T> : MonoBehaviour where T : Component
    {
        #region Fields
        [SerializeField]
        private T prefab;
        [SerializeField]
        private int count = 10;
        public List<T> pool;
        private List<T> holdedObjects = new List<T>();
        private bool isInited = false;
        #endregion Fields

        #region Properties
        public IReadOnlyList<T> HoldedObjects { get => holdedObjects.AsReadOnly(); }

        public T Prefab { get => prefab; }

        public int Count { get => count; }
        #endregion Properties

        private void Awake()
        {
            Init();
        }

        public ObjectPool() { pool = new List<T>(); }

        public ObjectPool(List<T> list)
        {
            pool = new List<T>();
            pool = list;
            foreach (var obj in pool)
            {
                (obj as GameObject).gameObject.SetActive(false);
            }
            isInited = true;
        }

        private void Init()
        {
            if (!isInited)
            {
                pool = new List<T>();
                for (int i = 0; i < count; i++)
                {
                    T obj = Instantiate(prefab, transform);
                    if (obj == null)
                        Debug.LogError("Instantiation of objoct from pool is failed! Object is NULL!", this);
                    obj.name += i;
                    obj.gameObject.SetActive(false);
                    pool.Add(obj);
                }
                isInited = true;
            }
        }

        public T Pull(bool hold = false)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].gameObject.activeInHierarchy)
                {
                    if (hold)
                        holdedObjects.Add(pool[i]);
                    return pool[i];
                }
            }
            Debug.LogWarning("Object is null! Check count of objects in pull!");
            return null;
        }

        public T PullRandom(bool hold = false)
        {
            var rnd = Random.Range(0, pool.Count);
            if (!pool[rnd].gameObject.activeInHierarchy)
            {
                if (hold)
                    holdedObjects.Add(pool[rnd]);
                return pool[rnd];
            }
            return null;
        }

        public void Push(T obj, bool forcePush = false)
        {
            if (!obj)
                Debug.LogError("Pushing object to pool is NULL!", this);
            if (obj.gameObject.activeInHierarchy)
            {
                try
                {
                    var founded = pool.Find(x => x == obj);
                    if (founded && (!holdedObjects.Contains(founded) || forcePush))
                    {
                        founded.gameObject.SetActive(false);
                        holdedObjects.Remove(founded);
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError(ex, this);
                }
            }
        }
    }
}