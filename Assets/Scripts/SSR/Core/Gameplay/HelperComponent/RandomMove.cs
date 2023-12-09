using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class RandomMove : OutsiteTargetTransform
	{
		[SerializeField]
		private Vector3 minPosition;

		[SerializeField]
		private Vector3 maxPosition;

		[SerializeField]
		private float speed;

		[SerializeField]
		private bool deltaMoving = true;

		private Vector2 stablePosition;

		private Vector2 currentDestination;

		public void OnEnable()
		{
			GetStablePosition();
			currentDestination = GetRandomPosition();
		}

		private void GetStablePosition()
		{
			if (deltaMoving)
			{
				stablePosition = base.TargetTransform.position;
			}
		}

		private Vector2 GetRandomPosition()
		{
			if (deltaMoving)
			{
				return new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y)) + stablePosition;
			}
			return new Vector2(Random.Range(minPosition.x, maxPosition.x), Random.Range(minPosition.y, maxPosition.y));
		}

		public void Update()
		{
			Vector2 vector = base.TargetTransform.position;
			if (currentDestination == vector)
			{
				currentDestination = GetRandomPosition();
			}
			base.TargetTransform.position = Vector2.MoveTowards(vector, currentDestination, speed * Time.deltaTime);
		}
	}
}
