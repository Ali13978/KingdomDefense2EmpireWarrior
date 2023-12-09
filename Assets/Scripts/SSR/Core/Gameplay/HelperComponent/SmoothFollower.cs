using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class SmoothFollower : Follower
	{
		[Header("SmoothFollower")]
		[SerializeField]
		private Vector3 smoothFactor = Vector3.one * 5f;

		protected override void OnUpdatePositionFollow()
		{
			Vector3 vector = base.Destination.Position + base.Offset;
			Vector3 targetPosition = base.TargetPosition;
			Vector3 vector2 = Time.deltaTime * smoothFactor;
			if (base.FollowX)
			{
				if (vector2.x == 0f)
				{
					targetPosition.x = vector.x;
				}
				else
				{
					targetPosition.x = Mathf.Lerp(targetPosition.x, vector.x, vector2.x);
				}
			}
			if (base.FollowY)
			{
				if (vector2.y == 0f)
				{
					targetPosition.y = vector.y;
				}
				else
				{
					targetPosition.y = Mathf.Lerp(targetPosition.y, vector.y, vector2.y);
				}
			}
			if (base.FollowZ)
			{
				if (vector2.z == 0f)
				{
					vector2.z = vector.z;
				}
				else
				{
					targetPosition.z = Mathf.Lerp(targetPosition.z, vector.z, vector2.z);
				}
			}
			base.TargetPosition = targetPosition;
		}
	}
}
