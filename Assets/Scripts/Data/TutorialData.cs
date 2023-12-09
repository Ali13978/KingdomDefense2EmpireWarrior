using System;
using System.Collections.Generic;

namespace Data
{
	[Serializable]
	public class TutorialData
	{
		private Dictionary<string, bool> listTutorialData;

		public Dictionary<string, bool> ListTutorialData
		{
			get
			{
				return listTutorialData;
			}
			set
			{
				listTutorialData = value;
			}
		}
	}
}
