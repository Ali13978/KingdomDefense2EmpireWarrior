using UnityEngine;

namespace Gameplay
{
	public class CameraController : SingletonMonoBehaviour<CameraController>
	{
		[SerializeField]
		private PinchZoomFov pinchZoomFov;

		[SerializeField]
		private CameraHolderController cameraHolderController;

		public PinchZoomFov PinchZoomFov
		{
			get
			{
				return pinchZoomFov;
			}
			private set
			{
				pinchZoomFov = value;
			}
		}

		private void Awake()
		{
			PinchZoomFov = GetComponent<PinchZoomFov>();
		}

		private void Start()
		{
			PinchZoomFov.enabled = true;
		}

		[ContextMenu("Shake")]
		public void ShakeNormal()
		{
			cameraHolderController.ShakeNormal();
		}
	}
}
