using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class TrashManRecycleBin
{
	public GameObject prefab;

	public int instancesToPreallocate = 5;

	public int instancesToAllocateIfEmpty = 1;

	public bool imposeHardLimit;

	public int hardLimit = 5;

	public bool cullExcessPrefabs;

	public int instancesToMaintainInPool = 5;

	public float cullInterval = 10f;

	public bool automaticallyRecycleParticleSystems;

	public bool persistBetweenScenes;

	private Stack<GameObject> _gameObjectPool;

	private float _timeOfLastCull = float.MinValue;

	private int _spawnedInstanceCount;

	public event Action<GameObject> onSpawnedEvent;

	public event Action<GameObject> onDespawnedEvent;

	private void allocateGameObjects(int count)
	{
		if (imposeHardLimit && _gameObjectPool.Count + count > hardLimit)
		{
			count = hardLimit - _gameObjectPool.Count;
		}
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(prefab.gameObject);
			gameObject.name = prefab.name;
			if ((bool)(gameObject.transform as RectTransform))
			{
				gameObject.transform.SetParent(TrashMan.instance.transform, worldPositionStays: false);
			}
			else
			{
				gameObject.transform.parent = TrashMan.instance.transform;
			}
			gameObject.SetActive(value: false);
			_gameObjectPool.Push(gameObject);
		}
	}

	private GameObject pop()
	{
		if (imposeHardLimit && _spawnedInstanceCount >= hardLimit)
		{
			return null;
		}
		if (_gameObjectPool.Count > 0)
		{
			_spawnedInstanceCount++;
			return _gameObjectPool.Pop();
		}
		allocateGameObjects(instancesToAllocateIfEmpty);
		return pop();
	}

	public void initialize()
	{
		_gameObjectPool = new Stack<GameObject>(instancesToPreallocate);
		allocateGameObjects(instancesToPreallocate);
	}

	public void cullExcessObjects()
	{
		if (cullExcessPrefabs && _gameObjectPool.Count > instancesToMaintainInPool && Time.time > _timeOfLastCull + cullInterval)
		{
			_timeOfLastCull = Time.time;
			for (int i = instancesToMaintainInPool; i <= _gameObjectPool.Count; i++)
			{
				UnityEngine.Object.Destroy(_gameObjectPool.Pop());
			}
		}
	}

	public GameObject spawn()
	{
		GameObject gameObject = pop();
		if (gameObject != null)
		{
			if (this.onSpawnedEvent != null)
			{
				this.onSpawnedEvent(gameObject);
			}
			if (automaticallyRecycleParticleSystems)
			{
				ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
				if ((bool)component)
				{
					TrashMan.despawnAfterDelay(gameObject, component.duration + component.startLifetime);
				}
				else
				{
					UnityEngine.Debug.LogError("automaticallyRecycleParticleSystems is true but there is no ParticleSystem on this GameObject!");
				}
			}
		}
		return gameObject;
	}

	public void despawn(GameObject go)
	{
		go.SetActive(value: false);
		_spawnedInstanceCount--;
		_gameObjectPool.Push(go);
		if (this.onDespawnedEvent != null)
		{
			this.onDespawnedEvent(go);
		}
	}

	public void clearBin(bool shouldDestroyAllManagedObjects)
	{
		while (_gameObjectPool.Count > 0)
		{
			GameObject obj = _gameObjectPool.Pop();
			if (shouldDestroyAllManagedObjects)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}
}
