using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TowerPrebuilder : MonoBehaviour
	{
		[SerializeField]
		private List<TowerPrebuild> listTowersPrebuild = new List<TowerPrebuild>();

		private void Start()
		{
			BuildTowers();
		}

		private void BuildTowers()
		{
			foreach (TowerPrebuild item in listTowersPrebuild)
			{
				BuildTower(item.towerID, item.towerLevel, item.buildRegionID);
			}
		}

		private void BuildTower(int towerID, int towerLevel, int buildRegionID)
		{
			TowerModel towerModel = SingletonMonoBehaviour<SpawnTower>.Instance.Get(towerID, towerLevel);
			towerModel.StartBuild(towerID, towerLevel, buildRegionID);
			towerModel.Appear();
			towerModel.transform.position = SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[buildRegionID].transform.position;
			SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[buildRegionID].DisplayNotBuildable();
		}
	}
}
