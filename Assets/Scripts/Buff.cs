namespace Gameplay
{
	public struct Buff
	{
		private bool isPositive;

		private float value;

		private float duration;

		public float Duration
		{
			get
			{
				return duration;
			}
			set
			{
				duration = value;
			}
		}

		public float Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}

		public bool IsPositive
		{
			get
			{
				return isPositive;
			}
			set
			{
				isPositive = value;
			}
		}

		public Buff(bool isPositive, float value, float duration)
		{
			this.isPositive = isPositive;
			this.value = value;
			this.duration = duration;
		}
	}
}
