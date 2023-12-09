using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public static class ParticleExtension
	{
		public static float CurrentProgress(this ParticleSystem particleSystem)
		{
			return particleSystem.time / particleSystem.duration;
		}

		public static float CurrentProgressToSpecificLifeTime(this ParticleSystem particleSystem, float specificLifeTime)
		{
			return particleSystem.time / specificLifeTime;
		}

		public static float CurrentProgress(this ParticleSystem.Particle particle)
		{
			return 1f - particle.remainingLifetime / particle.startLifetime;
		}

		public static float CurrentProgressToSpecificLifeTime(this ParticleSystem.Particle particle, float specificLifeTime)
		{
			return (particle.startLifetime - particle.remainingLifetime) / (particle.startLifetime - specificLifeTime);
		}
	}
}
