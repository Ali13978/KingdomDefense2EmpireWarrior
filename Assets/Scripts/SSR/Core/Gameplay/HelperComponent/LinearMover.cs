using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class LinearMover : OutsiteTargetTransform
	{
		[SerializeField]
		private Vector3 velocity;

		public Vector3 Velocity
		{
			get
			{
				return velocity;
			}
			set
			{
				velocity = value;
			}
		}

		private void Update()
		{
			base.TargetPosition += velocity * Time.deltaTime;
		}
	}
}
