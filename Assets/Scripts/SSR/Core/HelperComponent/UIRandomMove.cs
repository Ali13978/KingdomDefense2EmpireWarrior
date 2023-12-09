using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class UIRandomMove : OutsiteTargetRectTransform
	{
		[SerializeField]
		private Vector2 minPosition;

		[SerializeField]
		private Vector2 maxPosition;

		[SerializeField]
		private float speed;

		[SerializeField]
		private bool deltaMoving = true;

		private Vector2 stablePosition;

		private Vector2 currentDestination;

		public void Awake()
		{
			GetStablePosition();
			currentDestination = GetRandomPosition();
		}

		public void OnEnable()
		{
			GetStablePosition();
		}

		private void GetStablePosition()
		{
			if (deltaMoving)
			{
				stablePosition = base.TargetRectTransform.anchoredPosition;
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
			Vector2 anchoredPosition = base.TargetRectTransform.anchoredPosition;
			if (currentDestination == anchoredPosition)
			{
				currentDestination = GetRandomPosition();
			}
			base.TargetRectTransform.anchoredPosition = Vector2.MoveTowards(anchoredPosition, currentDestination, speed * Time.deltaTime);
		}
	}
}
