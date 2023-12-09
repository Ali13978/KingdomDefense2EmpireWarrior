using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class DailyRewardSerializeData
	{
		public int currentDay;

		private List<DailyRewardData> listDailyRewardData;

		public List<DailyRewardData> ListDailyRewarData
		{
			get
			{
				return listDailyRewardData;
			}
			set
			{
				listDailyRewardData = value;
			}
		}
	}
}
