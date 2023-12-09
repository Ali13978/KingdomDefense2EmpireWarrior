using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class YCurveRandomPositionProvider : PositionProvider
	{
		[SerializeField]
		private PositionProvider originalPositionProvider;

		[SerializeField]
		private float leftDistance;

		[SerializeField]
		private float rightDistance;

		[SerializeField]
		private AnimationCurve yCurve;

		public override Vector3 Position
		{
			get
			{
				Vector3 position = originalPositionProvider.Position;
				Vector3 position2 = originalPositionProvider.Position;
				float x = position2.x;
				float num = x - leftDistance;
				float max = x + rightDistance;
				float num2 = Random.Range(num, max);
				float y = yCurve.Evaluate((num2 - num) / (leftDistance + rightDistance)) + position.y;
				return new Vector3(num2, y, position.z);
			}
		}
	}
}
