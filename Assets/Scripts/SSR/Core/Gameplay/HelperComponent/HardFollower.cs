using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class HardFollower : Follower
	{
		protected override void OnUpdatePositionFollow()
		{
			Vector3 targetPosition = base.TargetPosition;
			Vector3 vector = base.Destination.Position + base.Offset;
			if (base.FollowX)
			{
				targetPosition.x = vector.x;
			}
			if (base.FollowY)
			{
				targetPosition.y = vector.y;
			}
			if (base.FollowZ)
			{
				targetPosition.z = vector.z;
			}
			base.TargetPosition = targetPosition;
		}
	}
}
