using System.Collections.Generic;
using UnityEngine;

public class F3DPool : MonoBehaviour
{
	public static F3DPool instance;

	[Header("VFX Pool")]
	public Transform[] poolItems;

	public int[] poolLength;

	[Header("Audio Pool")]
	public Transform audioSourcePrefab;

	public AudioClip[] audioPoolItems;

	public int[] audioPoolLength;

	private Dictionary<Transform, Transform[]> pool;

	private Dictionary<AudioClip, AudioSource[]> audioPool;

	private void Start()
	{
		instance = this;
		if (poolItems.Length > 0)
		{
			pool = new Dictionary<Transform, Transform[]>();
			for (int i = 0; i < poolItems.Length; i++)
			{
				Transform[] array = new Transform[poolLength[i]];
				for (int j = 0; j < poolLength[i]; j++)
				{
					Transform transform = UnityEngine.Object.Instantiate(poolItems[i], Vector3.zero, Quaternion.identity);
					transform.gameObject.SetActive(value: false);
					transform.parent = base.transform;
					array[j] = transform;
				}
				pool.Add(poolItems[i], array);
			}
		}
		if (audioPoolItems.Length <= 0)
		{
			return;
		}
		audioPool = new Dictionary<AudioClip, AudioSource[]>();
		for (int k = 0; k < audioPoolItems.Length; k++)
		{
			AudioSource[] array2 = new AudioSource[audioPoolLength[k]];
			for (int l = 0; l < audioPoolLength[k]; l++)
			{
				AudioSource component = UnityEngine.Object.Instantiate(audioSourcePrefab, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
				component.clip = audioPoolItems[k];
				component.gameObject.SetActive(value: false);
				component.transform.parent = base.transform;
				array2[l] = component;
			}
			audioPool.Add(audioPoolItems[k], array2);
		}
	}

	public Transform Spawn(Transform obj, Vector3 pos, Quaternion rot, Transform parent)
	{
		for (int i = 0; i < pool[obj].Length; i++)
		{
			if (!pool[obj][i].gameObject.activeSelf)
			{
				Transform transform = pool[obj][i];
				transform.parent = parent;
				transform.position = pos;
				transform.rotation = rot;
				transform.gameObject.SetActive(value: true);
				transform.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
				return transform;
			}
		}
		return null;
	}

	public AudioSource SpawnAudio(AudioClip clip, Vector3 pos, Transform parent)
	{
		for (int i = 0; i < audioPool[clip].Length; i++)
		{
			if (!audioPool[clip][i].gameObject.activeSelf)
			{
				AudioSource audioSource = audioPool[clip][i];
				audioSource.transform.parent = parent;
				audioSource.transform.position = pos;
				audioSource.gameObject.SetActive(value: true);
				audioSource.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
				return audioSource;
			}
		}
		return null;
	}

	public void Despawn(Transform obj)
	{
		obj.BroadcastMessage("OnDespawned", SendMessageOptions.DontRequireReceiver);
		obj.gameObject.SetActive(value: false);
	}
}
