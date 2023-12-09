using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	[RequireComponent(typeof(Renderer))]
	public class Blinker : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private Renderer targetRenderer;

		[SerializeField]
		private float interval = 0.2f;

		private float timeTracking;

		public void Update()
		{
			timeTracking += Time.deltaTime;
			if (timeTracking >= interval)
			{
				timeTracking = 0f;
				targetRenderer.enabled = !targetRenderer.enabled;
			}
		}

		public void Reset()
		{
			targetRenderer = GetComponent<Renderer>();
		}
	}
}
