using System;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	[Serializable]
	public class JetPackRideYSpeedModel : IJetPackRideYSpeedModel
	{
		[Header("Fall")]
		[SerializeField]
		private float initialFallSpeed = -2f;

		[SerializeField]
		private float fallAcceleration = -2f;

		[Header("Rise")]
		[SerializeField]
		private float initialRiseSpeed = 2f;

		[SerializeField]
		private float riseAcceleration = 2f;

		[SerializeField]
		[ReadOnly]
		private float speed;

		public float CurrentSpeed => speed;

		public void OnHitCeil()
		{
			speed = 0f;
		}

		public void OnHitGround()
		{
			speed = 0f;
		}

		public void OnInitialize()
		{
			speed = 0f;
		}

		public void OnStartFall()
		{
			speed = initialFallSpeed;
		}

		public void OnStartRise()
		{
			speed = initialRiseSpeed;
		}

		public void UpdateFall()
		{
			speed += fallAcceleration * Time.deltaTime;
		}

		public void UpdateRise()
		{
			speed += riseAcceleration * Time.deltaTime;
		}
	}
}
