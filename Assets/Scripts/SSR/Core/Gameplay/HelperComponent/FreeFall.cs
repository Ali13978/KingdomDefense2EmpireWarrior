using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class FreeFall : OutsiteTargetTransform
	{
		[SerializeField]
		private PositionProvider destination;

		[SerializeField]
		private float gravity = 25f;

		[SerializeField]
		private float initialSpeed;

		[Space]
		[SerializeField]
		private OrderedEventDispatcher stopFallingEvent = new OrderedEventDispatcher();

		private float currentSpeed;

		private bool falling;

		public float Gravity
		{
			get
			{
				return gravity;
			}
			set
			{
				gravity = value;
			}
		}

		private void OnEnable()
		{
			if (!falling)
			{
				base.enabled = falling;
			}
		}

		public void Update()
		{
			if (MoveTargetTransform() == destination.Position)
			{
				StopFalling();
			}
			else
			{
				currentSpeed += Gravity * Time.deltaTime;
			}
		}

		private Vector3 MoveTargetTransform()
		{
			Vector3 targetPosition = base.TargetPosition;
			return base.TargetPosition = Vector3.MoveTowards(targetPosition, destination.Position, currentSpeed * Time.deltaTime);
		}

		[ContextMenu("Start fall")]
		public void StartFall()
		{
			currentSpeed = initialSpeed;
			falling = true;
			base.enabled = true;
		}

		private void StopFalling()
		{
			falling = false;
			base.enabled = falling;
			stopFallingEvent.Dispatch();
		}
	}
}
