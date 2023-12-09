using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class ThemeSerializeData
	{
		private List<int> listThemeIDUnlocked;

		public int lastThemeIDPlayed;

		private Dictionary<int, int> listStatueData;

		public List<int> ListThemeIDUnlocked
		{
			get
			{
				return listThemeIDUnlocked;
			}
			set
			{
				listThemeIDUnlocked = value;
			}
		}

		public Dictionary<int, int> ListStatueData
		{
			get
			{
				return listStatueData;
			}
			set
			{
				listStatueData = value;
			}
		}
	}
}
