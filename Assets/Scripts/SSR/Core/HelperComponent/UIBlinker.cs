using UnityEngine;
using UnityEngine.UI;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(Graphic))]
	public class UIBlinker : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private Graphic targetRenderer;

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
			targetRenderer = GetComponent<Graphic>();
		}
	}
}
