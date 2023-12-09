using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class CurvyChaser : Follower
	{
		[Header("Chase setting")]
		[SerializeField]
		private float duration = 1f;

		[Header("Curves")]
		[SerializeField]
		private List<Vector3AnimationCurve> distubingCurves = new List<Vector3AnimationCurve>();

		private bool chasing;

		private float timeTracker;

		private float currentProgress;

		private Vector3 startPosition;

		private Vector3AnimationCurve currentCurve;

		[ContextMenu("Start chase")]
		public void StartChase()
		{
			timeTracker = 0f;
			startPosition = base.TargetPosition;
			if (distubingCurves.Count == 0)
			{
				currentCurve = null;
			}
			else
			{
				int index = Random.Range(0, distubingCurves.Count);
				currentCurve = distubingCurves[index];
			}
			chasing = true;
			base.enabled = true;
		}

		public void OnEnable()
		{
			if (!chasing)
			{
				base.enabled = false;
			}
		}

		public void OnDisable()
		{
			chasing = false;
		}

		public new void OnValidate()
		{
			base.OnValidate();
		}

		protected override void OnUpdatePositionFollow()
		{
			currentProgress = timeTracker / duration;
			base.TargetPosition = GetCurrentStablePosition() + GetCurrentDisturbingVector();
			if (timeTracker >= duration)
			{
				FinishChase();
			}
			else
			{
				timeTracker += Time.deltaTime;
			}
		}

		private Vector3 GetCurrentStablePosition()
		{
			return Vector3.Lerp(startPosition, base.Destination.Position, currentProgress);
		}

		private Vector3 GetCurrentDisturbingVector()
		{
			if (currentCurve == null)
			{
				return Vector3.zero;
			}
			return currentCurve.Evaluate(currentProgress);
		}

		public void FinishChase()
		{
			chasing = false;
			base.enabled = false;
		}
	}
}
