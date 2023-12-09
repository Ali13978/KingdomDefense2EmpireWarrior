using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Data
{
	[Serializable]
	public class MapSerializeData
	{
		public int mapIDUnlocked;

		public int lastMapIDPlayed;

		[OptionalField]
		public int lastMapModeChoose;

		private Dictionary<int, MapData> listMapsData;

		public Dictionary<int, MapData> ListMapsData
		{
			get
			{
				return listMapsData;
			}
			set
			{
				listMapsData = value;
			}
		}
	}
}
