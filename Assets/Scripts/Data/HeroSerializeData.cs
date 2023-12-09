using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class HeroSerializeData
	{
		private Dictionary<int, HeroData> listHeroesData;

		private List<int> listHeroOwned;

		public Dictionary<int, HeroData> ListHeroesData
		{
			get
			{
				return listHeroesData;
			}
			set
			{
				listHeroesData = value;
			}
		}

		public List<int> ListHeroOwned
		{
			get
			{
				return listHeroOwned;
			}
			set
			{
				listHeroOwned = value;
			}
		}
	}
}
