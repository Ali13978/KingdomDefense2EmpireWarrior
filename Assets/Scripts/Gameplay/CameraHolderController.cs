using DG.Tweening;
using Middle;
using UnityEngine;

namespace Gameplay
{
	public class CameraHolderController : MonoBehaviour
	{
		[Space]
		[Header("Camera Shake")]
		[SerializeField]
		private float duration;

		[SerializeField]
		private Vector3 streng;

		[SerializeField]
		private int vibro;

		[SerializeField]
		private float randomness;

		[SerializeField]
		private bool snapping;

		private bool isShaking;

		[ContextMenu("Shake")]
		public void ShakeNormal()
		{
			isShaking = true;
			base.transform.DOShakePosition(duration, streng, vibro, randomness, snapping).OnComplete(EndShake);
			if (Config.Instance.Vibration)
			{
				Handheld.Vibrate();
			}
		}

		private void EndShake()
		{
			isShaking = false;
		}
	}
}
