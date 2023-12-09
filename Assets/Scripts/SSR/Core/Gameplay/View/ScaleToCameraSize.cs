using UnityEngine;

namespace SSR.Core.Gameplay.View
{
	public class ScaleToCameraSize : MonoBehaviour
	{
		[SerializeField]
		private bool autoScaleAtStart = true;

		[SerializeField]
		private Camera targetCamera;

		public void Start()
		{
			Scale();
		}

		public void Scale()
		{
			float num = targetCamera.orthographicSize * 2f;
			float x = targetCamera.aspect * num;
			Vector3 localScale = base.transform.localScale;
			localScale.x = x;
			localScale.y = num;
			base.transform.localScale = localScale;
		}
	}
}
