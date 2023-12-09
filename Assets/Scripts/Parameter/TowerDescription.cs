using System.Collections.Generic;

namespace Parameter
{
	public class TowerDescription : Singleton<TowerDescription>
	{
		private List<List<TowerDes>> listTowerDes = new List<List<TowerDes>>();

		private List<TowerSkillDes> listTowerSkillDes = new List<TowerSkillDes>();

		public void ClearTowerData()
		{
			listTowerDes.Clear();
		}

		public void SetTowerParameter(TowerDes tower)
		{
			int count = listTowerDes.Count;
			if (count <= tower.id)
			{
				List<TowerDes> list = new List<TowerDes>();
				list.Insert(tower.level, tower);
				listTowerDes.Insert(tower.id, list);
			}
			else
			{
				List<TowerDes> list2 = listTowerDes[tower.id];
				list2.Insert(tower.level, tower);
			}
		}

		public TowerDes GetTowerParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listTowerDes[id][level];
			}
			return default(TowerDes);
		}

		public int GetNumberOfTower()
		{
			return listTowerDes.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfTower() > 0)
			{
				return listTowerDes[0].Count;
			}
			return 0;
		}

		private bool CheckParameter(int id, int level)
		{
			if (id < 0 || level < 0 || id >= GetNumberOfTower() || level >= GetNumberOfLevel())
			{
				return false;
			}
			return true;
		}

		public void ClearTowerSkillData()
		{
			listTowerSkillDes.Clear();
		}

		public void SetTowerSkillParameter(TowerSkillDes towerSkillDes)
		{
			listTowerSkillDes.Add(towerSkillDes);
		}

		public List<TowerSkillDes> GetListTowerSkillDes()
		{
			return listTowerSkillDes;
		}

		public string GetTowerName(int towerId)
		{
			if (towerId < listTowerDes.Count)
			{
				TowerDes towerDes = listTowerDes[towerId][0];
				return towerDes.name;
			}
			return null;
		}

		public string GetTowerType(int towerId)
		{
			if (towerId < listTowerDes.Count)
			{
				TowerDes towerDes = listTowerDes[towerId][0];
				return towerDes.type;
			}
			return null;
		}

		public string GetTowerDescription(int towerId, int towerLevel)
		{
			if (towerId < listTowerDes.Count)
			{
				TowerDes towerDes = listTowerDes[towerId][towerLevel];
				return towerDes.fullDescription;
			}
			return null;
		}

		public string GetTowerShortDescription(int towerId, int towerLevel)
		{
			if (towerId < listTowerDes.Count)
			{
				TowerDes towerDes = listTowerDes[towerId][towerLevel];
				return towerDes.shortDescription;
			}
			return null;
		}

		public string GetTowerUnlockDescription(int towerId, int towerLevel)
		{
			if (towerId < listTowerDes.Count)
			{
				TowerDes towerDes = listTowerDes[towerId][towerLevel];
				return towerDes.unlockDescription;
			}
			return null;
		}

		public string GetTowerUltimateName(int towerId, int towerLevel, int skillID)
		{
			string result = string.Empty;
			int ultimateBranchByLevel = TowerParameter.Instance.GetUltimateBranchByLevel(towerLevel);
			foreach (TowerSkillDes listTowerSkillDe in listTowerSkillDes)
			{
				TowerSkillDes current = listTowerSkillDe;
				if (current.id == towerId && current.level == towerLevel && current.ultimateBranch == ultimateBranchByLevel && current.skillID == skillID)
				{
					result = current.ultimateName;
				}
			}
			return result;
		}

		public string GetTowerUltimateDescription(int towerId, int towerLevel, int skillID)
		{
			string result = string.Empty;
			int ultimateBranchByLevel = TowerParameter.Instance.GetUltimateBranchByLevel(towerLevel);
			foreach (TowerSkillDes listTowerSkillDe in listTowerSkillDes)
			{
				TowerSkillDes current = listTowerSkillDe;
				if (current.id == towerId && current.level == towerLevel && current.ultimateBranch == ultimateBranchByLevel && current.skillID == skillID)
				{
					List<int> listParamNumber = TowerSkillParameter.Instance.GetListParamNumber(towerId, ultimateBranchByLevel, skillID, 0);
					switch (listParamNumber.Count)
					{
					case 0:
						result = current.ultimateDescription;
						break;
					case 1:
					{
						string param6 = TowerSkillParameter.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[0]);
						result = string.Format(current.ultimateDescription, param6);
						break;
					}
					case 2:
					{
						string param4 = TowerSkillParameter.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[0]);
						string param5 = TowerSkillParameter.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[1]);
						result = string.Format(current.ultimateDescription, param4, param5);
						break;
					}
					case 3:
					{
						string param = TowerSkillParameter.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[0]);
						string param2 = TowerSkillParameter.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[1]);
						string param3 = TowerSkillParameter.Instance.GetParam(towerId, ultimateBranchByLevel, skillID, listParamNumber[2]);
						result = string.Format(current.ultimateDescription, param, param2, param3);
						break;
					}
					}
				}
			}
			return result;
		}
	}
}
