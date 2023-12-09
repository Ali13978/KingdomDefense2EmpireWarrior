using System;
using System.Runtime.Serialization;

namespace Data
{
	[Serializable]
	public class HeroData
	{
		public int level;

		public int totalExp;

		[OptionalField]
		public int[] skillPoints;

		[OptionalField]
		public bool havePet;
	}
}
