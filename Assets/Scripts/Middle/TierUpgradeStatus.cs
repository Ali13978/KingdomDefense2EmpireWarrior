using System;
using UnityEngine;

namespace Middle
{
	[Serializable]
	public class TierUpgradeStatus
	{
		[SerializeField]
		private int option1Level;

		[SerializeField]
		private int option2Level;

		public int Option1Level
		{
			get
			{
				return option1Level;
			}
			set
			{
				option1Level = value;
			}
		}

		public int Option2Level
		{
			get
			{
				return option2Level;
			}
			set
			{
				option2Level = value;
			}
		}

		public bool IsMaxed
		{
			get
			{
				int num = 0;
				for (int i = 0; i <= option1Level; i++)
				{
					num += i;
				}
				for (int j = 0; j <= option2Level; j++)
				{
					num += j;
				}
				return num == 6;
			}
		}

		public int GetLevel(int option)
		{
			if (option == 1)
			{
				return option1Level;
			}
			return option2Level;
		}

		public bool IsUpgradable(int option)
		{
			int num = 0;
			for (int i = 0; i <= Option1Level; i++)
			{
				num += i;
			}
			for (int j = 0; j <= Option2Level; j++)
			{
				num += j;
			}
			num += ((option != 1) ? Option2Level : Option1Level) + 1;
			return num <= 6;
		}
	}
}
