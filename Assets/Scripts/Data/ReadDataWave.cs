using Gameplay;
using GeneralVariable;
using LifetimePopup;
using LitJson;
using Middle;
using Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Data
{
	public class ReadDataWave : SingletonMonoBehaviour<ReadDataWave>
	{
		private List<Dictionary<string, object>> data;

		[Header("Tournament data")]
		[SerializeField]
		private string[] jsonDataTournamentMapString;

		[Space]
		[SerializeField]
		private string jsonDataTournamentMapRuleString;

		private JsonData[] jsonDataTournamentMap;

		private JsonData[] jsonDataTournamentMapRule;

		private float connectionTimeOut;

		private float timeOutTracking;

		private bool isTrackingTimeOut;

		private bool isReceivedWaveData;

		private void Start()
		{
			string empty = string.Empty;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				empty = "Parameters/MapCampaign/map_" + SingletonMonoBehaviour<GameData>.Instance.MapID;
				if (PlayerPrefs.GetInt("TESTcustomCSV", 0) > 0)
				{
					empty = "Parameters/custom";
				}
				ReadWaveData(empty);
				Loading.Instance.LoadSceneCompleted();
				break;
			case GameMode.DailyTrialMode:
			{
				int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
				empty = "Parameters/MapDailyTrial/map_daily_" + currentDayIndex;
				if (PlayerPrefs.GetInt("TESTcustomCSV", 0) > 0)
				{
					empty = "Parameters/custom";
				}
				ReadWaveData(empty);
				Loading.Instance.LoadSceneCompleted();
				break;
			}
			case GameMode.TournamentMode:
				ReadTournamentWaveData();
				break;
			}
		}

		private void Update()
		{
			if (isTrackingTimeOut)
			{
				if (timeOutTracking == 0f)
				{
					CheckLoadingStatus();
				}
				timeOutTracking = Mathf.MoveTowards(timeOutTracking, 0f, Time.deltaTime);
			}
		}

		private void ReadWaveData(string filePath)
		{
			int battleLevel = (int)MiddleDelivery.Instance.BattleLevel;
			try
			{
				if (data == null)
				{
					data = CSVReader.Read(filePath);
				}
				SingletonMonoBehaviour<GameData>.Instance.TotalHealth = (int)data[0]["health"];
				SingletonMonoBehaviour<GameData>.Instance.CurrentHealth = SingletonMonoBehaviour<GameData>.Instance.TotalHealth;
				SingletonMonoBehaviour<GameData>.Instance.Money = (int)data[0]["money"];
				SingletonMonoBehaviour<GameData>.Instance.DeltaTimeWave = (int)data[0]["delta_time"];
				for (int i = 0; i < data.Count; i++)
				{
					int wave = (int)data[i]["wave"];
					int num = (int)data[i]["time"];
					int enemyID = (int)data[i]["enemy_id"];
					int num2 = (int)data[i]["gate"];
					int num3 = (int)data[i]["formation_id"];
					int num4 = (int)data[i]["min_difficulty"];
					if (battleLevel >= num4)
					{
						if (num3 == 0)
						{
							SingletonMonoBehaviour<GameData>.Instance.AddWavesEnemyData(wave, num, enemyID, num2, num3, num2);
						}
						else
						{
							FormationConfigData formationConfigData = CommonData.Instance.formationData[num3];
							for (int j = 0; j < formationConfigData.times.Count; j++)
							{
								SingletonMonoBehaviour<GameData>.Instance.AddWavesEnemyData(wave, num + (int)(formationConfigData.times[j] * 1000f), enemyID, num2, num3, num2);
							}
						}
					}
				}
				SingletonMonoBehaviour<GameData>.Instance.PostprocessWavesEnemyData();
				InitPoolObjects();
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
				throw;
			}
		}

		private void ReadTournamentWaveData()
		{
			StartCoroutine(GetStoragedDataTournamentMap());
		}

		private void GetEditorDataTournamentMap()
		{
			int currentSeasonMapID = MapRuleParameter.Instance.GetCurrentSeasonMapID();
			string filePath = "Parameters/MapTournament/map_tournament_" + currentSeasonMapID;
			ReadWaveData(filePath);
		}

		private IEnumerator GetStoragedDataTournamentMap()
		{
			timeOutTracking = GeneralVariable.GeneralVariable.CONNECTION_TIMEOUT;
			isTrackingTimeOut = true;
			isReceivedWaveData = false;
			int mapIDTournament = MapRuleParameter.Instance.GetCurrentSeasonMapID();
			UnityWebRequest www = UnityWebRequest.Get(jsonDataTournamentMapString[mapIDTournament]);
			yield return www.SendWebRequest();
			if (www.isNetworkError)
			{
				UnityEngine.Debug.Log(www.error);
				yield break;
			}
			UnityEngine.Debug.Log(www.downloadHandler.text);
			jsonDataTournamentMap = JsonMapper.ToObject<JsonData[]>(www.downloadHandler.text);
			int battleLevel = (int)MiddleDelivery.Instance.BattleLevel;
			SingletonMonoBehaviour<GameData>.Instance.TotalHealth = (int)jsonDataTournamentMap[0]["health"];
			SingletonMonoBehaviour<GameData>.Instance.CurrentHealth = SingletonMonoBehaviour<GameData>.Instance.TotalHealth;
			SingletonMonoBehaviour<GameData>.Instance.Money = (int)jsonDataTournamentMap[0]["money"];
			SingletonMonoBehaviour<GameData>.Instance.DeltaTimeWave = (int)jsonDataTournamentMap[0]["delta_time"];
			for (int i = 0; i < jsonDataTournamentMap.Length; i++)
			{
				int wave = (int)jsonDataTournamentMap[i]["wave"];
				int num = (int)jsonDataTournamentMap[i]["time"];
				int enemyID = (int)jsonDataTournamentMap[i]["enemy_id"];
				int num2 = (int)jsonDataTournamentMap[i]["gate"];
				int num3 = (int)jsonDataTournamentMap[i]["formation_id"];
				int num4 = (int)jsonDataTournamentMap[i]["min_difficulty"];
				if (battleLevel < num4)
				{
					continue;
				}
				if (num3 == 0)
				{
					SingletonMonoBehaviour<GameData>.Instance.AddWavesEnemyData(wave, num, enemyID, num2, num3, num2);
					continue;
				}
				FormationConfigData formationConfigData = CommonData.Instance.formationData[num3];
				for (int j = 0; j < formationConfigData.times.Count; j++)
				{
					SingletonMonoBehaviour<GameData>.Instance.AddWavesEnemyData(wave, num + (int)(formationConfigData.times[j] * 1000f), enemyID, num2, num3, num2);
				}
			}
			SingletonMonoBehaviour<GameData>.Instance.PostprocessWavesEnemyData();
			InitPoolObjects();
			Loading.Instance.LoadSceneCompleted();
			GameplayManager.Instance.endlessModeManager.Init();
			isReceivedWaveData = true;
		}

		private void CheckLoadingStatus()
		{
			if (isReceivedWaveData)
			{
				UnityEngine.Debug.Log("Time out, get data completed!");
			}
			else
			{
				UnityEngine.Debug.Log("Time out, get data failed!");
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(150);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
				Loading.Instance.ShowLoading();
				Invoke("DoLoad", 1f);
			}
			isTrackingTimeOut = false;
			isReceivedWaveData = false;
		}

		private void DoLoad()
		{
			ModeManager.Instance.gameMode = GameMode.CampaignMode;
			GameApplication.Instance.LoadScene(GameApplication.WorldMapSceneName);
		}

		private void InitPoolObjects()
		{
			SingletonMonoBehaviour<SpawnEnemy>.Instance.InitPoolEnemies();
			SingletonMonoBehaviour<SpawnTower>.Instance.InitPoolTowers();
			GameplayManager.Instance.LoadMap();
			SingletonMonoBehaviour<SpawnAlly>.Instance.InitPoolHeroes();
			SingletonMonoBehaviour<SpawnAlly>.Instance.InitHeroesStartPosition();
			InMapStartWaveButtonsManager.Instance.InitListButtons();
			InMapStartWaveButtonsManager.Instance.ShowListButtonOnStart();
		}
	}
}
