using System.Collections;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public abstract class LightningSpellScript : MonoBehaviour
	{
		[Header("Direction and distance")]
		[Tooltip("The start point of the spell. Set this to a muzzle end or hand.")]
		public GameObject SpellStart;

		[Tooltip("The end point of the spell. Set this to an empty game object. This will change depending on things like collisions, randomness, etc. Not all spells need an end object, but create this anyway to be sure.")]
		public GameObject SpellEnd;

		[HideInInspector]
		[Tooltip("The direction of the spell. Should be normalized. Does not change unless explicitly modified.")]
		public Vector3 Direction;

		[Tooltip("The maximum distance of the spell")]
		public float MaxDistance = 15f;

		[Header("Collision")]
		[Tooltip("Whether the collision is an exploision. If not explosion, collision is directional.")]
		public bool CollisionIsExplosion;

		[Tooltip("The radius of the collision explosion")]
		public float CollisionRadius = 1f;

		[Tooltip("The force to explode with when there is a collision")]
		public float CollisionForce = 50f;

		[Tooltip("Collision force mode")]
		public ForceMode CollisionForceMode = ForceMode.Impulse;

		[Tooltip("The particle system for collisions. For best effects, this should emit particles in bursts at time 0 and not loop.")]
		public ParticleSystem CollisionParticleSystem;

		[Tooltip("The layers that the spell should collide with")]
		public LayerMask CollisionMask = -1;

		[Tooltip("Collision audio source")]
		public AudioSource CollisionAudioSource;

		[Tooltip("Collision audio clips. One will be chosen at random and played one shot with CollisionAudioSource.")]
		public AudioClip[] CollisionAudioClips;

		[Tooltip("Collision sound volume range.")]
		public RangeOfFloats CollisionVolumeRange = new RangeOfFloats
		{
			Minimum = 0.4f,
			Maximum = 0.6f
		};

		[Header("Duration and Cooldown")]
		[Tooltip("The duration in seconds that the spell will last. Not all spells support a duration. For one shot spells, this is how long the spell cast / emission light, etc. will last.")]
		public float Duration;

		[Tooltip("The cooldown in seconds. Once cast, the spell must wait for the cooldown before being cast again.")]
		public float Cooldown;

		[Header("Emission")]
		[Tooltip("Emission sound")]
		public AudioSource EmissionSound;

		[Tooltip("Emission particle system. For best results use world space, turn off looping and play on awake.")]
		public ParticleSystem EmissionParticleSystem;

		[Tooltip("Light to illuminate when spell is cast")]
		public Light EmissionLight;

		private int stopToken;

		protected float DurationTimer
		{
			get;
			private set;
		}

		protected float CooldownTimer
		{
			get;
			private set;
		}

		public bool Casting
		{
			get;
			private set;
		}

		public bool CanCastSpell => !Casting && CooldownTimer <= 0f;

		private IEnumerator StopAfterSecondsCoRoutine(float seconds)
		{
			int token = stopToken;
			yield return new WaitForSeconds(seconds);
			if (token == stopToken)
			{
				StopSpell();
			}
		}

		protected void ApplyCollisionForce(Vector3 point)
		{
			if (!(CollisionForce > 0f) || !(CollisionRadius > 0f))
			{
				return;
			}
			Collider[] array = Physics.OverlapSphere(point, CollisionRadius, CollisionMask);
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				Rigidbody component = collider.GetComponent<Rigidbody>();
				if (component != null)
				{
					if (CollisionIsExplosion)
					{
						component.AddExplosionForce(CollisionForce, point, CollisionRadius, CollisionForce * 0.02f, CollisionForceMode);
					}
					else
					{
						component.AddForce(CollisionForce * Direction, CollisionForceMode);
					}
				}
			}
		}

		protected void PlayCollisionSound(Vector3 pos)
		{
			if (CollisionAudioSource != null && CollisionAudioClips != null && CollisionAudioClips.Length != 0)
			{
				int num = UnityEngine.Random.Range(0, CollisionAudioClips.Length - 1);
				float volumeScale = UnityEngine.Random.Range(CollisionVolumeRange.Minimum, CollisionVolumeRange.Maximum);
				CollisionAudioSource.transform.position = pos;
				CollisionAudioSource.PlayOneShot(CollisionAudioClips[num], volumeScale);
			}
		}

		protected virtual void Start()
		{
			if (EmissionLight != null)
			{
				EmissionLight.enabled = false;
			}
		}

		protected virtual void Update()
		{
			CooldownTimer = Mathf.Max(0f, CooldownTimer - Time.deltaTime);
			DurationTimer = Mathf.Max(0f, DurationTimer - Time.deltaTime);
		}

		protected virtual void LateUpdate()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected abstract void OnCastSpell();

		protected abstract void OnStopSpell();

		protected virtual void OnActivated()
		{
		}

		protected virtual void OnDeactivated()
		{
		}

		public bool CastSpell()
		{
			if (!CanCastSpell)
			{
				return false;
			}
			Casting = true;
			DurationTimer = Duration;
			CooldownTimer = Cooldown;
			OnCastSpell();
			if (Duration > 0f)
			{
				StopAfterSeconds(Duration);
			}
			if (EmissionParticleSystem != null)
			{
				EmissionParticleSystem.Play();
			}
			if (EmissionLight != null)
			{
				EmissionLight.transform.position = SpellStart.transform.position;
				EmissionLight.enabled = true;
			}
			if (EmissionSound != null)
			{
				EmissionSound.Play();
			}
			return true;
		}

		public void StopSpell()
		{
			if (Casting)
			{
				stopToken++;
				if (EmissionParticleSystem != null)
				{
					EmissionParticleSystem.Stop();
				}
				if (EmissionLight != null)
				{
					EmissionLight.enabled = false;
				}
				if (EmissionSound != null && EmissionSound.loop)
				{
					EmissionSound.Stop();
				}
				DurationTimer = 0f;
				Casting = false;
				OnStopSpell();
			}
		}

		public void ActivateSpell()
		{
			OnActivated();
		}

		public void DeactivateSpell()
		{
			OnDeactivated();
		}

		public void StopAfterSeconds(float seconds)
		{
			StartCoroutine(StopAfterSecondsCoRoutine(seconds));
		}

		public static GameObject FindChildRecursively(Transform t, string name)
		{
			if (t.name == name)
			{
				return t.gameObject;
			}
			for (int i = 0; i < t.childCount; i++)
			{
				GameObject gameObject = FindChildRecursively(t.GetChild(i), name);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
			return null;
		}
	}
}
