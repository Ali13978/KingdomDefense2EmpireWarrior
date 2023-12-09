using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ObjectPool : MonoBehaviour
{
	public enum StartupPoolMode
	{
		Awake,
		Start,
		CallManually
	}

	[Serializable]
	public class StartupPool
	{
		public int size;

		public GameObject prefab;
	}

	private static ObjectPool _instance;

	private static List<GameObject> tempList = new List<GameObject>();

	private Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();

	private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

	public StartupPoolMode startupPoolMode;

	public StartupPool[] startupPools;

	private bool startupPoolsCreated;

	public static ObjectPool instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}
			_instance = UnityEngine.Object.FindObjectOfType<ObjectPool>();
			if (_instance != null)
			{
				return _instance;
			}
			GameObject gameObject = new GameObject("ObjectPool");
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			_instance = gameObject.AddComponent<ObjectPool>();
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
		if (startupPoolMode == StartupPoolMode.Awake)
		{
			CreateStartupPools();
		}
	}

	private void Start()
	{
		if (startupPoolMode == StartupPoolMode.Start)
		{
			CreateStartupPools();
		}
	}

	public static void CreateStartupPools()
	{
		if (instance.startupPoolsCreated)
		{
			return;
		}
		instance.startupPoolsCreated = true;
		StartupPool[] array = instance.startupPools;
		if (array != null && array.Length > 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				CreatePool(array[i].prefab, array[i].size);
			}
		}
	}

	public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
	{
		CreatePool(prefab.gameObject, initialPoolSize);
	}

	public static void CreatePool(GameObject prefab, int initialPoolSize)
	{
		if (!(prefab != null) || instance.pooledObjects.ContainsKey(prefab))
		{
			return;
		}
		List<GameObject> list = new List<GameObject>();
		instance.pooledObjects.Add(prefab, list);
		if (initialPoolSize > 0)
		{
			bool activeSelf = prefab.activeSelf;
			prefab.SetActive(value: false);
			Transform transform = instance.transform;
			while (list.Count < initialPoolSize)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
				gameObject.transform.parent = transform;
				list.Add(gameObject);
			}
			prefab.SetActive(activeSelf);
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
		if (prefab == null)
		{
			return null;
		}
		if (instance.pooledObjects.TryGetValue(prefab, out List<GameObject> value))
		{
			GameObject gameObject = null;
			Transform transform;
			if (value.Count > 0)
			{
				while (gameObject == null && value.Count > 0)
				{
					gameObject = value[0];
					value.RemoveAt(0);
				}
				if (gameObject != null)
				{
					transform = gameObject.transform;
					transform.parent = parent;
					transform.localPosition = position;
					transform.localRotation = rotation;
					transform.localScale = prefab.transform.localScale;
					gameObject.SetActive(value: true);
					instance.spawnedObjects.Add(gameObject, prefab);
					return gameObject;
				}
			}
			gameObject = UnityEngine.Object.Instantiate(prefab);
			transform = gameObject.transform;
			transform.parent = parent;
			transform.localPosition = position;
			transform.localRotation = rotation;
			transform.localScale = prefab.transform.localScale;
			instance.spawnedObjects.Add(gameObject, prefab);
			return gameObject;
		}
		CreatePool(prefab, 1);
		return Spawn(prefab, parent, position, rotation);
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
		if (instance.spawnedObjects.TryGetValue(obj, out GameObject value))
		{
			Recycle(obj, value);
		}
		else
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	private static void Recycle(GameObject obj, GameObject prefab)
	{
		instance.pooledObjects[prefab].Add(obj);
		instance.spawnedObjects.Remove(obj);
		obj.transform.parent = instance.transform;
		obj.SetActive(value: false);
	}

	public static void RecycleAll<T>(T prefab) where T : Component
	{
		RecycleAll(prefab.gameObject);
	}

	public static void RecycleAll(GameObject prefab)
	{
		foreach (KeyValuePair<GameObject, GameObject> spawnedObject in instance.spawnedObjects)
		{
			if (spawnedObject.Value == prefab)
			{
				tempList.Add(spawnedObject.Key);
			}
		}
		for (int i = 0; i < tempList.Count; i++)
		{
			Recycle(tempList[i]);
		}
		tempList.Clear();
	}

	public static void RecycleAll()
	{
		tempList.AddRange(instance.spawnedObjects.Keys);
		for (int i = 0; i < tempList.Count; i++)
		{
			Recycle(tempList[i]);
		}
		tempList.Clear();
	}

	public static bool IsSpawned(GameObject obj)
	{
		return instance.spawnedObjects.ContainsKey(obj);
	}

	public static int CountPooled<T>(T prefab) where T : Component
	{
		return CountPooled(prefab.gameObject);
	}

	public static int CountPooled(GameObject prefab)
	{
		if (instance.pooledObjects.TryGetValue(prefab, out List<GameObject> value))
		{
			return value.Count;
		}
		return 0;
	}

	public static int CountSpawned<T>(T prefab) where T : Component
	{
		return CountSpawned(prefab.gameObject);
	}

	public static int CountSpawned(GameObject prefab)
	{
		int num = 0;
		foreach (GameObject value in instance.spawnedObjects.Values)
		{
			if (prefab == value)
			{
				num++;
			}
		}
		return num;
	}

	public static int CountAllPooled()
	{
		int num = 0;
		foreach (List<GameObject> value in instance.pooledObjects.Values)
		{
			num += value.Count;
		}
		return num;
	}

	public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		if (instance.pooledObjects.TryGetValue(prefab, out List<GameObject> value))
		{
			list.AddRange(value);
		}
		return list;
	}

	public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		if (instance.pooledObjects.TryGetValue(prefab.gameObject, out List<GameObject> value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				list.Add(value[i].GetComponent<T>());
			}
		}
		return list;
	}

	public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		foreach (KeyValuePair<GameObject, GameObject> spawnedObject in instance.spawnedObjects)
		{
			if (spawnedObject.Value == prefab)
			{
				list.Add(spawnedObject.Key);
			}
		}
		return list;
	}

	public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		GameObject gameObject = prefab.gameObject;
		foreach (KeyValuePair<GameObject, GameObject> spawnedObject in instance.spawnedObjects)
		{
			if (spawnedObject.Value == gameObject)
			{
				list.Add(spawnedObject.Key.GetComponent<T>());
			}
		}
		return list;
	}

	public static void DestroyPooled(GameObject prefab)
	{
		if (instance.pooledObjects.TryGetValue(prefab, out List<GameObject> value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				UnityEngine.Object.Destroy(value[i]);
			}
			value.Clear();
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
}
