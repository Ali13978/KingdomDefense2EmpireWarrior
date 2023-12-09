using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class DailyTrialSerializeData
	{
		public int currentDay;

		private Dictionary<int, DailyTrialData> listDailyTrialData;

		public Dictionary<int, DailyTrialData> ListDailyTrialData
		{
			get
			{
				return listDailyTrialData;
			}
			set
			{
				listDailyTrialData = value;
			}
		}
	}
}
