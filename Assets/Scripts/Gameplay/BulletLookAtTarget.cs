using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class BulletLookAtTarget : CustomMonoBehaviour
	{
		private Vector2 myPreviousPosition;

		private void Update()
		{
			LookAtDirection2D((Vector2)base.transform.position - myPreviousPosition);
			myPreviousPosition = base.transform.position;
		}

		private void LookAtDirection2D(Vector2 direction)
		{
			float angle = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
			base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}
