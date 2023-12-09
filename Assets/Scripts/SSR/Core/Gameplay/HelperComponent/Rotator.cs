using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class Rotator : OutsiteTargetTransform
	{
		[SerializeField]
		private Vector3 angularSpeed = default(Vector3);

		private void Update()
		{
			Rotate(angularSpeed * Time.deltaTime);
		}
	}
}
