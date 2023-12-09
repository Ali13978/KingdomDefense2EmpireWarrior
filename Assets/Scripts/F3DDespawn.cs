using UnityEngine;

public class F3DDespawn : MonoBehaviour
{
	public float DespawnDelay;

	public bool DespawnOnMouseUp;

	private AudioSource aSrc;

	private void Awake()
	{
		aSrc = GetComponent<AudioSource>();
	}

	public void OnSpawned()
	{
		if (!DespawnOnMouseUp)
		{
			F3DTime.time.AddTimer(DespawnDelay, 1, DespawnOnTimer);
		}
	}

	public void OnDespawned()
	{
	}

	public void DespawnOnTimer()
	{
		if (aSrc != null)
		{
			if (aSrc.loop)
			{
				DespawnOnMouseUp = true;
				return;
			}
			DespawnOnMouseUp = false;
			Despawn();
		}
		else
		{
			Despawn();
		}
	}

	public void Despawn()
	{
		F3DPool.instance.Despawn(base.transform);
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0) && ((aSrc != null && aSrc.loop) || DespawnOnMouseUp))
		{
			Despawn();
		}
	}
}
