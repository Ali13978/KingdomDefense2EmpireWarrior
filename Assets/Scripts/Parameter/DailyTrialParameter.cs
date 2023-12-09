using System.Collections.Generic;

namespace Parameter
{
	public class DailyTrialParameter
	{
		private static DailyTrialParameter instance;

		private static string DAILY_TRIAL_PREFIX = "TITLE_DAILY_TRIAL_{0}";

		public List<DailyTrialParam> listInputData = new List<DailyTrialParam>();

		public List<List<DailyTrialRewardParam>> listRewardData = new List<List<DailyTrialRewardParam>>();

		public static DailyTrialParameter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DailyTrialParameter();
				}
				return instance;
			}
		}

		public void SetParameter(DailyTrialParam param)
		{
			int count = listInputData.Count;
			if (count <= param.day)
			{
				listInputData.Add(param);
			}
		}

		private bool CheckParameter(int day)
		{
			if (day >= GetNumberOfParam())
			{
				return false;
			}
			return true;
		}

		public DailyTrialParam GetParameter(int day)
		{
			if (CheckParameter(day))
			{
				return listInputData[day];
			}
			return default(DailyTrialParam);
		}

		public int GetNumberOfParam()
		{
			return listInputData.Count;
		}

		public int[] getListInputItem(int day)
		{
			int[] array = new int[9];
			int[] array2 = array;
			DailyTrialParam dailyTrialParam = listInputData[day];
			array2[0] = dailyTrialParam.input_pi_freezing;
			int[] array3 = array;
			DailyTrialParam dailyTrialParam2 = listInputData[day];
			array3[1] = dailyTrialParam2.input_pi_meteor;
			int[] array4 = array;
			DailyTrialParam dailyTrialParam3 = listInputData[day];
			array4[2] = dailyTrialParam3.input_pi_healing;
			int[] array5 = array;
			DailyTrialParam dailyTrialParam4 = listInputData[day];
			array5[3] = dailyTrialParam4.input_pi_goldchest;
			array[4] = 0;
			array[5] = 0;
			array[6] = 0;
			array[7] = 0;
			array[8] = 0;
			return array;
		}

		public List<int> getListInputHeroesID(int day)
		{
			List<int> list = new List<int>();
			DailyTrialParam dailyTrialParam = listInputData[day];
			if (dailyTrialParam.input_hero_id_slot_0 >= 0)
			{
				List<int> list2 = list;
				DailyTrialParam dailyTrialParam2 = listInputData[day];
				list2.Add(dailyTrialParam2.input_hero_id_slot_0);
			}
			DailyTrialParam dailyTrialParam3 = listInputData[day];
			if (dailyTrialParam3.input_hero_id_slot_1 >= 0)
			{
				List<int> list3 = list;
				DailyTrialParam dailyTrialParam4 = listInputData[day];
				list3.Add(dailyTrialParam4.input_hero_id_slot_1);
			}
			DailyTrialParam dailyTrialParam5 = listInputData[day];
			if (dailyTrialParam5.input_hero_id_slot_2 >= 0)
			{
				List<int> list4 = list;
				DailyTrialParam dailyTrialParam6 = listInputData[day];
				list4.Add(dailyTrialParam6.input_hero_id_slot_2);
			}
			return list;
		}

		public bool IsTowerAllow(int day, int towerID)
		{
			bool flag = false;
			return GetMaxLevelTowerCanUpgrade(day, towerID) >= 0;
		}

		public int GetMaxLevelTowerCanUpgrade(int day, int towerID)
		{
			int result = -1;
			switch (towerID)
			{
			case 0:
			{
				DailyTrialParam dailyTrialParam5 = listInputData[day];
				result = dailyTrialParam5.tower_level_max_0;
				break;
			}
			case 1:
			{
				DailyTrialParam dailyTrialParam4 = listInputData[day];
				result = dailyTrialParam4.tower_level_max_1;
				break;
			}
			case 2:
			{
				DailyTrialParam dailyTrialParam3 = listInputData[day];
				result = dailyTrialParam3.tower_level_max_2;
				break;
			}
			case 3:
			{
				DailyTrialParam dailyTrialParam2 = listInputData[day];
				result = dailyTrialParam2.tower_level_max_3;
				break;
			}
			case 4:
			{
				DailyTrialParam dailyTrialParam = listInputData[day];
				result = dailyTrialParam.tower_level_max_4;
				break;
			}
			}
			return result;
		}

		public string GetTitle(int day)
		{
			string empty = string.Empty;
			return GameTools.GetLocalization(string.Format(DAILY_TRIAL_PREFIX, day));
		}

		public void SetRewardParameter(DailyTrialRewardParam reward)
		{
			int count = listRewardData.Count;
			if (count <= reward.day)
			{
				List<DailyTrialRewardParam> list = new List<DailyTrialRewardParam>();
				list.Insert(reward.reward_level, reward);
				listRewardData.Insert(reward.day, list);
			}
			else
			{
				List<DailyTrialRewardParam> list2 = listRewardData[reward.day];
				list2.Insert(reward.reward_level, reward);
			}
		}

		public DailyTrialRewardParam GetRewardParameter(int day, int rewardLevel)
		{
			if (CheckParameter(day, rewardLevel))
			{
				return listRewardData[day][rewardLevel];
			}
			return default(DailyTrialRewardParam);
		}

		private bool CheckParameter(int day, int rewardLevel)
		{
			if (day >= GetNumberOfRewardParam() || rewardLevel > GetNumberOfLevelReward())
			{
				return false;
			}
			return true;
		}

		public int GetNumberOfRewardParam()
		{
			return listRewardData.Count;
		}

		public int GetNumberOfLevelReward()
		{
			if (GetNumberOfRewardParam() > 0)
			{
				return listRewardData[0].Count;
			}
			return 0;
		}

		public int[] GetListWaveRank(int day)
		{
			int[] array = new int[3];
			int[] array2 = array;
			DailyTrialRewardParam rewardParameter = GetRewardParameter(day, 0);
			array2[0] = rewardParameter.wave_require;
			int[] array3 = array;
			DailyTrialRewardParam rewardParameter2 = GetRewardParameter(day, 1);
			array3[1] = rewardParameter2.wave_require;
			int[] array4 = array;
			DailyTrialRewardParam rewardParameter3 = GetRewardParameter(day, 2);
			array4[2] = rewardParameter3.wave_require;
			return array;
		}

		public int[] getListRewardValue(int day)
		{
			int[] array = new int[5];
			for (int i = 0; i < GetNumberOfLevelReward(); i++)
			{
				ref int reference = ref array[0];
				int num = reference;
				DailyTrialRewardParam rewardParameter = GetRewardParameter(day, i);
				reference = num + rewardParameter.item_freezing_bonus;
				ref int reference2 = ref array[1];
				int num2 = reference2;
				DailyTrialRewardParam rewardParameter2 = GetRewardParameter(day, i);
				reference2 = num2 + rewardParameter2.item_meteor_bonus;
				ref int reference3 = ref array[2];
				int num3 = reference3;
				DailyTrialRewardParam rewardParameter3 = GetRewardParameter(day, i);
				reference3 = num3 + rewardParameter3.item_healing_bonus;
				ref int reference4 = ref array[3];
				int num4 = reference4;
				DailyTrialRewardParam rewardParameter4 = GetRewardParameter(day, i);
				reference4 = num4 + rewardParameter4.item_goldchest_bonus;
				ref int reference5 = ref array[4];
				int num5 = reference5;
				DailyTrialRewardParam rewardParameter5 = GetRewardParameter(day, i);
				reference5 = num5 + rewardParameter5.gem_bonus;
			}
			return array;
		}
	}
}
