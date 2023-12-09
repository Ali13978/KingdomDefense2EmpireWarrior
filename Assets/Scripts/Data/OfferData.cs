using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class OfferData
	{
		private Dictionary<string, bool> listOfferData;

		public Dictionary<string, bool> ListOfferData
		{
			get
			{
				return listOfferData;
			}
			set
			{
				listOfferData = value;
			}
		}
	}
}
