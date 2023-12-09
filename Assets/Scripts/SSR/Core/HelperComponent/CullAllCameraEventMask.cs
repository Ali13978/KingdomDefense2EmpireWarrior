using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(Camera))]
	public class CullAllCameraEventMask : MonoBehaviour
	{
		public void Update()
		{
			Camera component = GetComponent<Camera>();
			component.eventMask = 0;
		}
	}
}
