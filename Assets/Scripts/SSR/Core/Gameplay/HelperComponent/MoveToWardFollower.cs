using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class MoveToWardFollower : Follower
	{
		[SerializeField]
		private float speed = 5f;

		public void Update()
		{
			base.TargetPosition = Vector3.MoveTowards(base.TargetPosition, base.Destination.Position, speed * Time.deltaTime);
		}
	}
}
