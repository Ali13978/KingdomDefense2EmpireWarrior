using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class ParticleSystemStartSizeController : ParticleSystemManipulator
	{
		[SerializeField]
		private float minStartSize;

		[SerializeField]
		private float maxStartSize = 1f;

		[SerializeField]
		private float lifeTimeToReachMax = 0.4f;

		protected override void UpdateParticleSystem()
		{
			base.TargetParticleSystem.startSize = Mathf.Lerp(minStartSize, maxStartSize, base.TargetParticleSystem.CurrentProgressToSpecificLifeTime(lifeTimeToReachMax));
		}
	}
}
