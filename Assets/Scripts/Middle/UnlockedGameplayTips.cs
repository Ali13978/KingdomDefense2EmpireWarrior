using MyCustom;
using System.Collections.Generic;
using UnityEngine;

namespace Middle
{
	public class UnlockedGameplayTips : CustomMonoBehaviour
	{
		private int NumberOfTips;

		private const string KeyFormat = "TipUnlocked_{0}";

		private static bool[] tipsFirstTime;

		private static UnlockedGameplayTips instance;

		public static UnlockedGameplayTips Instance
		{
			get
			{
				return instance;
			}
			private set
			{
				instance = value;
			}
		}

		private void Awake()
		{
			Instance = this;
			NumberOfTips = 7;
			LoadTipsFirstTime();
		}

		public bool IsTipFirstTime(int tipID)
		{
			bool flag = tipsFirstTime[tipID];
			if (flag)
			{
				UnlockTip(tipID);
			}
			return flag;
		}

		public static bool TipAppeared(int enemyId)
		{
			return !tipsFirstTime[enemyId];
		}

		private static void UnlockTip(int enemyId)
		{
			tipsFirstTime[enemyId] = false;
			PlayerPrefs.SetInt($"TipUnlocked_{enemyId}", 1);
		}

		private void LoadTipsFirstTime()
		{
			tipsFirstTime = new bool[NumberOfTips];
			for (int i = 0; i < NumberOfTips; i++)
			{
				tipsFirstTime[i] = (PlayerPrefs.GetInt($"TipUnlocked_{i}", 0) == 0);
			}
		}

		public List<bool> GetListTipsUnlockStatus()
		{
			List<bool> list = new List<bool>();
			for (int i = 0; i < tipsFirstTime.Length; i++)
			{
				if (!tipsFirstTime[i])
				{
					list.Add(item: true);
				}
				else
				{
					list.Add(item: false);
				}
			}
			return list;
		}
	}
}
