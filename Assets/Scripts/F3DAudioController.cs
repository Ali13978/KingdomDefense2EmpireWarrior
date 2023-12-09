using UnityEngine;

public class F3DAudioController : MonoBehaviour
{
	public static F3DAudioController instance;

	private float timer_01;

	private float timer_02;

	[Header("Vulcan")]
	public AudioClip[] vulcanHit;

	public AudioClip vulcanShot;

	public float vulcanDelay;

	public float vulcanHitDelay;

	[Header("Solo gun")]
	public AudioClip[] soloGunHit;

	public AudioClip soloGunShot;

	public float soloGunDelay;

	public float soloGunHitDelay;

	[Header("Sniper")]
	public AudioClip[] sniperHit;

	public AudioClip sniperShot;

	public float sniperDelay;

	public float sniperHitDelay;

	[Header("Shot gun")]
	public AudioClip[] shotGunHit;

	public AudioClip shotGunShot;

	public float shotGunDelay;

	public float shotGunHitDelay;

	[Header("Seeker")]
	public AudioClip[] seekerHit;

	public AudioClip seekerShot;

	public float seekerDelay;

	public float seekerHitDelay;

	[Header("Rail gun")]
	public AudioClip[] railgunHit;

	public AudioClip railgunShot;

	public float railgunDelay;

	public float railgunHitDelay;

	[Header("Plasma gun")]
	public AudioClip[] plasmagunHit;

	public AudioClip plasmagunShot;

	public float plasmagunDelay;

	public float plasmagunHitDelay;

	[Header("Plasma beam")]
	public AudioClip plasmabeamOpen;

	public AudioClip plasmabeamLoop;

	public AudioClip plasmabeamClose;

	[Header("Plasma beam heavy")]
	public AudioClip plasmabeamHeavyOpen;

	public AudioClip plasmabeamHeavyLoop;

	public AudioClip plasmabeamHeavyClose;

	[Header("Lightning gun")]
	public AudioClip lightningGunOpen;

	public AudioClip lightningGunLoop;

	public AudioClip lightningGunClose;

	[Header("Flame gun")]
	public AudioClip flameGunOpen;

	public AudioClip flameGunLoop;

	public AudioClip flameGunClose;

	[Header("Laser impulse")]
	public AudioClip[] laserImpulseHit;

	public AudioClip laserImpulseShot;

	public float laserImpulseDelay;

	public float laserImpulseHitDelay;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		timer_01 += Time.deltaTime;
		timer_02 += Time.deltaTime;
	}

	public void VulcanShot(Vector3 pos)
	{
		if (timer_01 >= vulcanDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(vulcanShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 5f;
				audioSource.loop = false;
				audioSource.Play();
				timer_01 = 0f;
			}
		}
	}

	public void VulcanHit(Vector3 pos)
	{
		if (timer_02 >= vulcanHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(vulcanHit[Random.Range(0, vulcanHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.6f, 1f);
				audioSource.minDistance = 7f;
				audioSource.loop = false;
				audioSource.Play();
				timer_02 = 0f;
			}
		}
	}

	public void SoloGunShot(Vector3 pos)
	{
		if (timer_01 >= soloGunDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(soloGunShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 30f;
				audioSource.loop = false;
				audioSource.Play();
				timer_01 = 0f;
			}
		}
	}

	public void SoloGunHit(Vector3 pos)
	{
		if (timer_02 >= soloGunHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(soloGunHit[Random.Range(0, soloGunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.95f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 50f;
				audioSource.loop = false;
				audioSource.Play();
				timer_02 = 0f;
			}
		}
	}

	public void SniperShot(Vector3 pos)
	{
		if (timer_01 >= sniperDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(sniperShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 6f;
				audioSource.loop = false;
				audioSource.Play();
				timer_01 = 0f;
			}
		}
	}

	public void SniperHit(Vector3 pos)
	{
		if (timer_02 >= sniperHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(sniperHit[Random.Range(0, sniperHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 8f;
				audioSource.loop = false;
				audioSource.Play();
				timer_02 = 0f;
			}
		}
	}

	public void ShotGunShot(Vector3 pos)
	{
		if (timer_01 >= shotGunDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(shotGunShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 8f;
				audioSource.loop = false;
				audioSource.Play();
				timer_01 = 0f;
			}
		}
	}

	public void ShotGunHit(Vector3 pos)
	{
		if (timer_02 >= shotGunHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(shotGunHit[Random.Range(0, shotGunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 7f;
				audioSource.loop = false;
				audioSource.Play();
				timer_02 = 0f;
			}
		}
	}

	public void SeekerShot(Vector3 pos)
	{
		if (timer_01 >= seekerDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(seekerShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 8f;
				audioSource.loop = false;
				audioSource.Play();
				timer_01 = 0f;
			}
		}
	}

	public void SeekerHit(Vector3 pos)
	{
		if (timer_02 >= seekerHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(seekerHit[Random.Range(0, seekerHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 25f;
				audioSource.loop = false;
				audioSource.Play();
				timer_02 = 0f;
			}
		}
	}

	public void RailGunShot(Vector3 pos)
	{
		if (timer_01 >= railgunDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(railgunShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 4f;
				audioSource.loop = false;
				audioSource.Play();
				timer_01 = 0f;
			}
		}
	}

	public void RailGunHit(Vector3 pos)
	{
		if (timer_02 >= railgunHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(railgunHit[Random.Range(0, railgunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 20f;
				audioSource.loop = false;
				audioSource.Play();
				timer_02 = 0f;
			}
		}
	}

	public void PlasmaGunShot(Vector3 pos)
	{
		if (timer_01 >= plasmagunDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(plasmagunShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 4f;
				audioSource.loop = false;
				audioSource.Play();
				timer_01 = 0f;
			}
		}
	}

	public void PlasmaGunHit(Vector3 pos)
	{
		if (timer_02 >= plasmagunHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(plasmagunHit[Random.Range(0, plasmagunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 50f;
				audioSource.loop = false;
				audioSource.Play();
				timer_02 = 0f;
			}
		}
	}

	public void PlasmaBeamLoop(Vector3 pos, Transform loopParent)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(plasmabeamOpen, pos, null);
		AudioSource audioSource2 = F3DPool.instance.SpawnAudio(plasmabeamLoop, pos, loopParent);
		if (audioSource != null && audioSource2 != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
			audioSource2.pitch = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.volume = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.loop = true;
			audioSource2.minDistance = 50f;
			audioSource2.Play();
		}
	}

	public void PlasmaBeamClose(Vector3 pos)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(plasmabeamClose, pos, null);
		if (audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
		}
	}

	public void PlasmaBeamHeavyLoop(Vector3 pos, Transform loopParent)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(plasmabeamHeavyOpen, pos, null);
		AudioSource audioSource2 = F3DPool.instance.SpawnAudio(plasmabeamHeavyLoop, pos, loopParent);
		if (audioSource != null && audioSource2 != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
			audioSource2.pitch = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.volume = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.loop = true;
			audioSource2.minDistance = 50f;
			audioSource2.Play();
		}
	}

	public void PlasmaBeamHeavyClose(Vector3 pos)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(plasmabeamHeavyClose, pos, null);
		if (audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
		}
	}

	public void LightningGunLoop(Vector3 pos, Transform loopParent)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(lightningGunOpen, pos, null);
		AudioSource audioSource2 = F3DPool.instance.SpawnAudio(lightningGunLoop, pos, loopParent.parent);
		if (audioSource != null && audioSource2 != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
			audioSource2.pitch = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.volume = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.loop = true;
			audioSource2.minDistance = 50f;
			audioSource2.Play();
		}
	}

	public void LightningGunClose(Vector3 pos)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(lightningGunClose, pos, null);
		if (audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
		}
	}

	public void FlameGunLoop(Vector3 pos, Transform loopParent)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(flameGunOpen, pos, null);
		AudioSource audioSource2 = F3DPool.instance.SpawnAudio(flameGunLoop, pos, loopParent.parent);
		if (audioSource != null && audioSource2 != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
			audioSource2.pitch = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.volume = UnityEngine.Random.Range(0.95f, 1f);
			audioSource2.loop = true;
			audioSource2.minDistance = 50f;
			audioSource2.Play();
		}
	}

	public void FlameGunClose(Vector3 pos)
	{
		AudioSource audioSource = F3DPool.instance.SpawnAudio(flameGunClose, pos, null);
		if (audioSource != null)
		{
			audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
			audioSource.minDistance = 50f;
			audioSource.loop = false;
			audioSource.Play();
		}
	}

	public void LaserImpulseShot(Vector3 pos)
	{
		if (timer_01 >= laserImpulseDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(laserImpulseShot, pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.9f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 20f;
				audioSource.loop = false;
				audioSource.Play();
				timer_01 = 0f;
			}
		}
	}

	public void LaserImpulseHit(Vector3 pos)
	{
		if (timer_02 >= laserImpulseHitDelay)
		{
			AudioSource audioSource = F3DPool.instance.SpawnAudio(laserImpulseHit[Random.Range(0, plasmagunHit.Length)], pos, null);
			if (audioSource != null)
			{
				audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.volume = UnityEngine.Random.Range(0.8f, 1f);
				audioSource.minDistance = 20f;
				audioSource.loop = false;
				audioSource.Play();
				timer_02 = 0f;
			}
		}
	}
}
