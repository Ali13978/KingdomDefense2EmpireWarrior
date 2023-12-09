using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Parameter
{
	public class TowerParameter
	{
		private List<List<Tower>> listTower = new List<List<Tower>>();

		private static TowerParameter instance;

		public static TowerParameter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TowerParameter();
				}
				return instance;
			}
		}

		public void SetTowerParameter(Tower tower)
		{
			tower = ApplyBuff(tower);
			int count = listTower.Count;
			if (count <= tower.id)
			{
				List<Tower> list = new List<Tower>();
				list.Insert(tower.level, tower);
				listTower.Insert(tower.id, list);
			}
			else
			{
				List<Tower> list2 = listTower[tower.id];
				list2.Insert(tower.level, tower);
			}
		}

		public Tower GetTowerParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTower[id][level];
			}
			return default(Tower);
		}

		public void ModifyTowerParameter(int id, int level, Tower newParameter)
		{
			listTower[id][level] = newParameter;
		}

		private bool CheckParameter(int id, int level)
		{
			if (id < 0 || level < 0 || id >= GetNumberOfTower() || level >= GetNumberOfLevel())
			{
				return false;
			}
			return true;
		}

		public int GetMinDamage(int towerID, int towerLevel)
		{
			int num = 0;
			if (CheckParameter(towerID, towerLevel))
			{
				Tower tower = listTower[towerID][towerLevel];
				if (tower.damage_Physics_min == 0)
				{
					Tower tower2 = listTower[towerID][towerLevel];
					return tower2.damage_Magic_min;
				}
				Tower tower3 = listTower[towerID][towerLevel];
				return tower3.damage_Physics_min;
			}
			return -1;
		}

		public int GetMaxDamage(int towerID, int towerLevel)
		{
			int num = 0;
			if (CheckParameter(towerID, towerLevel))
			{
				Tower tower = listTower[towerID][towerLevel];
				if (tower.damage_Physics_max == 0)
				{
					Tower tower2 = listTower[towerID][towerLevel];
					return tower2.damage_Magic_max;
				}
				Tower tower3 = listTower[towerID][towerLevel];
				return tower3.damage_Physics_max;
			}
			return -1;
		}

		public bool isPhysicsAttack(int towerID)
		{
			Tower tower = listTower[towerID][0];
			return tower.damage_Physics_max > 0;
		}

		public float GetCooldownTime(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				Tower tower = listTower[id][level];
				return (float)tower.reload / 1000f;
			}
			return -1f;
		}

		public int GetAttackSpeed(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				Tower tower = listTower[id][level];
				return tower.reload;
			}
			return -1;
		}

		public int GetRangeMax(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				Tower tower = listTower[id][level];
				return tower.attackRangeMax;
			}
			return -1;
		}

		public int GetUnitHealth(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				Tower tower = listTower[id][level];
				return tower.unit_health;
			}
			return -1;
		}

		public int GetUnitArmor(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				Tower tower = listTower[id][level];
				return tower.unit_armor_physics;
			}
			return -1;
		}

		public int GetPrice(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				Tower tower = listTower[id][level];
				return tower.price;
			}
			return -1;
		}

		public int GetUltimateBranchByLevel(int level)
		{
			int result = -1;
			if (level == 3)
			{
				result = 0;
			}
			if (level == 4)
			{
				result = 1;
			}
			return result;
		}

		public int GetNumberOfTower()
		{
			return listTower.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfTower() > 0)
			{
				return listTower[0].Count;
			}
			return 0;
		}

		public bool GetIsAirAttack(int id)
		{
			Tower tower = listTower[id][0];
			return tower.isAirAttack;
		}

		public int GetGoldProduce(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				Tower tower = listTower[id][level];
				return tower.goldProduce;
			}
			return -1;
		}

		public float GetAutoCollectProduceGoldTime(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				Tower tower = listTower[id][level];
				return (float)tower.autoCollectTime / 1000f;
			}
			return -1f;
		}

		private Tower ApplyBuff(Tower tower)
		{
			if (tower.id == 0)
			{
				int currentUpgradeLevel = ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(0);
				for (int i = 0; i <= currentUpgradeLevel; i++)
				{
					if (i == 0)
					{
						int num = 100 - ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(0, 0);
						tower.price = (int)((float)(tower.price * num) / 100f);
					}
					if (i == 1)
					{
						int upgradeValue = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(1, 0);
						tower.attackRangeMax += upgradeValue;
					}
					if (i == 2)
					{
						int upgradeValue2 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(7, 0);
						tower.ignoreArmorChance += upgradeValue2;
					}
					if (i == 3)
					{
						int upgradeValue3 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(2, 0);
						tower.attackRangeMax += upgradeValue3;
					}
					if (i == 4)
					{
						int upgradeValue4 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(6, 0);
						tower.criticalStrikeChance += upgradeValue4;
					}
				}
			}
			if (tower.id == 1)
			{
				int currentUpgradeLevel2 = ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(1);
				for (int j = 0; j <= currentUpgradeLevel2; j++)
				{
					if (j == 0)
					{
						int num = 100 - ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(0, 1);
						tower.price = (int)((float)(tower.price * num) / 100f);
					}
					if (j == 1)
					{
						int upgradeValue5 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(12, 1);
						tower.unit_armor_physics += upgradeValue5;
					}
					if (j == 2)
					{
						int upgradeValue = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(1, 1);
						tower.attackRangeMax += upgradeValue;
						int upgradeValue6 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(3, 1);
						tower.reload -= upgradeValue6;
					}
					if (j == 3)
					{
						int upgradeValue7 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(13, 1);
						tower.unit_health += upgradeValue7;
					}
					if (j == 4)
					{
						int upgradeValue8 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(5, 1);
						tower.ignoreReloadChance += upgradeValue8;
					}
				}
			}
			if (tower.id == 2)
			{
				int currentUpgradeLevel3 = ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(2);
				for (int k = 0; k <= currentUpgradeLevel3; k++)
				{
					if (k == 0)
					{
						int num = 100 - ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(0, 2);
						tower.price = (int)((float)(tower.price * num) / 100f);
					}
					if (k == 1)
					{
						UnityEngine.Debug.Log("nang cap 1: giam thoi gian reload!");
						int upgradeValue6 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(3, 2);
						tower.reload -= upgradeValue6;
					}
					if (k == 2)
					{
						int upgradeValue9 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(8, 2);
						tower.damage_Physics_min += upgradeValue9;
						tower.damage_Physics_max += upgradeValue9;
					}
					if (k == 3)
					{
						int upgradeValue = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(1, 2);
						tower.attackRangeMax += upgradeValue;
					}
					if (k == 4)
					{
						int upgradeValue10 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(4, 2);
						tower.reload -= upgradeValue10;
					}
				}
			}
			if (tower.id == 3)
			{
				int currentUpgradeLevel4 = ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(3);
				for (int l = 0; l <= currentUpgradeLevel4; l++)
				{
					if (l == 0)
					{
						int num = 100 - ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(0, 3);
						tower.price = (int)((float)(tower.price * num) / 100f);
					}
					if (l == 1)
					{
						int upgradeValue = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(1, 3);
						tower.attackRangeMax += upgradeValue;
					}
					if (l == 2)
					{
						int upgradeValue9 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(8, 3);
						tower.damage_Magic_min += upgradeValue9;
						tower.damage_Magic_max += upgradeValue9;
					}
					if (l == 3)
					{
						int upgradeValue4 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(6, 3);
						tower.criticalStrikeChance += upgradeValue4;
					}
					if (l == 4)
					{
						string debuffKey = DamageFXType.Slow.ToString();
						int upgradeValue11 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(9, 3);
						int upgradeValue12 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(11, 3);
						int upgradeValue13 = ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(10, 3);
						tower.debuffKey = debuffKey;
						tower.debuffChance = upgradeValue11;
						tower.debuffEffectDuration = upgradeValue12;
						tower.debuffEffectValue = upgradeValue13;
					}
				}
			}
			return tower;
		}
	}
}
