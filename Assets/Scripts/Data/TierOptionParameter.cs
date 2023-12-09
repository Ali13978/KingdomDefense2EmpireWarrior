using System.Collections.Generic;

namespace Data
{
	public class TierOptionParameter
	{
		private List<int> value;

		public List<int> Value
		{
			get
			{
				return value;
			}
			private set
			{
				this.value = value;
			}
		}

		public TierOptionParameter(List<int> value)
		{
			this.value = value;
		}
	}
}
