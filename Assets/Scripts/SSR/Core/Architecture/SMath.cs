using UnityEngine;

namespace SSR.Core.Architecture
{
	public static class SMath
	{
		public static Vector2 ClampSize(Vector2 originalSize, Vector2 maxSize)
		{
			while (originalSize.x > maxSize.x || originalSize.y > maxSize.y)
			{
				float num = 1f;
				num = ((!(originalSize.x > maxSize.x)) ? (maxSize.y / originalSize.y) : (maxSize.x / originalSize.x));
				originalSize *= num;
			}
			return originalSize;
		}
	}
}
