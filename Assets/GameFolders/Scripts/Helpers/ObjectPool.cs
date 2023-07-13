using System.Collections.Generic;
using UnityEngine;

namespace GameFolders.Scripts.Helpers
{
    public sealed class ObjectPool : MonoBehaviour
    {
        public enum StartupPoolMode
        {
            Awake,
            Start,
            CallManually
        };

        [System.Serializable]
        public class StartupPool
        {
            public int size;
            public GameObject prefab;
        }

        static ObjectPool _instance;
        static readonly List<GameObject> TempList = new List<GameObject>();

        readonly Dictionary<GameObject, List<GameObject>> _pooledObjects =
            new Dictionary<GameObject, List<GameObject>>();

        readonly Dictionary<GameObject, GameObject> _spawnedObjects = new Dictionary<GameObject, GameObject>();

        public StartupPoolMode startupPoolMode;
        public StartupPool[] startupPools;

        bool _startupPoolsCreated;

        void Awake()
        {
            _instance = this;
            if (startupPoolMode == StartupPoolMode.Awake) CreateStartupPools();
        }

        void Start()
        {
            if (startupPoolMode == StartupPoolMode.Start) CreateStartupPools();
        }

        public GameObject GetSpawnedObject(GameObject prefab)
        {
            if (!_spawnedObjects.TryGetValue(prefab, out GameObject spawnedObject)) return null;
            spawnedObject.SetActive(true);
            return spawnedObject;
        }

        public static void CreateStartupPools()
        {
            if (!Instance._startupPoolsCreated)
            {
                Instance._startupPoolsCreated = true;
                var pools = Instance.startupPools;
                if (pools != null && pools.Length > 0)
                    for (int i = 0; i < pools.Length; ++i)
                        CreatePool(pools[i].prefab, pools[i].size);
            }
        }

        public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
        {
            CreatePool(prefab.gameObject, initialPoolSize);
        }

        public static void CreatePool(GameObject prefab, int initialPoolSize)
        {
            if (prefab != null && !Instance._pooledObjects.ContainsKey(prefab))
            {
                var list = new List<GameObject>();
                Instance._pooledObjects.Add(prefab, list);

                if (initialPoolSize > 0)
                {
                    bool active = prefab.activeSelf;
                    prefab.SetActive(false);
                    Transform parent = Instance.transform;
                    while (list.Count < initialPoolSize)
                    {
                        GameObject obj = Instantiate(prefab, parent, true);
                        list.Add(obj);
                    }

                    prefab.SetActive(active);
                }
            }
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Vector3 position) where T : Component
        {
            return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        public static T Spawn<T>(T prefab) where T : Component
        {
            return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            List<GameObject> list;
            Transform trans;
            GameObject obj;

            CreatePool(prefab, 1);

            obj = null;
            list = Instance._pooledObjects[prefab];

            if (list.Count > 0)
            {
                while (Equals(obj, null) && list.Count > 0)
                {
                    obj = list[0];
                    list.RemoveAt(0);
                }

                if (!Equals(obj, null))
                {
                    trans = obj.transform;
                    trans.SetParent(parent);
                    trans.localPosition = position;
                    trans.localRotation = rotation;
                    obj.SetActive(true);
                    Instance._spawnedObjects.Add(obj, prefab);
                    return obj;
                }
            }

            obj = Instantiate(prefab, parent);
            trans = obj.transform;
            trans.SetParent(parent);
            trans.localPosition = position;
            trans.localRotation = rotation;
            Instance._spawnedObjects.Add(obj, prefab);
            return obj;
        }

        public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
        {
            return Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return Spawn(prefab, null, position, rotation);
        }

        public static GameObject Spawn(GameObject prefab, Transform parent)
        {
            return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab, Vector3 position)
        {
            return Spawn(prefab, null, position, Quaternion.identity);
        }

        public static GameObject Spawn(GameObject prefab)
        {
            return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(T obj) where T : Component
        {
            Recycle(obj.gameObject);
        }

        public static void Recycle(GameObject obj)
        {
            if (!Instance) return;
            GameObject prefab;
            if (Instance._spawnedObjects.TryGetValue(obj, out prefab)) Recycle(obj, prefab);
        }

        static void Recycle(GameObject obj, GameObject prefab)
        {
            if(obj == null) return;
            Instance._pooledObjects[prefab].Add(obj);
            Instance._spawnedObjects.Remove(obj);
            obj.transform.SetParent(Instance.transform);
            obj.SetActive(false);
        }

        public static void RecycleAll<T>(T prefab) where T : Component
        {
            RecycleAll(prefab.gameObject);
        }

        public static void RecycleAll(GameObject prefab)
        {
            foreach (var item in Instance._spawnedObjects)
                if (item.Value == prefab)
                    TempList.Add(item.Key);
            for (int i = 0; i < TempList.Count; ++i) Recycle(TempList[i]);
            TempList.Clear();
        }

        public static void RecycleAll()
        {
            TempList.AddRange(Instance._spawnedObjects.Keys);
            for (int i = 0; i < TempList.Count; ++i) Recycle(TempList[i]);
            TempList.Clear();
        }

        public static bool IsSpawned(GameObject obj)
        {
            return Instance._spawnedObjects.ContainsKey(obj);
        }

        public static int CountPooled<T>(T prefab) where T : Component
        {
            return CountPooled(prefab.gameObject);
        }

        public static int CountPooled(GameObject prefab)
        {
            List<GameObject> list;
            if (Instance._pooledObjects.TryGetValue(prefab, out list)) return list.Count;
            return 0;
        }

        public static int CountSpawned<T>(T prefab) where T : Component
        {
            return CountSpawned(prefab.gameObject);
        }

        public static int CountSpawned(GameObject prefab)
        {
            int count = 0;
            foreach (var instancePrefab in Instance._spawnedObjects.Values)
                if (prefab == instancePrefab)
                    ++count;
            return count;
        }

        public static int CountAllPooled()
        {
            int count = 0;
            foreach (var list in Instance._pooledObjects.Values) count += list.Count;
            return count;
        }

        public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null) list = new List<GameObject>();
            if (!appendList) list.Clear();
            List<GameObject> pooled;
            if (Instance._pooledObjects.TryGetValue(prefab, out pooled)) list.AddRange(pooled);
            return list;
        }

        public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null) list = new List<T>();
            if (!appendList) list.Clear();
            List<GameObject> pooled;
            if (Instance._pooledObjects.TryGetValue(prefab.gameObject, out pooled))
                for (int i = 0; i < pooled.Count; ++i)
                    list.Add(pooled[i].GetComponent<T>());
            return list;
        }

        public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
        {
            if (list == null) list = new List<GameObject>();
            if (!appendList) list.Clear();
            foreach (var item in Instance._spawnedObjects)
                if (item.Value == prefab)
                    list.Add(item.Key);
            return list;
        }

        public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
        {
            if (list == null) list = new List<T>();
            if (!appendList) list.Clear();
            var prefabObj = prefab.gameObject;
            foreach (var item in Instance._spawnedObjects)
                if (item.Value == prefabObj)
                    list.Add(item.Key.GetComponent<T>());
            return list;
        }

        public static void DestroyPooled(GameObject prefab)
        {
            List<GameObject> pooled;
            if (Instance._pooledObjects.TryGetValue(prefab, out pooled))
            {
                for (int i = 0; i < pooled.Count; ++i) GameObject.Destroy(pooled[i]);
                pooled.Clear();
            }
        }

        public static void DestroyAllPooledObjects()
        {
            if (Instance._pooledObjects.Count == 0) return;
            foreach (var itemPrefab in Instance._pooledObjects.Keys)
            {
                DestroyPooled(itemPrefab);
            }
        }

        public static void DestroyAllSpawnedObjects()
        {
            if(Instance._spawnedObjects.Count == 0) return;
            foreach (var itemPrefab in Instance._spawnedObjects.Keys)
            {
                DestroyAll(itemPrefab);
            }
        }

        public static void DestroyPooled<T>(T prefab) where T : Component
        {
            DestroyPooled(prefab.gameObject);
        }

        public static void DestroyAll(GameObject prefab)
        {
            RecycleAll(prefab);
            DestroyPooled(prefab);
        }

        public static void DestroyAll<T>(T prefab) where T : Component
        {
            DestroyAll(prefab.gameObject);
        }

        public static ObjectPool Instance => _instance ? _instance : _instance = FindObjectOfType<ObjectPool>();
    }

    public static class ObjectPoolExtensions
    {
        public static void CreatePool<T>(this T prefab) where T : Component
        {
            ObjectPool.CreatePool(prefab, 0);
        }

        public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
        {
            ObjectPool.CreatePool(prefab, initialPoolSize);
        }

        public static void CreatePool(this GameObject prefab)
        {
            ObjectPool.CreatePool(prefab, 0);
        }

        public static void CreatePool(this GameObject prefab, int initialPoolSize)
        {
            ObjectPool.CreatePool(prefab, initialPoolSize);
        }

        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation)
            where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, position, rotation);
        }

        public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, position, rotation);
        }

        public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab, Transform parent) where T : Component
        {
            return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static T Spawn<T>(this T prefab) where T : Component
        {
            return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            return ObjectPool.Spawn(prefab, parent, position, rotation);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return ObjectPool.Spawn(prefab, null, position, rotation);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
        {
            return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Vector3 position)
        {
            return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab, Transform parent)
        {
            return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public static GameObject Spawn(this GameObject prefab)
        {
            return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public static void Recycle<T>(this T obj) where T : Component
        {
            ObjectPool.Recycle(obj);
        }

        public static void Recycle(this GameObject obj)
        {
            ObjectPool.Recycle(obj);
        }

        public static void RecycleAll<T>(this T prefab) where T : Component
        {
            ObjectPool.RecycleAll(prefab);
        }

        public static void RecycleAll(this GameObject prefab)
        {
            ObjectPool.RecycleAll(prefab);
        }

        public static int CountPooled<T>(this T prefab) where T : Component
        {
            return ObjectPool.CountPooled(prefab);
        }

        public static int CountPooled(this GameObject prefab)
        {
            return ObjectPool.CountPooled(prefab);
        }

        public static int CountSpawned<T>(this T prefab) where T : Component
        {
            return ObjectPool.CountSpawned(prefab);
        }

        public static int CountSpawned(this GameObject prefab)
        {
            return ObjectPool.CountSpawned(prefab);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return ObjectPool.GetSpawned(prefab, list, appendList);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
        {
            return ObjectPool.GetSpawned(prefab, list, false);
        }

        public static List<GameObject> GetSpawned(this GameObject prefab)
        {
            return ObjectPool.GetSpawned(prefab, null, false);
        }

        public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, list, appendList);
        }

        public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, list, false);
        }

        public static List<T> GetSpawned<T>(this T prefab) where T : Component
        {
            return ObjectPool.GetSpawned(prefab, null, false);
        }

        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
        {
            return ObjectPool.GetPooled(prefab, list, appendList);
        }

        public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
        {
            return ObjectPool.GetPooled(prefab, list, false);
        }

        public static List<GameObject> GetPooled(this GameObject prefab)
        {
            return ObjectPool.GetPooled(prefab, null, false);
        }

        public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
        {
            return ObjectPool.GetPooled(prefab, list, appendList);
        }

        public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
        {
            return ObjectPool.GetPooled(prefab, list, false);
        }

        public static List<T> GetPooled<T>(this T prefab) where T : Component
        {
            return ObjectPool.GetPooled(prefab, null, false);
        }

        public static void DestroyPooled(this GameObject prefab)
        {
            ObjectPool.DestroyPooled(prefab);
        }

        public static void DestroyPooled<T>(this T prefab) where T : Component
        {
            ObjectPool.DestroyPooled(prefab.gameObject);
        }

        public static void DestroyAll(this GameObject prefab)
        {
            ObjectPool.DestroyAll(prefab);
        }

        public static void DestroyAll<T>(this T prefab) where T : Component
        {
            ObjectPool.DestroyAll(prefab.gameObject);
        }
    }
}