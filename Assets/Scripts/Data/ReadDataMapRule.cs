using LitJson;
using Middle;
using MyCustom;
using Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Data
{
	public class ReadDataMapRule : MonoBehaviour
	{
		[Header("Tournament data")]
		[SerializeField]
		private string jsonDataTournamentMapRuleString;

		public string jsonDataTournamentPriceConstantPath;

		public string jsonDataTournamentRewardPath = "https://firebasestorage.googleapis.com/v0/b/kingdom-defense-82890624.appspot.com/o/tournament_reward.txt?alt=media&token=52f10978-c646-4f13-a220-89a3546d7c04";

		private JsonData[] jsonDataTournamentMapRule;

		private void Awake()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				ReadCampaignMapRule();
				break;
			case GameMode.DailyTrialMode:
				ReadDailyTrialMapRule();
				break;
			}
		}

		private void ReadCampaignMapRule()
		{
			string text = "Parameters/map_rule";
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					CampaignMap mapRuleParameter = default(CampaignMap);
					mapRuleParameter.mapID = (int)list[i]["map_id"];
					mapRuleParameter.tower_0_level = (int)list[i]["tower_0_level"];
					mapRuleParameter.tower_1_level = (int)list[i]["tower_1_level"];
					mapRuleParameter.tower_2_level = (int)list[i]["tower_2_level"];
					mapRuleParameter.tower_3_level = (int)list[i]["tower_3_level"];
					mapRuleParameter.tower_4_level = (int)list[i]["tower_4_level"];
					mapRuleParameter.hero_allowed_max = (int)list[i]["hero_allowed_max"];
					mapRuleParameter.have_tower_preview = (int)list[i]["have_tower_preview"];
					mapRuleParameter.tutorial_param_0 = (string)list[i]["param_0"];
					mapRuleParameter.tutorial_param_1 = (string)list[i]["param_1"];
					mapRuleParameter.tutorial_param_2 = (string)list[i]["param_2"];
					mapRuleParameter.tip_infor_id = (int)list[i]["tip_infor_id"];
					MapRuleParameter.Instance.SetMapRuleParameter(mapRuleParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		private void ReadDailyTrialMapRule()
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			string text = "Parameters/MapDailyTrial/daily_trial_map_rule_" + currentDayIndex;
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					DailyTrialMap mapRuleParameter = default(DailyTrialMap);
					mapRuleParameter.wave = (int)list[i]["wave"];
					mapRuleParameter.tower_0_level = (int)list[i]["tower_0_level"];
					mapRuleParameter.tower_1_level = (int)list[i]["tower_1_level"];
					mapRuleParameter.tower_2_level = (int)list[i]["tower_2_level"];
					mapRuleParameter.tower_3_level = (int)list[i]["tower_3_level"];
					mapRuleParameter.tower_4_level = (int)list[i]["tower_4_level"];
					mapRuleParameter.bonus_gold = (int)list[i]["bonus_gold"];
					mapRuleParameter.bonus_health = (int)list[i]["bonus_health"];
					mapRuleParameter.new_tower_event = (int)list[i]["new_tower_event"];
					mapRuleParameter.new_tower_id_slot_0 = (int)list[i]["new_tower_id_slot0"];
					mapRuleParameter.new_tower_id_slot_1 = (int)list[i]["new_tower_id_slot1"];
					mapRuleParameter.new_tower_id_slot_2 = (int)list[i]["new_tower_id_slot2"];
					MapRuleParameter.Instance.SetMapRuleParameter(mapRuleParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		public void ReadTournamentMapRule()
		{
			if (MapRuleParameter.Instance.IsMapRuleRead)
			{
				GameEventCenter.Instance.Trigger(GameEventType.OnTournamentMapRuleReceived, null);
			}
			else if (StaticMethod.IsInternetConnectionAvailable())
			{
				StartCoroutine(GetStoragedDataTournamentMapRule());
			}
		}

		private void GetEditorDataTournamentMapRule()
		{
			string file = "Parameters/MapTournament/map_tournament_rule";
			List<Dictionary<string, object>> list = CSVReader.Read(file);
			for (int i = 0; i < list.Count; i++)
			{
				TournamentMap mapRuleParameter = default(TournamentMap);
				mapRuleParameter.seasonID = (string)list[i]["seasonID"];
				mapRuleParameter.mapID = (int)list[i]["mapID"];
				mapRuleParameter.blessedHeroID = (int)list[i]["hero_blessed_id"];
				mapRuleParameter.powerupItemLimit = (int)list[i]["powerup_item_limit"];
				mapRuleParameter.tower_0_level = (int)list[i]["tower_0_level"];
				mapRuleParameter.tower_1_level = (int)list[i]["tower_1_level"];
				mapRuleParameter.tower_2_level = (int)list[i]["tower_2_level"];
				mapRuleParameter.tower_3_level = (int)list[i]["tower_3_level"];
				mapRuleParameter.tower_4_level = (int)list[i]["tower_4_level"];
				mapRuleParameter.wave_loop_begin = (int)list[i]["wave_loop_begin"];
				mapRuleParameter.wave_loop_end = (int)list[i]["wave_loop_end"];
				mapRuleParameter.health_increase_percentage_per_loop = (int)list[i]["health_increase_percentage_per_loop"];
				mapRuleParameter.damage_increase_percentage_per_loop = (int)list[i]["damage_increase_percentage_per_loop"];
				MapRuleParameter.Instance.SetMapRuleParameter(mapRuleParameter);
			}
			GameEventCenter.Instance.Trigger(GameEventType.OnTournamentMapRuleReceived, null);
		}

		private IEnumerator GetStoragedDataTournamentMapRule()
		{
			UnityWebRequest www = UnityWebRequest.Get(jsonDataTournamentMapRuleString);
			yield return www.SendWebRequest();
			if (www.isNetworkError)
			{
				UnityEngine.Debug.Log(www.error);
				yield break;
			}
			jsonDataTournamentMapRule = JsonMapper.ToObject<JsonData[]>(www.downloadHandler.text);
			for (int i = 0; i < jsonDataTournamentMapRule.Length; i++)
			{
				TournamentMap mapRuleParameter = default(TournamentMap);
				if (jsonDataTournamentMapRule[i].Keys.Contains("seasonID"))
				{
					mapRuleParameter.seasonID = (string)jsonDataTournamentMapRule[i]["seasonID"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("mapID"))
				{
					mapRuleParameter.mapID = (int)jsonDataTournamentMapRule[i]["mapID"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("hero_blessed_id"))
				{
					mapRuleParameter.blessedHeroID = (int)jsonDataTournamentMapRule[i]["hero_blessed_id"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("powerup_item_limit"))
				{
					mapRuleParameter.powerupItemLimit = (int)jsonDataTournamentMapRule[i]["powerup_item_limit"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("tower_0_level"))
				{
					mapRuleParameter.tower_0_level = (int)jsonDataTournamentMapRule[i]["tower_0_level"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("tower_1_level"))
				{
					mapRuleParameter.tower_1_level = (int)jsonDataTournamentMapRule[i]["tower_1_level"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("tower_2_level"))
				{
					mapRuleParameter.tower_2_level = (int)jsonDataTournamentMapRule[i]["tower_2_level"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("tower_3_level"))
				{
					mapRuleParameter.tower_3_level = (int)jsonDataTournamentMapRule[i]["tower_3_level"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("tower_4_level"))
				{
					mapRuleParameter.tower_4_level = (int)jsonDataTournamentMapRule[i]["tower_4_level"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("wave_loop_begin"))
				{
					mapRuleParameter.wave_loop_begin = (int)jsonDataTournamentMapRule[i]["wave_loop_begin"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("wave_loop_end"))
				{
					mapRuleParameter.wave_loop_end = (int)jsonDataTournamentMapRule[i]["wave_loop_end"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("health_increase_percentage_per_loop"))
				{
					mapRuleParameter.health_increase_percentage_per_loop = (int)jsonDataTournamentMapRule[i]["health_increase_percentage_per_loop"];
				}
				if (jsonDataTournamentMapRule[i].Keys.Contains("damage_increase_percentage_per_loop"))
				{
					mapRuleParameter.damage_increase_percentage_per_loop = (int)jsonDataTournamentMapRule[i]["damage_increase_percentage_per_loop"];
				}
				MapRuleParameter.Instance.SetMapRuleParameter(mapRuleParameter);
			}
			MapRuleParameter.Instance.IsMapRuleRead = true;
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForSeconds(0.5f);
			GameEventCenter.Instance.Trigger(GameEventType.OnTournamentMapRuleReceived, null);
		}

		public void ReadTournamentPriceConstants()
		{
			if (MapRuleParameter.Instance.IsPriceConstantRead)
			{
				GameEventCenter.Instance.Trigger(GameEventType.OnTournamentPriceConstantsReceived, null);
			}
			else if (StaticMethod.IsInternetConnectionAvailable())
			{
				StartCoroutine(GetStoragedDataTournamentPriceConstants());
			}
		}

		private IEnumerator GetStoragedDataTournamentPriceConstants()
		{
			UnityWebRequest rewardWWW = UnityWebRequest.Get(jsonDataTournamentRewardPath);
			yield return rewardWWW.SendWebRequest();
			if (rewardWWW.isNetworkError)
			{
				UnityEngine.Debug.Log(rewardWWW.error);
			}
			else
			{
				JsonData[] array = JsonMapper.ToObject<JsonData[]>(rewardWWW.downloadHandler.text);
				TournamentPrizeConfig tournamentPrizeConfig = new TournamentPrizeConfig();
				tournamentPrizeConfig.dataArray = new TournamentPrizeConfigData[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					TournamentPrizeConfigData tournamentPrizeConfigData = new TournamentPrizeConfigData();
					tournamentPrizeConfigData.Leagueindex = (int)array[i]["LeagueIndex"];
					tournamentPrizeConfigData.Rankrangelower = (int)array[i]["RankRangeLower"];
					tournamentPrizeConfigData.Rankrangeupper = (int)array[i]["RankRangeUpper"];
					tournamentPrizeConfigData.Itemtypes = (string)array[i]["ItemTypes"];
					tournamentPrizeConfigData.Itemquantities = GameTools.DecodeStringToIntArray((string)array[i]["ItemQuantities"]);
					tournamentPrizeConfig.dataArray[i] = tournamentPrizeConfigData;
				}
				CommonData.Instance.tournamentPrizeConfig = tournamentPrizeConfig;
				UnityEngine.Debug.Log("read prize data successfully " + tournamentPrizeConfig.dataArray[tournamentPrizeConfig.dataArray.Length - 1].Itemtypes);
			}
			UnityWebRequest www = UnityWebRequest.Get(jsonDataTournamentPriceConstantPath);
			yield return www.SendWebRequest();
			if (www.isNetworkError)
			{
				UnityEngine.Debug.Log(www.error);
				yield break;
			}
			JsonData[] result = JsonMapper.ToObject<JsonData[]>(www.downloadHandler.text);
			for (int j = 0; j < result.Length; j++)
			{
				TournamentPriceConstant tournamentPriceConstant = new TournamentPriceConstant();
				int leagueIndex = 0;
				if (result[j].Keys.Contains("LeagueIndex"))
				{
					leagueIndex = (int)result[j]["LeagueIndex"];
				}
				if (result[j].Keys.Contains("Jfactor"))
				{
					float.TryParse(result[j]["Jfactor"].ToString(), out tournamentPriceConstant.jFactor);
				}
				if (result[j].Keys.Contains("InitGemQuantity"))
				{
					tournamentPriceConstant.initGemQuantity = (int)result[j]["InitGemQuantity"];
				}
				MapRuleParameter.Instance.AddTournamentConstants(leagueIndex, tournamentPriceConstant);
			}
			MapRuleParameter.Instance.IsPriceConstantRead = true;
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			GameEventCenter.Instance.Trigger(GameEventType.OnTournamentPriceConstantsReceived, null);
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
		}
	}
}
