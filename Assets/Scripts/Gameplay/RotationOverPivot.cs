using UnityEngine;

namespace Gameplay
{
	public class RotationOverPivot : MonoBehaviour
	{
		[SerializeField]
		private float speed;

		public int RotateDirection;

		private void Update()
		{
			Rotate();
		}

		private void Rotate()
		{
			base.transform.Rotate(Vector3.back * RotateDirection * speed);
		}
	}
}
