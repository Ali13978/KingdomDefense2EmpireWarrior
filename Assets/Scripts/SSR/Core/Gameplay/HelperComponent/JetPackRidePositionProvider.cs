using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class JetPackRidePositionProvider : PositionProvider
	{
		public enum State
		{
			Fall,
			Rise
		}

		[SerializeField]
		private PositionProvider referencePositionProvider;

		[SerializeField]
		private float maxY;

		[SerializeField]
		private float minY;

		[SerializeField]
		private State initialState = State.Rise;

		[SerializeField]
		[ReadOnly]
		private State state;

		[SerializeField]
		[ReadOnly]
		private Vector3 position;

		[SerializeField]
		private JetPackRideYSpeedModel ySpeedModel = new JetPackRideYSpeedModel();

		private int lastUpdatedFrame;

		public override Vector3 Position
		{
			get
			{
				if (lastUpdatedFrame != Time.frameCount)
				{
					UpdatePosition();
					lastUpdatedFrame = Time.frameCount;
				}
				return position;
			}
		}

		public void UpdatePosition()
		{
			Vector3 vector = referencePositionProvider.Position;
			position.z = vector.z;
			position.x = vector.x;
			position.y += ySpeedModel.CurrentSpeed * Time.deltaTime;
			position.y = Mathf.Clamp(position.y, minY, maxY);
			if (position.y == minY)
			{
				ySpeedModel.OnHitGround();
			}
			else if (position.y == maxY)
			{
				ySpeedModel.OnHitCeil();
			}
			switch (state)
			{
			case State.Fall:
				ySpeedModel.UpdateFall();
				break;
			case State.Rise:
				ySpeedModel.UpdateRise();
				break;
			}
		}

		public void Initialize()
		{
			ySpeedModel.OnInitialize();
			position = referencePositionProvider.Position;
			state = initialState;
			if (state == State.Fall)
			{
				ySpeedModel.OnStartFall();
			}
			else
			{
				ySpeedModel.OnStartRise();
			}
		}

		public void SwitchToFall()
		{
			state = State.Fall;
			ySpeedModel.OnStartFall();
		}

		public void SwitchToRise()
		{
			state = State.Rise;
			ySpeedModel.OnStartRise();
		}
	}
}
