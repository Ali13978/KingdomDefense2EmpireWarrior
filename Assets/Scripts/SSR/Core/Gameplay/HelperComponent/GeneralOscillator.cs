using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class GeneralOscillator : OutsiteTargetTransform
	{
		[Header("GeneralOscillator")]
		[SerializeField]
		private float periodDuration = 1f;

		[SerializeField]
		private Vector3AnimationCurve positionCurve = Vector3AnimationCurve.Linear(0f, Vector3.zero, 1f, Vector3.zero);

		[SerializeField]
		private bool ignoreX;

		[SerializeField]
		private bool ignoreY;

		[SerializeField]
		private bool ignoreZ;

		private float timeTracking;

		public float PeriodDuration
		{
			get
			{
				return periodDuration;
			}
			set
			{
				periodDuration = value;
			}
		}

		public void Update()
		{
			UpdatePosition();
			UpdateTimeTracking();
		}

		private void UpdatePosition()
		{
			Vector3 targetPosition = positionCurve.Evaluate(timeTracking / PeriodDuration);
			Vector3 targetPosition2 = base.TargetPosition;
			if (ignoreX)
			{
				targetPosition.x = targetPosition2.x;
			}
			if (ignoreY)
			{
				targetPosition.y = targetPosition2.y;
			}
			if (ignoreZ)
			{
				targetPosition.z = targetPosition2.z;
			}
			base.TargetPosition = targetPosition;
		}

		private void UpdateTimeTracking()
		{
			if (timeTracking != PeriodDuration)
			{
				timeTracking = Mathf.MoveTowards(timeTracking, PeriodDuration, Time.deltaTime);
			}
			else
			{
				timeTracking = 0f;
			}
		}
	}
}
