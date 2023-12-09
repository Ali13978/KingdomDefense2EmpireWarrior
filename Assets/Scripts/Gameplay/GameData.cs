using CodeStage.AntiCheat.ObscuredTypes;
using Data;
using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class GameData : SingletonMonoBehaviour<GameData>
	{
		private List<int> listEnemyID = new List<int>();

		private List<List<EnemyData>> listAllEnemyWave = new List<List<EnemyData>>();

		private List<EnemyModel> listActiveEnemy = new List<EnemyModel>();

		private List<EnemyModel> listOnScreenEnemy = new List<EnemyModel>();

		private List<TowerModel> listActiveTower = new List<TowerModel>();

		private List<CharacterModel> listActiveAlly = new List<CharacterModel>();

		private List<int> listHeroesIdsSelected = new List<int>();

		private bool recordingPosition;

		private TowerModel currentTowerSelected;

		private bool playerKnowHowToUseSkill;

		private ObscuredInt _money;

		private ObscuredInt _currentHealth;

		[SerializeField]
		private PowerupItemData powerupItemData;

		public static float PIXEL_PER_UNIT = 80f;

		public static float REVERSE_PIXEL_PER_UNIT = 1f / PIXEL_PER_UNIT;

		private bool playedGameplayVideo_ForMoney;

		private bool playedGameplayVideo_ForLife;

		private bool isPlayingVideoAds;

		private bool playedVideoEndGame;

		private bool playedVideoLucky;

		private bool playedVideoGems;

		public List<int> ListEnemyID
		{
			get
			{
				return listEnemyID;
			}
			set
			{
				listEnemyID = value;
			}
		}

		public List<EnemyModel> ListActiveEnemy
		{
			get
			{
				return listActiveEnemy;
			}
			set
			{
				listActiveEnemy = value;
			}
		}

		public List<EnemyModel> ListOnScreenEnemy
		{
			get
			{
				return listOnScreenEnemy;
			}
			set
			{
				listOnScreenEnemy = value;
			}
		}

		public List<TowerModel> ListActiveTower
		{
			get
			{
				return listActiveTower;
			}
			set
			{
				listActiveTower = value;
			}
		}

		public List<CharacterModel> ListActiveAlly
		{
			get
			{
				return listActiveAlly;
			}
			private set
			{
				listActiveAlly = value;
			}
		}

		public List<int> ListHeroesIdsSelected
		{
			get
			{
				return listHeroesIdsSelected;
			}
			set
			{
				listHeroesIdsSelected = value;
			}
		}

		public bool RecordingPosition
		{
			get
			{
				return recordingPosition;
			}
			set
			{
				recordingPosition = value;
			}
		}

		public TowerModel CurrentTowerSelected
		{
			get
			{
				return currentTowerSelected;
			}
			set
			{
				currentTowerSelected = value;
			}
		}

		public bool PlayerKnowHowToUseSkill
		{
			get
			{
				return playerKnowHowToUseSkill;
			}
			set
			{
				playerKnowHowToUseSkill = value;
			}
		}

		public int MapID => MiddleDelivery.Instance.MapIDSelected;

		public bool IsGameStart
		{
			get;
			set;
		}

		public bool IsGameOver
		{
			get;
			set;
		}

		public bool IsAnyTutorialPopupOpen
		{
			get;
			set;
		}

		public bool IsAnyPopupOpen
		{
			get;
			set;
		}

		public int Money
		{
			get
			{
				return (int)_money - GameTools.deltaValue;
			}
			set
			{
				_money = value + GameTools.deltaValue;
			}
		}

		public int TotalEnemy
		{
			get;
			set;
		}

		public int GameplayGem
		{
			get;
			set;
		}

		public int TotalExp
		{
			get;
			set;
		}

		public int CurrentHealth
		{
			get
			{
				return (int)_currentHealth - GameTools.deltaValue;
			}
			set
			{
				_currentHealth = value + GameTools.deltaValue;
			}
		}

		public int TotalHealth
		{
			get;
			set;
		}

		public bool IsPause
		{
			get;
			set;
		}

		public int TotalWave
		{
			get;
			set;
		}

		public int CurrentWave
		{
			get;
			set;
		}

		public bool IsLastEnemyInBattle
		{
			get;
			set;
		}

		public int DeltaTimeWave
		{
			get;
			set;
		}

		public bool IsVictory
		{
			get;
			set;
		}

		public int CurrentOpenChestTurn
		{
			get;
			set;
		}

		public int CurrentOpenChestOffer
		{
			get;
			set;
		}

		public float tournamentBattleTime
		{
			get;
			set;
		}

		public int PlayCountMapInCampaign
		{
			get;
			set;
		}

		public PowerupItemData PowerupItemData
		{
			get
			{
				return powerupItemData;
			}
			set
			{
				powerupItemData = value;
			}
		}

		public bool PlayedGameplayVideo_ForMoney
		{
			get
			{
				return playedGameplayVideo_ForMoney;
			}
			set
			{
				playedGameplayVideo_ForMoney = value;
			}
		}

		public bool PlayedGameplayVideo_ForLife
		{
			get
			{
				return playedGameplayVideo_ForLife;
			}
			set
			{
				playedGameplayVideo_ForLife = value;
			}
		}

		public bool IsPlayingVideoAds
		{
			get
			{
				return isPlayingVideoAds;
			}
			set
			{
				isPlayingVideoAds = value;
			}
		}

		public bool PlayedVideoEndGame
		{
			get
			{
				return playedVideoEndGame;
			}
			set
			{
				playedVideoEndGame = value;
			}
		}

		public bool PlayedVideoLucky
		{
			get
			{
				return playedVideoLucky;
			}
			set
			{
				playedVideoLucky = value;
			}
		}

		public bool PlayedVideoGems
		{
			get
			{
				return playedVideoGems;
			}
			set
			{
				playedVideoGems = value;
			}
		}

		public event Action<TowerModel> OnTowerAdded;

		public event Action<TowerModel, bool> OnTowerRemoved;

		public event Action OnMoneyChange;

		public event Action OnOpenChestTurnChange;

		private void resetDataPlayVideo()
		{
			PlayedGameplayVideo_ForMoney = false;
			PlayedGameplayVideo_ForLife = false;
			PlayedVideoEndGame = false;
			IsPlayingVideoAds = false;
			playedVideoLucky = false;
		}

		private void Awake()
		{
			SetDefaultGameData();
			InitListHeroIDSelected();
		}

		private void Update()
		{
			if (ModeManager.Instance.gameMode == GameMode.TournamentMode && IsGameStart && !IsGameOver && !IsPause)
			{
				TournamentBattleTimeCount();
			}
		}

		private void TournamentBattleTimeCount()
		{
			tournamentBattleTime += Time.deltaTime;
		}

		public void SetDefaultGameData()
		{
			IsAnyPopupOpen = false;
			IsAnyTutorialPopupOpen = false;
			PlayerKnowHowToUseSkill = false;
			IsGameStart = false;
			IsGameOver = false;
			GameplayGem = 0;
			TotalExp = 0;
			IsPause = false;
			IsVictory = false;
			TotalEnemy = 0;
			CurrentWave = 0;
			IsLastEnemyInBattle = false;
			CurrentOpenChestTurn = 3;
			CurrentOpenChestOffer = 2;
			resetDataPlayVideo();
			if ((bool)PowerupItemData)
			{
				PowerupItemData.InitValue();
			}
			if (ModeManager.Instance.gameMode == GameMode.CampaignMode)
			{
				PlayCountMapInCampaign = ReadWriteDataMap.Instance.GetCurrentPlayCount(MapID);
			}
			GameEventCenter.Instance.UnsubscribeIngameEvent();
			if (EventQuestSystem.Instance != null)
			{
				EventQuestSystem.Instance.SaveEvent();
			}
			tournamentBattleTime = 0f;
		}

		private void InitListHeroIDSelected()
		{
			foreach (int item in MiddleDelivery.Instance.ListHeroesIdsSelected)
			{
				ListHeroesIdsSelected.Add(item);
			}
		}

		public bool IsListActiveEnemyContainThis(EnemyModel enemyModel)
		{
			return ListActiveEnemy.Contains(enemyModel);
		}

		public void AddEnemyToListActiveEnemy(EnemyModel enemyModel)
		{
			if (!IsListActiveEnemyContainThis(enemyModel))
			{
				ListActiveEnemy.Add(enemyModel);
			}
		}

		public void RemoveEnemyFromListActiveEnemy(EnemyModel enemyModel)
		{
			if (IsListActiveEnemyContainThis(enemyModel))
			{
				listActiveEnemy.Remove(enemyModel);
			}
		}

		public void SetWavesEnemyData(int wave, int no, int time, int enemyID, bool isLastEnemyInWave, int gate, int formationId, int minDifficulty)
		{
			EnemyData item = new EnemyData(wave, time, enemyID, isLastEnemyInWave, gate, formationId, minDifficulty);
			if (wave >= listAllEnemyWave.Count)
			{
				List<EnemyData> list = new List<EnemyData>();
				list.Insert(no, item);
				listAllEnemyWave.Insert(wave, list);
			}
			else
			{
				List<EnemyData> list2 = listAllEnemyWave[wave];
				list2.Insert(no, item);
			}
			TotalWave = wave + 1;
			CreateListEnemyID(enemyID);
		}

		public void AddWavesEnemyData(int wave, int time, int enemyID, int gate, int formationId, int minDifficulty)
		{
			EnemyData item = new EnemyData(wave, time, enemyID, isLastInWave: false, gate, formationId, minDifficulty);
			if (wave >= listAllEnemyWave.Count)
			{
				listAllEnemyWave.Add(new List<EnemyData>());
			}
			listAllEnemyWave[wave].Add(item);
			TotalWave = wave + 1;
			CreateListEnemyID(enemyID);
		}

		public void PostprocessWavesEnemyData()
		{
			for (int i = 0; i < listAllEnemyWave.Count; i++)
			{
				int index = 0;
				for (int j = 1; j < listAllEnemyWave[i].Count; j++)
				{
					EnemyData enemyData = listAllEnemyWave[i][j];
					int time = enemyData.time;
					EnemyData enemyData2 = listAllEnemyWave[i][index];
					if (time > enemyData2.time)
					{
						index = j;
					}
				}
				EnemyData value = listAllEnemyWave[i][index];
				value.isLastInWave = true;
				listAllEnemyWave[i][index] = value;
			}
		}

		public List<EnemyData> GetListEnemyWithWave(int wave)
		{
			if (wave >= listAllEnemyWave.Count)
			{
				return null;
			}
			return listAllEnemyWave[wave];
		}

		public List<int> GetListEnemyIDWithWave(int wave)
		{
			List<int> list = new List<int>();
			foreach (EnemyData item in listAllEnemyWave[wave])
			{
				EnemyData current = item;
				if (!list.Contains(current.id))
				{
					list.Add(current.id);
				}
			}
			return list;
		}

		public int GetTotalEnemyInWave(int enemyID, int wave)
		{
			int num = 0;
			foreach (EnemyData item in listAllEnemyWave[wave])
			{
				EnemyData current = item;
				if (current.id == enemyID)
				{
					num++;
				}
			}
			return num;
		}

		private void CreateListEnemyID(int id)
		{
			if (!ListEnemyID.Contains(id))
			{
				ListEnemyID.Add(id);
			}
		}

		public void ClearListEnemyID()
		{
			ListEnemyID.Clear();
		}

		public void GetInRangeEnemies(Vector2 centerPoint, float range, List<EnemyModel> inRangeEnemies)
		{
			inRangeEnemies.Clear();
			float num = range * range;
			for (int i = 0; i < ListActiveEnemy.Count; i++)
			{
				EnemyModel enemyModel = ListActiveEnemy[i];
				if (Vector2.SqrMagnitude((Vector2)enemyModel.transform.position - centerPoint) <= num && !enemyModel.IsUnderground)
				{
					inRangeEnemies.Add(enemyModel);
				}
			}
		}

		public bool IsNoEnemyLeft()
		{
			return ListActiveEnemy.Count == 0;
		}

		private void SetListEnemyOnScreen()
		{
			ListOnScreenEnemy.Clear();
			if (ListActiveEnemy.Count > 0)
			{
				foreach (EnemyModel item in ListActiveEnemy)
				{
					Vector3 position = item.transform.position;
					if (position.x < 6f)
					{
						Vector3 position2 = item.transform.position;
						if (position2.x > -6f)
						{
							Vector3 position3 = item.transform.position;
							if (position3.y < 4f)
							{
								Vector3 position4 = item.transform.position;
								if (position4.x > -4f)
								{
									ListOnScreenEnemy.Add(item);
								}
							}
						}
					}
				}
			}
		}

		public EnemyModel getEnemyWithHighestHealth(bool isFlyEnemy, bool isUndergroundEnemy)
		{
			EnemyModel enemyModel = null;
			int num = 0;
			SetListEnemyOnScreen();
			if (ListOnScreenEnemy.Count > 0)
			{
				enemyModel = ListOnScreenEnemy[0];
				num = 0;
				for (int i = 0; i < ListOnScreenEnemy.Count; i++)
				{
					if (enemyModel.EnemyHealthController.CurrentHealth < ListOnScreenEnemy[i].EnemyHealthController.CurrentHealth && ListOnScreenEnemy[i].IsAir == isFlyEnemy && ListOnScreenEnemy[i].IsUnderground == isUndergroundEnemy)
					{
						enemyModel = ListOnScreenEnemy[i];
						num = i;
					}
				}
			}
			if ((bool)enemyModel && enemyModel.IsAir && !isFlyEnemy)
			{
				return null;
			}
			if ((bool)enemyModel && enemyModel.IsUnderground && !isUndergroundEnemy)
			{
				return null;
			}
			return enemyModel;
		}

		public void AddTowerToActiveList(TowerModel towerModel)
		{
			ListActiveTower.Add(towerModel);
			if (this.OnTowerAdded != null)
			{
				this.OnTowerAdded(towerModel);
			}
		}

		public void RemoveTowerFromActiveList(TowerModel towerModel, bool isSold)
		{
			ListActiveTower.Remove(towerModel);
			if (this.OnTowerRemoved != null)
			{
				this.OnTowerRemoved(towerModel, isSold);
			}
		}

		public void GetInRangeTowers(Vector2 centerPoint, float range, List<TowerModel> towers, List<TowerModel> inRangeTowers)
		{
			inRangeTowers.Clear();
			float num = range * range;
			for (int i = 0; i < towers.Count; i++)
			{
				TowerModel towerModel = towers[i];
				if (Vector2.SqrMagnitude((Vector2)towerModel.transform.position - centerPoint) <= num)
				{
					inRangeTowers.Add(towerModel);
				}
			}
		}

		public void GetNearestTowers(Vector2 centerPoint, int maxTowers, List<TowerModel> towers, List<TowerModel> nearestTowers, List<float> squaredDistances)
		{
			nearestTowers.Clear();
			squaredDistances.Clear();
			for (int i = 0; i < towers.Count; i++)
			{
				TowerModel towerModel = towers[i];
				if (nearestTowers.Count < maxTowers)
				{
					nearestTowers.Add(towerModel);
					squaredDistances.Add(Vector2.SqrMagnitude((Vector2)towerModel.CachedPosition - centerPoint));
					continue;
				}
				float num = Vector2.SqrMagnitude((Vector2)towerModel.CachedPosition - centerPoint);
				for (int j = 0; j < maxTowers; j++)
				{
					if (num < squaredDistances[j])
					{
						squaredDistances[j] = num;
						nearestTowers[j] = towerModel;
						break;
					}
				}
			}
		}

		public void GetInRangeTowers(Vector2 centerPoint, float range, List<TowerModel> inRangeTowers)
		{
			GetInRangeTowers(centerPoint, range, ListActiveTower, inRangeTowers);
		}

		[Obsolete]
		public List<TowerModel> GetNearestTowers(Vector2 centerPoint, int maxTowers, List<TowerModel> towers)
		{
			List<TowerModel> list = new List<TowerModel>();
			GetNearestTowers(centerPoint, maxTowers, towers, list, new List<float>());
			return list;
		}

		public bool IsInRange(GameObject source, GameObject target, float sqrMaxRange, float sqrMinRange)
		{
			float num = SqrDistance(source, target);
			bool flag = false;
			bool flag2 = false;
			if (num <= sqrMaxRange)
			{
				flag = true;
			}
			if (sqrMinRange <= num)
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				return true;
			}
			return false;
		}

		public float SqrDistance(GameObject source, GameObject target)
		{
			return (source.transform.position - target.transform.position).sqrMagnitude;
		}

		public float SqrDistance(Vector3 p1, Vector3 p2)
		{
			return (p1 - p2).sqrMagnitude;
		}

		public void IncreaseMoney(ObscuredInt amount)
		{
			Money += amount;
			if ((int)amount > 0)
			{
				GameEventCenter.Instance.Trigger(GameEventType.EventEarnGold, new EventTriggerData(EventTriggerType.EarnGold, amount));
			}
			if (this.OnMoneyChange != null)
			{
				this.OnMoneyChange();
			}
		}

		public void DecreaseMoney(ObscuredInt amount)
		{
			Money -= amount;
			if (this.OnMoneyChange != null)
			{
				this.OnMoneyChange();
			}
		}

		public void ChangeOpenChestTurn(int amount)
		{
			CurrentOpenChestTurn += amount;
			if (this.OnOpenChestTurnChange != null)
			{
				this.OnOpenChestTurnChange();
			}
		}

		public void ChangeOpenChestOffer()
		{
			CurrentOpenChestOffer--;
		}

		public bool isAvailableOpenChestTurn()
		{
			return CurrentOpenChestTurn >= 1;
		}

		public bool isAvailableOpenChestOffer()
		{
			return CurrentOpenChestOffer >= 1;
		}

		public int GetActuallyGemAmount()
		{
			UnityEngine.Debug.Log("Tổng số gem nhận đc là = " + GameplayGem);
			return GameplayGem;
		}
	}
}
