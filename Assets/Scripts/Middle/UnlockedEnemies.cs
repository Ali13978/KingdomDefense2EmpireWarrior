using MyCustom;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Middle
{
	public class UnlockedEnemies : CustomMonoBehaviour
	{
		private int NumberOfEnemies;

		private const string KeyFormat = "EnemyUnlocked_{0}";

		private static bool[] enemiesFirstTime;

		private static UnlockedEnemies instance;

		public static UnlockedEnemies Instance
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
			NumberOfEnemies = EnemyParameterManager.Instance.GetNumberOfEnemy();
			LoadEnemiesFirstTime();
		}

		public bool IsEnemyFirstTime(int enemyId)
		{
			bool flag = enemiesFirstTime[enemyId];
			if (flag)
			{
				UnlockEnemy(enemyId);
			}
			return flag;
		}

		public static bool EnemyAppeared(int enemyId)
		{
			return !enemiesFirstTime[enemyId];
		}

		private static void UnlockEnemy(int enemyId)
		{
			enemiesFirstTime[enemyId] = false;
			PlayerPrefs.SetInt($"EnemyUnlocked_{enemyId}", 1);
		}

		private void LoadEnemiesFirstTime()
		{
			enemiesFirstTime = new bool[NumberOfEnemies];
			for (int i = 0; i < NumberOfEnemies; i++)
			{
				enemiesFirstTime[i] = (PlayerPrefs.GetInt($"EnemyUnlocked_{i}", 0) == 0);
			}
		}

		public List<bool> GetListEnemyUnlockStatus()
		{
			List<bool> list = new List<bool>();
			for (int i = 0; i < enemiesFirstTime.Length; i++)
			{
				if (!enemiesFirstTime[i])
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
