using UnityEngine;

namespace SSR.DevTool.SceneViewUtility
{
	public class RectangleGizmosIllustrator : GizmosIllustrator
	{
		[SerializeField]
		private Vector2 offset = Vector2.zero;

		[SerializeField]
		private Vector2 size = Vector2.one;

		protected override void DrawGizmos()
		{
			Gizmos.DrawWireCube(base.TargetTransform.position + (Vector3)offset, size);
		}
	}
}
