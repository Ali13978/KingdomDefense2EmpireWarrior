using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class SaleBundleData
	{
		private List<SerializeBundleItem> listSpecialBundleData;

		public string lastTimePlayed;

		public List<SerializeBundleItem> ListSpecialBundleData
		{
			get
			{
				return listSpecialBundleData;
			}
			set
			{
				listSpecialBundleData = value;
			}
		}
	}
}
