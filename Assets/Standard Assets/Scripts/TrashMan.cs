using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashMan : MonoBehaviour
{
	public static TrashMan instance;

	[HideInInspector]
	public List<TrashManRecycleBin> recycleBinCollection;

	public float cullExcessObjectsInterval = 10f;

	public bool persistBetweenScenes;

	private Dictionary<int, TrashManRecycleBin> _instanceIdToRecycleBin = new Dictionary<int, TrashManRecycleBin>();

	private Dictionary<string, int> _poolNameToInstanceId = new Dictionary<string, int>();

	[HideInInspector]
	public new Transform transform;

	private void Awake()
	{
		if (instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			transform = base.gameObject.transform;
			instance = this;
			initializePrefabPools();
			if (persistBetweenScenes)
			{
				Object.DontDestroyOnLoad(base.gameObject);
			}
		}
		if (cullExcessObjectsInterval > 0f)
		{
			StartCoroutine(cullExcessObjects());
		}
		SceneManager.activeSceneChanged += activeSceneChanged;
	}

	private void activeSceneChanged(Scene oldScene, Scene newScene)
	{
		if (oldScene.name == null)
		{
			return;
		}
		for (int num = recycleBinCollection.Count - 1; num >= 0; num--)
		{
			if (!recycleBinCollection[num].persistBetweenScenes)
			{
				removeRecycleBin(recycleBinCollection[num]);
			}
		}
	}

	private void OnApplicationQuit()
	{
		instance = null;
	}

	private IEnumerator cullExcessObjects()
	{
		WaitForSeconds waiter = new WaitForSeconds(cullExcessObjectsInterval);
		while (true)
		{
			for (int i = 0; i < recycleBinCollection.Count; i++)
			{
				recycleBinCollection[i].cullExcessObjects();
			}
			yield return waiter;
		}
	}

	private void initializePrefabPools()
	{
		if (recycleBinCollection != null)
		{
			foreach (TrashManRecycleBin item in recycleBinCollection)
			{
				if (item != null && !(item.prefab == null))
				{
					item.initialize();
					_instanceIdToRecycleBin.Add(item.prefab.GetInstanceID(), item);
					_poolNameToInstanceId.Add(item.prefab.name, item.prefab.GetInstanceID());
				}
			}
		}
	}

	private static GameObject spawn(int gameObjectInstanceId, Vector3 position, Quaternion rotation)
	{
		if (instance._instanceIdToRecycleBin.ContainsKey(gameObjectInstanceId))
		{
			GameObject gameObject = instance._instanceIdToRecycleBin[gameObjectInstanceId].spawn();
			if (gameObject != null)
			{
				Transform transform = gameObject.transform;
				if ((bool)(transform as RectTransform))
				{
					transform.SetParent(null, worldPositionStays: false);
				}
				else
				{
					transform.parent = null;
				}
				transform.position = position;
				transform.rotation = rotation;
				gameObject.SetActive(value: true);
			}
			return gameObject;
		}
		return null;
	}

	private IEnumerator internalDespawnAfterDelay(GameObject go, float delayInSeconds)
	{
		yield return new WaitForSeconds(delayInSeconds);
		despawn(go);
	}

	public static void InitPool(string objectPath, int pInstancesToPreallocate = 0)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(objectPath)) as GameObject;
		gameObject.SetActive(value: false);
		TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
		trashManRecycleBin.prefab = gameObject;
		trashManRecycleBin.instancesToPreallocate = pInstancesToPreallocate;
		TrashManRecycleBin recycleBin = trashManRecycleBin;
		manageRecycleBin(recycleBin);
		despawn(gameObject);
	}

	public static void manageRecycleBin(TrashManRecycleBin recycleBin)
	{
		if (!instance._poolNameToInstanceId.ContainsKey(recycleBin.prefab.name))
		{
			instance.recycleBinCollection.Add(recycleBin);
			recycleBin.initialize();
			instance._instanceIdToRecycleBin.Add(recycleBin.prefab.GetInstanceID(), recycleBin);
			instance._poolNameToInstanceId.Add(recycleBin.prefab.name, recycleBin.prefab.GetInstanceID());
		}
	}

	public static void removeRecycleBin(TrashManRecycleBin recycleBin, bool shouldDestroyAllManagedObjects = true)
	{
		string name = recycleBin.prefab.name;
		if (instance._poolNameToInstanceId.ContainsKey(name))
		{
			instance._poolNameToInstanceId.Remove(name);
			instance._instanceIdToRecycleBin.Remove(recycleBin.prefab.GetInstanceID());
			instance.recycleBinCollection.Remove(recycleBin);
			recycleBin.clearBin(shouldDestroyAllManagedObjects);
		}
	}

	public static GameObject spawn(GameObject go, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
	{
		if (instance._instanceIdToRecycleBin.ContainsKey(go.GetInstanceID()))
		{
			return spawn(go.GetInstanceID(), position, rotation);
		}
		UnityEngine.Debug.LogWarning("attempted to spawn go (" + go.name + ") but there is no recycle bin setup for it. Falling back to Instantiate");
		GameObject gameObject = UnityEngine.Object.Instantiate(go, position, rotation);
		if (gameObject.transform as RectTransform != null)
		{
			gameObject.transform.SetParent(null, worldPositionStays: false);
		}
		else
		{
			gameObject.transform.parent = null;
		}
		return gameObject;
	}

	public static GameObject spawn(string gameObjectName, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
	{
		int value = -1;
		if (instance._poolNameToInstanceId.TryGetValue(gameObjectName, out value))
		{
			return spawn(value, position, rotation);
		}
		UnityEngine.Debug.LogError("attempted to spawn a GameObject from recycle bin (" + gameObjectName + ") but there is no recycle bin setup for it");
		return null;
	}

	public static void despawn(GameObject go)
	{
		if (go == null)
		{
			return;
		}
		string name = go.name;
		if (!instance._poolNameToInstanceId.ContainsKey(name))
		{
			UnityEngine.Object.Destroy(go);
			return;
		}
		instance._instanceIdToRecycleBin[instance._poolNameToInstanceId[name]].despawn(go);
		if (go.transform as RectTransform != null)
		{
			go.transform.SetParent(instance.transform, worldPositionStays: false);
		}
		else
		{
			go.transform.parent = instance.transform;
		}
	}

	public static void despawnAfterDelay(GameObject go, float delayInSeconds)
	{
		if (!(go == null))
		{
			instance.StartCoroutine(instance.internalDespawnAfterDelay(go, delayInSeconds));
		}
	}

	public static TrashManRecycleBin recycleBinForGameObjectName(string gameObjectName)
	{
		if (instance._poolNameToInstanceId.ContainsKey(gameObjectName))
		{
			int key = instance._poolNameToInstanceId[gameObjectName];
			return instance._instanceIdToRecycleBin[key];
		}
		return null;
	}

	public static TrashManRecycleBin recycleBinForGameObject(GameObject go)
	{
		if (instance._instanceIdToRecycleBin.TryGetValue(go.GetInstanceID(), out TrashManRecycleBin value))
		{
			return value;
		}
		return null;
	}
}
