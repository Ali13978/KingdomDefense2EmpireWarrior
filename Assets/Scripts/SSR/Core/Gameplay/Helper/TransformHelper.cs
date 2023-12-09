using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Gameplay.Helper
{
	public static class TransformHelper
	{
		public class TransformSilblingIndexComparer : IComparer<Transform>
		{
			public int Compare(Transform x, Transform y)
			{
				int siblingIndex = x.GetSiblingIndex();
				int siblingIndex2 = y.GetSiblingIndex();
				if (siblingIndex > siblingIndex2)
				{
					return 1;
				}
				if (siblingIndex < siblingIndex2)
				{
					return -1;
				}
				return 0;
			}
		}

		public class TransformPositionXComparer : IComparer<Transform>
		{
			public int Compare(Transform x, Transform y)
			{
				Vector3 position = x.position;
				float x2 = position.x;
				Vector3 position2 = y.position;
				float x3 = position2.x;
				if (x2 > x3)
				{
					return 1;
				}
				if (x2 < x3)
				{
					return -1;
				}
				return 0;
			}
		}

		public class TransformPositionYComparer : IComparer<Transform>
		{
			public int Compare(Transform x, Transform y)
			{
				Vector3 position = x.position;
				float y2 = position.y;
				Vector3 position2 = y.position;
				float y3 = position2.y;
				if (y2 > y3)
				{
					return 1;
				}
				if (y2 < y3)
				{
					return -1;
				}
				return 0;
			}
		}

		public static void SetLocalPositionX(this Transform transform, float newX)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.x = newX;
			transform.localPosition = localPosition;
		}

		public static void SetLocalPositionY(this Transform transform, float newY)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.y = newY;
			transform.localPosition = localPosition;
		}

		public static void SetLocalScaleX(this Transform transform, float newX)
		{
			Vector3 localScale = transform.localScale;
			localScale.x = newX;
			transform.localScale = localScale;
		}

		public static void SetLocalScaleY(this Transform transform, float newY)
		{
			Vector3 localScale = transform.localScale;
			localScale.y = newY;
			transform.localScale = localScale;
		}

		public static void SetLocalPositionXY(this Transform transform, Vector2 position)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.x = position.x;
			localPosition.y = position.y;
			transform.localPosition = localPosition;
		}

		public static void SetPositionXY(this Transform transform, Vector2 position)
		{
			Vector3 position2 = transform.position;
			position2.x = position.x;
			position2.y = position.y;
			transform.position = position2;
		}
	}
}
