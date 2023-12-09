using DG.Tweening;
using UnityEngine;

namespace MyTween
{
	public static class MyTween
	{
		public static void MyTweenLookAt2D(this Transform target, Vector2 posToLook)
		{
			Vector2 vector = posToLook - (Vector2)target.position;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f + 90f;
			Quaternion b = Quaternion.AngleAxis(angle, Vector3.forward);
			target.rotation = Quaternion.Slerp(target.rotation, b, Time.deltaTime);
		}

		public static void MyTweenLookAtDirect2D(this Transform target, Vector2 posToLook)
		{
			Vector2 vector = posToLook - (Vector2)target.position;
			float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f + 90f;
			target.rotation = Quaternion.AngleAxis(angle, Vector3.right);
		}

		public static Transform MyTweenMoveToPosition(this Transform target, Vector3 posMoveTo, float speed)
		{
			float duration = Vector3.Distance(target.position, posMoveTo) / speed;
			target.DOMove(posMoveTo, duration);
			return target;
		}
	}
}
