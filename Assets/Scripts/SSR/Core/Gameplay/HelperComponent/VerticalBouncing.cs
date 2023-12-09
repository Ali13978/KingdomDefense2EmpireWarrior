using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class VerticalBouncing : OutsiteTargetTransform
	{
		[SerializeField]
		private float gravityMagnitude;

		[SerializeField]
		private float groundY;

		[SerializeField]
		private float initialY;

		private float currentSpeedY;

		private bool inward = true;

		public float GroundY
		{
			get
			{
				return groundY;
			}
			set
			{
				groundY = value;
			}
		}

		public float GravityMagnitude
		{
			get
			{
				return gravityMagnitude;
			}
			set
			{
				gravityMagnitude = value;
			}
		}

		public virtual void Awake()
		{
			ResetState();
		}

		public virtual void Update()
		{
			Vector3 targetPosition = base.TargetPosition;
			float y = targetPosition.y;
			if (inward)
			{
				currentSpeedY += GravityMagnitude * Time.deltaTime;
				y = Mathf.MoveTowards(y, groundY, currentSpeedY * Time.deltaTime);
				if (y == groundY)
				{
					inward = false;
				}
			}
			else
			{
				y = Mathf.MoveTowards(y, initialY, currentSpeedY * Time.deltaTime);
				currentSpeedY = Mathf.MoveTowards(currentSpeedY, 0f, GravityMagnitude * Time.deltaTime);
				if (currentSpeedY == 0f)
				{
					inward = true;
				}
			}
			targetPosition.y = y;
			base.TargetPosition = targetPosition;
		}

		public void ResetState()
		{
			currentSpeedY = 0f;
			inward = true;
			Vector3 targetPosition = base.TargetPosition;
			initialY = targetPosition.y;
		}
	}
}
