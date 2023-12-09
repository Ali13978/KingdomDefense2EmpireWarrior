using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class SemiHardFollower : Follower
	{
		[SerializeField]
		private float reachDestinationTime = 1f;

		[SerializeField]
		private OrderedEventDispatcher onReachTarget = new OrderedEventDispatcher();

		private Vector3 currentOffset;

		private float offsetChangingStep;

		private bool following;

		private bool inFirstStage;

		private void OnEnable()
		{
			if (!following)
			{
				base.enabled = false;
			}
		}

		[ContextMenu("Start following")]
		public void StartFollow()
		{
			currentOffset = base.TargetPosition - base.Destination.Position;
			offsetChangingStep = (base.Offset - currentOffset).magnitude / reachDestinationTime;
			following = true;
			inFirstStage = true;
			base.enabled = true;
		}

		protected override void OnUpdatePositionFollow()
		{
			UpdateTargetPosition();
			if (inFirstStage)
			{
				UpdateCurrentOffset();
			}
		}

		private void UpdateCurrentOffset()
		{
			currentOffset = Vector3.MoveTowards(currentOffset, base.Offset, offsetChangingStep * Time.deltaTime);
			if (currentOffset == base.Offset)
			{
				inFirstStage = false;
				onReachTarget.Dispatch();
			}
		}

		private void UpdateTargetPosition()
		{
			Vector3 targetPosition = base.TargetPosition;
			Vector3 vector = base.Destination.Position + currentOffset;
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
