using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class MoveTo : OutsiteTargetTransform
	{
		[SerializeField]
		private float duration;

		[SerializeField]
		private PositionProvider destination;

		private bool moving;

		private float currentTime;

		private float step;

		private void OnEnable()
		{
			if (!moving)
			{
				base.enabled = false;
			}
		}

		public void StartMove()
		{
			currentTime = 0f;
			moving = true;
			step = Vector3.Distance(base.TargetPosition, destination.Position) / duration;
			base.enabled = true;
		}

		private void Update()
		{
			currentTime = Mathf.MoveTowards(currentTime, duration, Time.deltaTime);
		}
	}
}
