using System;
using System.Collections.Generic;

namespace Parameter
{
	public class MapRuleParameter
	{
		public List<CampaignMap> listMapRuleCampaign = new List<CampaignMap>();

		public List<DailyTrialMap> listMapRuleDaily = new List<DailyTrialMap>();

		public List<TournamentMap> listMapRuleTournament = new List<TournamentMap>();

		public Dictionary<int, TournamentPriceConstant> leagueIndexToPriceConstants = new Dictionary<int, TournamentPriceConstant>();

		public string curSeasonID = "ss0";

		public bool IsMapRuleRead;

		public bool IsPriceConstantRead;

		private static MapRuleParameter instance;

		public static MapRuleParameter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MapRuleParameter();
				}
				return instance;
			}
		}

		public void SetMapRuleParameter(CampaignMap mapRule)
		{
			int count = listMapRuleCampaign.Count;
			if (count <= mapRule.mapID)
			{
				listMapRuleCampaign.Insert(mapRule.mapID, mapRule);
			}
		}

		public CampaignMap GetCampaignMapRuleParameter(int mapID)
		{
			return listMapRuleCampaign[mapID];
		}

		public bool IsTowerAllowed_Campaign(int mapID, int towerID)
		{
			bool flag = false;
			return GetMaxLevelTowerCanUpgrade_Campaign(mapID, towerID) >= 0;
		}

		public int GetMaxLevelTowerCanUpgrade_Campaign(int mapID, int towerID)
		{
			int result = -1;
			switch (towerID)
			{
			case 0:
			{
				CampaignMap campaignMap5 = listMapRuleCampaign[mapID];
				result = campaignMap5.tower_0_level;
				break;
			}
			case 1:
			{
				CampaignMap campaignMap4 = listMapRuleCampaign[mapID];
				result = campaignMap4.tower_1_level;
				break;
			}
			case 2:
			{
				CampaignMap campaignMap3 = listMapRuleCampaign[mapID];
				result = campaignMap3.tower_2_level;
				break;
			}
			case 3:
			{
				CampaignMap campaignMap2 = listMapRuleCampaign[mapID];
				result = campaignMap2.tower_3_level;
				break;
			}
			case 4:
			{
				CampaignMap campaignMap = listMapRuleCampaign[mapID];
				result = campaignMap.tower_4_level;
				break;
			}
			}
			return result;
		}

		public int[] GetMaxTowerLevelByMapID(int mapID)
		{
			int[] array = new int[5];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GetMaxLevelTowerCanUpgrade_Campaign(mapID, i);
			}
			return array;
		}

		public int GetMaxHeroAllowed(int mapID)
		{
			CampaignMap campaignMap = listMapRuleCampaign[mapID];
			return campaignMap.hero_allowed_max;
		}

		public bool HaveTutorialTowerInMap(int mapID)
		{
			bool flag = false;
			List<TowerTutorial> listTowerTutorialInMap = getListTowerTutorialInMap(mapID);
			return listTowerTutorialInMap.Count > 0;
		}

		public bool HaveNormalInfor(int mapID)
		{
			return getTipInforID(mapID) > 0;
		}

		public int getTipInforID(int mapID)
		{
			CampaignMap campaignMap = listMapRuleCampaign[mapID];
			return campaignMap.tip_infor_id;
		}

		public bool HaveUltimateInfor(int mapID)
		{
			bool result = false;
			List<TowerTutorial> listTowerTutorialInMap = getListTowerTutorialInMap(mapID);
			foreach (TowerTutorial item in listTowerTutorialInMap)
			{
				if (item.level >= 3)
				{
					result = true;
				}
			}
			return result;
		}

		public List<TowerTutorial> getListTowerTutorialInMap(int mapID)
		{
			List<TowerTutorial> list = new List<TowerTutorial>();
			foreach (CampaignMap item4 in listMapRuleCampaign)
			{
				CampaignMap current = item4;
				if (current.mapID == mapID)
				{
					string[] array = current.tutorial_param_0.Split(new char[1]
					{
						'_'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length == 2)
					{
						TowerTutorial towerTutorial = new TowerTutorial();
						towerTutorial.id = int.Parse(array[0]);
						towerTutorial.level = int.Parse(array[1]);
						TowerTutorial item = towerTutorial;
						list.Add(item);
					}
					string[] array2 = current.tutorial_param_1.Split(new char[1]
					{
						'_'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array2.Length == 2)
					{
						TowerTutorial towerTutorial = new TowerTutorial();
						towerTutorial.id = int.Parse(array2[0]);
						towerTutorial.level = int.Parse(array2[1]);
						TowerTutorial item2 = towerTutorial;
						list.Add(item2);
					}
					string[] array3 = current.tutorial_param_2.Split(new char[1]
					{
						'_'
					}, StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length == 2)
					{
						TowerTutorial towerTutorial = new TowerTutorial();
						towerTutorial.id = int.Parse(array3[0]);
						towerTutorial.level = int.Parse(array3[1]);
						TowerTutorial item3 = towerTutorial;
						list.Add(item3);
					}
				}
			}
			return list;
		}

		public void SetMapRuleParameter(DailyTrialMap mapRule)
		{
			int count = listMapRuleDaily.Count;
			if (count <= mapRule.wave)
			{
				listMapRuleDaily.Insert(mapRule.wave, mapRule);
			}
		}

		public DailyTrialMap GetDailyTrialMapRuleParameter(int wave)
		{
			return listMapRuleDaily[wave];
		}

		public int GetGoldBonusForCallEnemy(int wave)
		{
			DailyTrialMap dailyTrialMap = listMapRuleDaily[wave];
			return dailyTrialMap.bonus_gold;
		}

		public int GetHealthBonusForCallEnemy(int wave)
		{
			DailyTrialMap dailyTrialMap = listMapRuleDaily[wave];
			return dailyTrialMap.bonus_health;
		}

		public bool IsTowerAllowed_Daily(int wave, int towerID)
		{
			bool flag = false;
			return GetMaxLevelTowerCanUpgrade_Daily(wave, towerID) >= 0;
		}

		public int GetMaxLevelTowerCanUpgrade_Daily(int wave, int towerID)
		{
			int result = -1;
			switch (towerID)
			{
			case 0:
			{
				DailyTrialMap dailyTrialMap5 = listMapRuleDaily[wave];
				result = dailyTrialMap5.tower_0_level;
				break;
			}
			case 1:
			{
				DailyTrialMap dailyTrialMap4 = listMapRuleDaily[wave];
				result = dailyTrialMap4.tower_1_level;
				break;
			}
			case 2:
			{
				DailyTrialMap dailyTrialMap3 = listMapRuleDaily[wave];
				result = dailyTrialMap3.tower_2_level;
				break;
			}
			case 3:
			{
				DailyTrialMap dailyTrialMap2 = listMapRuleDaily[wave];
				result = dailyTrialMap2.tower_3_level;
				break;
			}
			case 4:
			{
				DailyTrialMap dailyTrialMap = listMapRuleDaily[wave];
				result = dailyTrialMap.tower_4_level;
				break;
			}
			}
			return result;
		}

		public bool HaveEventNewTower(int wave)
		{
			DailyTrialMap dailyTrialMap = listMapRuleDaily[wave];
			return dailyTrialMap.new_tower_event == 1;
		}

		public List<int> GetListTowerIDForPopup(int wave)
		{
			List<int> list = new List<int>();
			DailyTrialMap dailyTrialMap = listMapRuleDaily[wave];
			if (dailyTrialMap.new_tower_id_slot_0 >= 0)
			{
				List<int> list2 = list;
				DailyTrialMap dailyTrialMap2 = listMapRuleDaily[wave];
				list2.Add(dailyTrialMap2.new_tower_id_slot_0);
			}
			DailyTrialMap dailyTrialMap3 = listMapRuleDaily[wave];
			if (dailyTrialMap3.new_tower_id_slot_1 >= 0)
			{
				List<int> list3 = list;
				DailyTrialMap dailyTrialMap4 = listMapRuleDaily[wave];
				list3.Add(dailyTrialMap4.new_tower_id_slot_1);
			}
			DailyTrialMap dailyTrialMap5 = listMapRuleDaily[wave];
			if (dailyTrialMap5.new_tower_id_slot_2 >= 0)
			{
				List<int> list4 = list;
				DailyTrialMap dailyTrialMap6 = listMapRuleDaily[wave];
				list4.Add(dailyTrialMap6.new_tower_id_slot_2);
			}
			return list;
		}

		public void SetMapRuleParameter(TournamentMap mapRule)
		{
			int count = listMapRuleTournament.Count;
			string seasonID = mapRule.seasonID;
			seasonID = seasonID.Remove(0, 2);
			int num = int.Parse(seasonID);
			if (count <= num)
			{
				listMapRuleTournament.Add(mapRule);
			}
		}

		public TournamentMap GetTournamentMapRuleParameter(int mapID)
		{
			TournamentMap result = default(TournamentMap);
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.mapID == mapID)
				{
					result = current;
				}
			}
			return result;
		}

		public void AddTournamentConstants(int leagueIndex, TournamentPriceConstant data)
		{
			leagueIndexToPriceConstants.Add(leagueIndex, data);
		}

		public bool IsTowerAllowed_Tournament(string seasonID, int towerID)
		{
			bool flag = false;
			return GetMaxLevelTowerCanUpgrade_Tournament(seasonID, towerID) >= 0;
		}

		public int GetMaxLevelTowerCanUpgrade_Tournament(string seasonID, int towerID)
		{
			int result = 0;
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.seasonID.Equals(seasonID))
				{
					switch (towerID)
					{
					case 0:
						result = current.tower_0_level;
						break;
					case 1:
						result = current.tower_1_level;
						break;
					case 2:
						result = current.tower_2_level;
						break;
					case 3:
						result = current.tower_3_level;
						break;
					case 4:
						result = current.tower_4_level;
						break;
					}
				}
			}
			return result;
		}

		public int GetCurrentSeasonMapID()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.seasonID.Equals(currentSeasonID))
				{
					result = current.mapID;
				}
			}
			return result;
		}

		public int GetBlessedHeroID()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.seasonID.Equals(currentSeasonID))
				{
					result = current.blessedHeroID;
				}
			}
			return result;
		}

		public int GetPowerupItemLimit()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.seasonID.Equals(currentSeasonID))
				{
					result = current.powerupItemLimit;
				}
			}
			return result;
		}

		public string GetCurrentSeasonID()
		{
			return curSeasonID;
		}

		public void SetCurrentSeasonID(int seasonNumber)
		{
			curSeasonID = "ss" + seasonNumber % 6;
		}

		public int GetEndlessWaveLoopBegin()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.seasonID.Equals(currentSeasonID))
				{
					result = current.wave_loop_begin;
				}
			}
			return result;
		}

		public int GetEndlessWaveLoopEnd()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.seasonID.Equals(currentSeasonID))
				{
					result = current.wave_loop_end;
				}
			}
			return result;
		}

		public int GetEndlessHealthIncreasePercentage()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.seasonID.Equals(currentSeasonID))
				{
					result = current.health_increase_percentage_per_loop;
				}
			}
			return result;
		}

		public int GetEndlessDamageIncreasePercentage()
		{
			int result = 0;
			string currentSeasonID = GetCurrentSeasonID();
			foreach (TournamentMap item in listMapRuleTournament)
			{
				TournamentMap current = item;
				if (current.seasonID.Equals(currentSeasonID))
				{
					result = current.damage_increase_percentage_per_loop;
				}
			}
			return result;
		}
	}
}
