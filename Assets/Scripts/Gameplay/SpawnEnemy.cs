using DG.Tweening;
using GeneralVariable;
using Middle;
using Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class SpawnEnemy : SingletonMonoBehaviour<SpawnEnemy>
	{
		[SerializeField]
		private int mapIDTutorial;

		[SerializeField]
		private int waveIDTutorialHeroSkill;

		[Space]
		private Vector3 PoolPosition = new Vector3(1000f, 100f, 0f);

		[NonSerialized]
		public int currentGateSnail;

		[NonSerialized]
		public int currentGateWolfman;

		[NonSerialized]
		public int currentGateGoldenSnail;

		private bool isDispatchEvent_TutorialHeroSkill;

		private int count;

		public event Action onDispatchEventTutorialHeroSkill;

		public void InitPoolEnemies()
		{
			for (int i = 0; i < SingletonMonoBehaviour<GameData>.Instance.ListEnemyID.Count; i++)
			{
				EnemyModel enemyModel = null;
				enemyModel = UnityEngine.Object.Instantiate(Resources.Load<EnemyModel>($"Enemies/enemy_{SingletonMonoBehaviour<GameData>.Instance.ListEnemyID[i]}"));
				enemyModel.gameObject.SetActive(value: false);
				TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
				trashManRecycleBin.prefab = enemyModel.gameObject;
				trashManRecycleBin.instancesToPreallocate = 0;
				TrashManRecycleBin recycleBin = trashManRecycleBin;
				TrashMan.manageRecycleBin(recycleBin);
				TrashMan.despawn(enemyModel.gameObject);
			}
			TrashMan.InitPool(GeneralVariable.GeneralVariable.SELECT_ENEMY_INDICATOR_PATH);
		}

		public void InitAdditionEnemy(int enemyID)
		{
			EnemyModel enemyModel = null;
			enemyModel = UnityEngine.Object.Instantiate(Resources.Load<EnemyModel>($"Enemies/enemy_{enemyID}"));
			enemyModel.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = enemyModel.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(enemyModel.gameObject);
		}

		public void StartWaveNormal(int wave)
		{
			List<EnemyData> listEnemyWithWave = SingletonMonoBehaviour<GameData>.Instance.GetListEnemyWithWave(wave);
			int num = listEnemyWithWave.Count;
			for (int i = 0; i < num; i++)
			{
				bool lastEnemyInBattle = false;
				bool flag = false;
				EnemyData enemyData = listEnemyWithWave[i];
				int time = enemyData.time;
				int id = enemyData.id;
				int gate = enemyData.gate;
				if (i + 1 == num && wave + 1 >= SingletonMonoBehaviour<GameData>.Instance.TotalWave)
				{
					lastEnemyInBattle = true;
				}
				flag = enemyData.isLastInWave;
				StartCoroutine(Spawn(wave, time, id, MiddleDelivery.Instance.BattleLevel, gate, lastEnemyInBattle, flag));
			}
		}

		private IEnumerator Spawn(int wave, int time, int id, BattleLevel level, int gate, bool lastEnemyInBattle, bool lastEnemyInWave)
		{
			yield return new WaitForSeconds((float)time / 1000f);
			if (SingletonMonoBehaviour<GameData>.Instance.MapID == mapIDTutorial && wave == waveIDTutorialHeroSkill && !isDispatchEvent_TutorialHeroSkill)
			{
				if (this.onDispatchEventTutorialHeroSkill != null)
				{
					this.onDispatchEventTutorialHeroSkill();
				}
				isDispatchEvent_TutorialHeroSkill = true;
			}
			if (id == 9)
			{
				currentGateWolfman = gate;
			}
			if (id == 10)
			{
				currentGateSnail = gate;
			}
			if (id == 12)
			{
				currentGateGoldenSnail = gate;
			}
			if (EnemyParameterManager.Instance.IsBoss(id))
			{
				yield return new WaitUntil(SingletonMonoBehaviour<GameData>.Instance.IsNoEnemyLeft);
				UnityEngine.Debug.Log("boss");
			}
			EnemyModel enemy = Get(id);
			enemy.gameObject.SetActive(value: true);
			enemy.gameObject.transform.localScale = Vector3.one;
			enemy.gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
			enemy.SetDataStartRun(id, (int)level, gate, UnityEngine.Random.Range(0, Config.Instance.LineCount));
			enemy.GetFSMController();
			SetLastEnemyInBattle(lastEnemyInBattle);
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				if (!lastEnemyInBattle && lastEnemyInWave && wave < SingletonMonoBehaviour<GameData>.Instance.TotalWave - 1)
				{
					GameplayManager.Instance.ShowStartWaveButton(wave + 1, EnemyParameterManager.Instance.getListEnemyGate(wave + 1));
				}
				break;
			case GameMode.DailyTrialMode:
				if (!lastEnemyInBattle && lastEnemyInWave && wave < SingletonMonoBehaviour<GameData>.Instance.TotalWave - 1)
				{
					GameplayManager.Instance.ShowStartWaveButton(wave + 1, EnemyParameterManager.Instance.getListEnemyGate(wave + 1));
				}
				break;
			case GameMode.TournamentMode:
				if (!lastEnemyInWave)
				{
					break;
				}
				if (!GameplayManager.Instance.endlessModeManager.IsLastEnemyInNormalWave)
				{
					if (wave + 1 >= SingletonMonoBehaviour<GameData>.Instance.TotalWave)
					{
						wave = GameplayManager.Instance.endlessModeManager.waveLoopBegin - 1;
					}
					GameplayManager.Instance.ShowStartWaveButton(wave + 1, EnemyParameterManager.Instance.getListEnemyGate(wave + 1));
				}
				else
				{
					if (wave >= GameplayManager.Instance.endlessModeManager.waveLoopEnd)
					{
						wave = GameplayManager.Instance.endlessModeManager.waveLoopBegin - 1;
					}
					GameplayManager.Instance.ShowStartWaveButton(wave + 1, EnemyParameterManager.Instance.getListEnemyGate(wave + 1));
				}
				break;
			}
		}

		private void SetLastEnemyInBattle(bool lastEnemyInBattle)
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				if (lastEnemyInBattle)
				{
					SingletonMonoBehaviour<GameData>.Instance.IsLastEnemyInBattle = lastEnemyInBattle;
				}
				break;
			case GameMode.DailyTrialMode:
				if (lastEnemyInBattle)
				{
					SingletonMonoBehaviour<GameData>.Instance.IsLastEnemyInBattle = lastEnemyInBattle;
				}
				break;
			case GameMode.TournamentMode:
				SingletonMonoBehaviour<GameData>.Instance.IsLastEnemyInBattle = false;
				break;
			}
			if (lastEnemyInBattle)
			{
				GameplayManager.Instance.endlessModeManager.SetLastEnemyInNormalWave(lastEnemyInBattle);
			}
		}

		public void SpawnAdditionEnemyAtGate(int id, float fullPos, int gate, float randomPosition)
		{
			EnemyModel enemyModel = Get(id);
			enemyModel.SetDataStartRun(id, (int)MiddleDelivery.Instance.BattleLevel, gate, UnityEngine.Random.Range(0, Config.Instance.LineCount), enemyModel.EnemyMovementController.MoveToPosition(fullPos, randomPosition));
			enemyModel.GetFSMController();
		}

		public EnemyModel Get(int id)
		{
			if (id < 0)
			{
				UnityEngine.Debug.LogError("Input ID < 0");
				return null;
			}
			EnemyModel enemyModel = null;
			string gameObjectName = $"enemy_{id}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			enemyModel = gameObject.GetComponent<EnemyModel>();
			enemyModel.transform.parent = base.transform;
			SingletonMonoBehaviour<GameData>.Instance.AddEnemyToListActiveEnemy(enemyModel);
			return enemyModel;
		}

		public void Push(EnemyModel enemy, float delayTime)
		{
			StartCoroutine(IEPushEnemy(enemy, delayTime));
		}

		public IEnumerator IEPushEnemy(EnemyModel enemy, float delayTime)
		{
			yield return new WaitForSeconds(delayTime);
			enemy.transform.DOKill();
			enemy.transform.position = PoolPosition;
			enemy.gameObject.SetActive(value: false);
			TrashMan.despawn(enemy.gameObject);
		}
	}
}
