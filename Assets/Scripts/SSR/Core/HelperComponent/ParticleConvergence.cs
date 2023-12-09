using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class ParticleConvergence : ParticleManipulator
	{
		[SerializeField]
		private PositionProvider convergencePoint;

		[SerializeField]
		private float speed;

		private Vector3 currentTargetPosition;

		protected override void InitializeNewUpdate()
		{
			currentTargetPosition = convergencePoint.Position;
		}

		protected override ParticleSystem.Particle ManipulateParticle(ParticleSystem.Particle particle)
		{
			particle.position = Vector3.MoveTowards(particle.position, currentTargetPosition, speed * Time.deltaTime);
			return particle;
		}
	}
}
