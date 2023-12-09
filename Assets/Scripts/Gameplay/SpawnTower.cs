using Data;
using Middle;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class SpawnTower : SingletonMonoBehaviour<SpawnTower>
	{
		private Vector3 PoolPosition = new Vector3(1000f, 100f, 0f);

		public void InitPoolTowers()
		{
			int numberOfTower = TowerParameter.Instance.GetNumberOfTower();
			for (int i = 0; i < numberOfTower; i++)
			{
				int num = 0;
				switch (ModeManager.Instance.gameMode)
				{
				case GameMode.CampaignMode:
					num = MapRuleParameter.Instance.GetMaxLevelTowerCanUpgrade_Campaign(SingletonMonoBehaviour<GameData>.Instance.MapID, i);
					break;
				case GameMode.DailyTrialMode:
				{
					int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
					num = DailyTrialParameter.Instance.GetMaxLevelTowerCanUpgrade(currentDayIndex, i);
					break;
				}
				case GameMode.TournamentMode:
				{
					string currentSeasonID = MapRuleParameter.Instance.GetCurrentSeasonID();
					num = MapRuleParameter.Instance.GetMaxLevelTowerCanUpgrade_Tournament(currentSeasonID, i);
					break;
				}
				}
				for (int j = 0; j <= num; j++)
				{
					string arg = $"tower_{i}_{j}";
					TowerModel towerModel = null;
					towerModel = Object.Instantiate(Resources.Load<TowerModel>($"Towers/{arg}"));
					towerModel.gameObject.SetActive(value: false);
					TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
					trashManRecycleBin.prefab = towerModel.gameObject;
					trashManRecycleBin.instancesToPreallocate = 0;
					TrashManRecycleBin recycleBin = trashManRecycleBin;
					TrashMan.manageRecycleBin(recycleBin);
					TrashMan.despawn(towerModel.gameObject);
				}
			}
		}

		public void InitExtendWeapon(GameObject weapon)
		{
			GameObject gameObject = null;
			gameObject = Object.Instantiate(weapon);
			gameObject.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = gameObject.gameObject;
			trashManRecycleBin.instancesToPreallocate = 0;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(gameObject);
		}

		public WeaponStation GetWeaponByName(string name)
		{
			WeaponStation weaponStation = null;
			string gameObjectName = $"{name}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			weaponStation = gameObject.GetComponent<WeaponStation>();
			weaponStation.transform.parent = base.transform;
			return weaponStation;
		}

		public TowerModel Get(int id, int level)
		{
			TowerModel towerModel = null;
			string gameObjectName = $"tower_{id}_{level}(Clone)";
			GameObject gameObject = TrashMan.spawn(gameObjectName);
			towerModel = gameObject.GetComponent<TowerModel>();
			towerModel.gameObject.SetActive(value: true);
			towerModel.transform.parent = base.transform;
			return towerModel;
		}

		public void Push(TowerModel tower, int id, int level)
		{
			tower.transform.position = PoolPosition;
			tower.gameObject.SetActive(value: false);
			TrashMan.despawn(tower.gameObject);
		}

		public void Push(GameObject go)
		{
			go.transform.position = PoolPosition;
			go.gameObject.SetActive(value: false);
			TrashMan.despawn(go);
		}

		public void InitMiniDragon(GameObject miniDragonPrefab)
		{
			GameObject gameObject = null;
			gameObject = Object.Instantiate(miniDragonPrefab);
			gameObject.gameObject.SetActive(value: false);
			TrashManRecycleBin trashManRecycleBin = new TrashManRecycleBin();
			trashManRecycleBin.prefab = gameObject.gameObject;
			trashManRecycleBin.instancesToPreallocate = 2;
			TrashManRecycleBin recycleBin = trashManRecycleBin;
			TrashMan.manageRecycleBin(recycleBin);
			TrashMan.despawn(gameObject.gameObject);
		}

		public MiniDragonController GetMiniDragon()
		{
			MiniDragonController miniDragonController = null;
			GameObject gameObject = TrashMan.spawn("MiniDragon(Clone)");
			miniDragonController = gameObject.GetComponent<MiniDragonController>();
			miniDragonController.gameObject.SetActive(value: false);
			return miniDragonController;
		}
	}
}
