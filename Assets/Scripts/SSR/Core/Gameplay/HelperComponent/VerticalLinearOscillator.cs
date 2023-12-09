using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class VerticalLinearOscillator : OutsiteTargetTransform
	{
		[SerializeField]
		private float upDistance = 1f;

		[SerializeField]
		private float downDistance = 1f;

		[SerializeField]
		private float duration = 1f;

		[SerializeField]
		private bool upwardFirst = true;

		private bool upward;

		private float speed;

		private float maxY;

		private float minY;

		public float UpDistance
		{
			get
			{
				return upDistance;
			}
			set
			{
				upDistance = value;
			}
		}

		public float DownDistance
		{
			get
			{
				return downDistance;
			}
			set
			{
				downDistance = value;
			}
		}

		public float Duration
		{
			get
			{
				return duration;
			}
			set
			{
				duration = value;
			}
		}

		public bool UpwardFirst
		{
			get
			{
				return upwardFirst;
			}
			set
			{
				upwardFirst = value;
			}
		}

		private void Awake()
		{
			ResetState();
		}

		public void Update()
		{
			Vector3 targetPosition = base.TargetPosition;
			float y = targetPosition.y;
			if (upward)
			{
				y = Mathf.MoveTowards(y, maxY, speed * Time.deltaTime);
				if (y == maxY)
				{
					upward = false;
				}
			}
			else
			{
				y = Mathf.MoveTowards(y, minY, speed * Time.deltaTime);
				if (y == minY)
				{
					upward = true;
				}
			}
			targetPosition.y = y;
			base.TargetPosition = targetPosition;
		}

		public void ResetState()
		{
			Vector3 targetPosition = base.TargetPosition;
			float y = targetPosition.y;
			upward = UpwardFirst;
			speed = (UpDistance + DownDistance) / Duration;
			maxY = y + UpDistance;
			minY = y - DownDistance;
		}
	}
}
