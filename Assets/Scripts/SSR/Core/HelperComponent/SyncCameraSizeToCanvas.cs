using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[ExecuteInEditMode]
	public class SyncCameraSizeToCanvas : MonoBehaviour
	{
		[SerializeField]
		private RectTransform canvasRectTransform;

		[SerializeField]
		private Camera targetCamera;

		public void LateUpdate()
		{
			Camera camera = targetCamera;
			Vector2 sizeDelta = canvasRectTransform.sizeDelta;
			float y = sizeDelta.y;
			Vector3 lossyScale = canvasRectTransform.lossyScale;
			camera.orthographicSize = y * lossyScale.y / 2f;
		}
	}
}
