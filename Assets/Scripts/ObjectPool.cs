using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;
    }

    public static ObjectPool Instance { get; private set; }

    [SerializeField] private List<Pool> _pools = new List<Pool>();
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Debug.LogWarning("Multiple ObjectPool instances detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>(_pools.Count);

        foreach (var pool in _pools)
        {
            var objectPool = new Queue<GameObject>(pool.Size);

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.Tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.TryGetValue(tag, out Queue<GameObject> objectPool))
        {
            Debug.LogWarning($"Pool with tag '{tag}' doesn't exist.");
            return null;
        }

        if (objectPool.Count == 0)
        {
            Debug.LogWarning($"Pool with tag '{tag}' is empty. Consider increasing its size.");
            return null;
        }

        GameObject objectToSpawn = objectPool.Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        objectPool.Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
