using System.Collections.Generic;

namespace Data
{
	public class StarUpgradeParameter
	{
		private List<int> starPerTier;

		public List<int> Value
		{
			get
			{
				return starPerTier;
			}
			private set
			{
				starPerTier = value;
			}
		}

		public StarUpgradeParameter(List<int> value)
		{
			starPerTier = value;
		}
	}
}
