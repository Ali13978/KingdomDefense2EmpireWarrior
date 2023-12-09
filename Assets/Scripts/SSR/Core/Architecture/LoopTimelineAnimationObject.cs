using UnityEngine;

namespace SSR.Core.Architecture
{
	public abstract class LoopTimelineAnimationObject : MonoBehaviour, ITimelineAnimationObject
	{
		[SerializeField]
		[Range(0f, 1f)]
		private float currentPosition;

		private float lastUpdatedPosition = float.NaN;

		public float CurrentPosition
		{
			get
			{
				return currentPosition;
			}
			set
			{
				currentPosition = value;
			}
		}

		public void Update()
		{
			UpdateState(currentPosition, currentPosition != lastUpdatedPosition);
			lastUpdatedPosition = currentPosition;
		}

		protected abstract void UpdateState(float currentPosition, bool newPosition);
	}
}
