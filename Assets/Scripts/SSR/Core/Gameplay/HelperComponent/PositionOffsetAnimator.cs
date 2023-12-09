using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class PositionOffsetAnimator : OutsiteTargetTransform
	{
		[Header("X")]
		[SerializeField]
		private AnimationCurve offsetXCurve = new AnimationCurve();

		[SerializeField]
		private float amplitudeX = 1f;

		[Header("Y")]
		[SerializeField]
		private AnimationCurve offsetYCurve = new AnimationCurve();

		[SerializeField]
		private float amplitudeY = 1f;

		[Header("Z")]
		[SerializeField]
		private AnimationCurve offsetZCurve = new AnimationCurve();

		[SerializeField]
		private float amplitudeZ = 1f;

		[Space]
		[SerializeField]
		private float duration = 1f;

		[SerializeField]
		private bool loop;

		private float currentTime;

		private float currentTime01;

		private Vector3 originalPosition;

		private bool animating;

		private void OnEnable()
		{
			if (!animating)
			{
				base.enabled = false;
			}
		}

		private void Update()
		{
			UpdateTargetPosition();
			UpdateCurrentTime();
		}

		private void UpdateTargetPosition()
		{
			Vector3 b = default(Vector3);
			b.x = GetOffset(offsetXCurve, currentTime, amplitudeX);
			b.y = GetOffset(offsetYCurve, currentTime, amplitudeY);
			b.z = GetOffset(offsetZCurve, currentTime, amplitudeZ);
			base.TargetPosition = originalPosition + b;
		}

		private void UpdateCurrentTime()
		{
			if (currentTime >= duration)
			{
				if (loop)
				{
					currentTime = 0f;
					currentTime01 = 0f;
				}
				else
				{
					StopAnimate();
				}
			}
			else
			{
				currentTime += Time.deltaTime;
				currentTime01 = currentTime / duration;
			}
		}

		private float GetOffset(AnimationCurve curve, float time, float amplitude)
		{
			return curve.Evaluate(time) * amplitude;
		}

		[ContextMenu("Start animating")]
		public void StartAnimate()
		{
			currentTime = 0f;
			originalPosition = base.TargetPosition;
			animating = true;
			base.enabled = true;
		}

		public void StopAnimate()
		{
			animating = false;
			base.enabled = false;
		}

		public void CreateCurve()
		{
		}
	}
}
