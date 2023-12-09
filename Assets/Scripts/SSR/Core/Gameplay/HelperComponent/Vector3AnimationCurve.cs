using System;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	[Serializable]
	public class Vector3AnimationCurve
	{
		[SerializeField]
		private AnimationCurve x = AnimationCurve.Linear(0f, 0f, 1f, 0f);

		[SerializeField]
		private AnimationCurve y = AnimationCurve.Linear(0f, 0f, 1f, 0f);

		[SerializeField]
		private AnimationCurve z = AnimationCurve.Linear(0f, 0f, 1f, 0f);

		public Vector3 Evaluate(float time)
		{
			return new Vector3(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time));
		}

		public static Vector3AnimationCurve Linear(float timeStart, Vector3 valueStart, float timeEnd, Vector3 valueEnd)
		{
			Vector3AnimationCurve vector3AnimationCurve = new Vector3AnimationCurve();
			vector3AnimationCurve.x = AnimationCurve.Linear(timeStart, valueStart.x, timeEnd, valueEnd.x);
			vector3AnimationCurve.y = AnimationCurve.Linear(timeStart, valueStart.y, timeEnd, valueEnd.y);
			vector3AnimationCurve.z = AnimationCurve.Linear(timeStart, valueStart.z, timeEnd, valueEnd.z);
			return vector3AnimationCurve;
		}
	}
}
