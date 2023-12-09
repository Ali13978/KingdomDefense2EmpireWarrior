using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(ParticleSystem))]
	public abstract class ParticleManipulator : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private ParticleSystem targetParticleSystem;

		[SerializeField]
		private ParticleSystem.Particle[] particles;

		protected ParticleSystem TargetParticleSystem => targetParticleSystem;

		public void Update()
		{
			ReinitializeIfNeeded();
			InitializeNewUpdate();
			int num = targetParticleSystem.GetParticles(particles);
			for (int i = 0; i < num; i++)
			{
				particles[i] = ManipulateParticle(particles[i]);
			}
			targetParticleSystem.SetParticles(particles, num);
		}

		protected virtual void InitializeNewUpdate()
		{
		}

		protected abstract ParticleSystem.Particle ManipulateParticle(ParticleSystem.Particle particle);

		private void ReinitializeIfNeeded()
		{
			if (particles == null || particles.Length < targetParticleSystem.maxParticles)
			{
				particles = new ParticleSystem.Particle[targetParticleSystem.maxParticles];
			}
		}

		public void OnValidate()
		{
			particles = new ParticleSystem.Particle[targetParticleSystem.maxParticles];
		}

		public void Reset()
		{
			targetParticleSystem = GetComponent<ParticleSystem>();
			targetParticleSystem.maxParticles = 100;
		}
	}
}
