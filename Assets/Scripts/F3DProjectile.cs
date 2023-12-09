using UnityEngine;

public class F3DProjectile : MonoBehaviour
{
	public F3DFXType fxType;

	public LayerMask layerMask;

	public float lifeTime = 5f;

	public float despawnDelay;

	public float velocity = 300f;

	public float RaycastAdvance = 2f;

	public bool DelayDespawn;

	public ParticleSystem[] delayedParticles;

	private ParticleSystem[] particles;

	private new Transform transform;

	private RaycastHit hitPoint;

	private bool isHit;

	private bool isFXSpawned;

	private float timer;

	private void Awake()
	{
		transform = GetComponent<Transform>();
		particles = GetComponentsInChildren<ParticleSystem>();
	}

	public void OnSpawned()
	{
		isHit = false;
		isFXSpawned = false;
		timer = 0f;
		hitPoint = default(RaycastHit);
	}

	public void OnDespawned()
	{
	}

	private void Delay()
	{
		if (particles.Length <= 0 || delayedParticles.Length <= 0)
		{
			return;
		}
		for (int i = 0; i < particles.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < delayedParticles.Length; j++)
			{
				if (particles[i] == delayedParticles[j])
				{
					flag = true;
					break;
				}
			}
			particles[i].Stop(withChildren: false);
			if (!flag)
			{
				particles[i].Clear(withChildren: false);
			}
		}
	}

	private void OnProjectileDestroy()
	{
		F3DPool.instance.Despawn(transform);
	}

	private void ApplyForce(float force)
	{
		if (hitPoint.rigidbody != null)
		{
			hitPoint.rigidbody.AddForceAtPosition(transform.forward * force, hitPoint.point, ForceMode.VelocityChange);
		}
	}

	private void Update()
	{
		if (isHit)
		{
			if (!isFXSpawned)
			{
				switch (fxType)
				{
				case F3DFXType.Vulcan:
					F3DFXController.instance.VulcanImpact(hitPoint.point + hitPoint.normal * 0.2f);
					ApplyForce(2.5f);
					break;
				case F3DFXType.SoloGun:
					F3DFXController.instance.SoloGunImpact(hitPoint.point + hitPoint.normal * 0.2f);
					ApplyForce(25f);
					break;
				case F3DFXType.Seeker:
					F3DFXController.instance.SeekerImpact(hitPoint.point + hitPoint.normal * 1f);
					ApplyForce(30f);
					break;
				case F3DFXType.PlasmaGun:
					F3DFXController.instance.PlasmaGunImpact(hitPoint.point + hitPoint.normal * 0.2f);
					ApplyForce(25f);
					break;
				case F3DFXType.LaserImpulse:
					F3DFXController.instance.LaserImpulseImpact(hitPoint.point + hitPoint.normal * 0.2f);
					ApplyForce(25f);
					break;
				}
				isFXSpawned = true;
			}
			if (!DelayDespawn || (DelayDespawn && timer >= despawnDelay))
			{
				OnProjectileDestroy();
			}
		}
		else
		{
			Vector3 vector = transform.forward * Time.deltaTime * velocity;
			if (Physics.Raycast(transform.position, transform.forward, out hitPoint, vector.magnitude * RaycastAdvance, layerMask))
			{
				isHit = true;
				if (DelayDespawn)
				{
					timer = 0f;
					Delay();
				}
			}
			else if (timer >= lifeTime)
			{
				OnProjectileDestroy();
			}
			transform.position += vector;
		}
		timer += Time.deltaTime;
	}
}
