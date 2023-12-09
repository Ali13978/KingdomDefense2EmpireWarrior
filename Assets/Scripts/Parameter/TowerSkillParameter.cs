using System;
using System.Collections.Generic;

namespace Parameter
{
	public class TowerSkillParameter
	{
		private List<TowerSkillParam> listTowerSkillParams = new List<TowerSkillParam>();

		private static TowerSkillParameter instance;

		public static TowerSkillParameter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TowerSkillParameter();
				}
				return instance;
			}
		}

		public void ClearTowerSkillData()
		{
			listTowerSkillParams.Clear();
		}

		public void SetParameter(TowerSkillParam towerSkillCost)
		{
			listTowerSkillParams.Add(towerSkillCost);
		}

		public List<int> GetListParamNumber(int towerID, int ultimateBranch, int skillID, int skillLevel)
		{
			List<int> list = new List<int>();
			foreach (TowerSkillParam listTowerSkillParam in listTowerSkillParams)
			{
				TowerSkillParam current = listTowerSkillParam;
				if (current.towerID == towerID && current.ultimateBranch == ultimateBranch && current.skillID == skillID && current.skillLevel == skillLevel)
				{
					string paramPreview = current.paramPreview;
					string[] array = paramPreview.Split(new char[1]
					{
						'_'
					}, StringSplitOptions.RemoveEmptyEntries);
					string[] array2 = array;
					foreach (string s in array2)
					{
						list.Add(int.Parse(s));
					}
				}
			}
			return list;
		}

		public string GetParam(int towerID, int ultimateBranch, int skillID, int paramID)
		{
			string result = string.Empty;
			foreach (TowerSkillParam listTowerSkillParam in listTowerSkillParams)
			{
				TowerSkillParam current = listTowerSkillParam;
				if (current.towerID == towerID && current.ultimateBranch == ultimateBranch && current.skillID == skillID)
				{
					result = GetParamBySkillLevel(towerID, ultimateBranch, skillID, 0, paramID) + "/" + GetParamBySkillLevel(towerID, ultimateBranch, skillID, 1, paramID) + "/" + GetParamBySkillLevel(towerID, ultimateBranch, skillID, 2, paramID);
				}
			}
			return result;
		}

		public int GetParamBySkillLevel(int towerID, int ultimateBranch, int skillID, int skillLevel, int paramID)
		{
			int result = -1;
			foreach (TowerSkillParam listTowerSkillParam in listTowerSkillParams)
			{
				TowerSkillParam current = listTowerSkillParam;
				if (current.towerID == towerID && current.ultimateBranch == ultimateBranch && current.skillID == skillID && current.skillLevel == skillLevel)
				{
					switch (paramID)
					{
					case 0:
						result = current.param0;
						break;
					case 1:
						result = current.param1;
						break;
					case 2:
						result = current.param2;
						break;
					case 3:
						result = current.param3;
						break;
					case 4:
						result = current.param4;
						break;
					}
				}
			}
			return result;
		}

		public int GetUltimateSkillUpgradeCost(int towerID, int ultimateBranch, int skillID, int skillLevel)
		{
			int result = -1;
			foreach (TowerSkillParam listTowerSkillParam in listTowerSkillParams)
			{
				TowerSkillParam current = listTowerSkillParam;
				if (current.towerID == towerID && current.ultimateBranch == ultimateBranch && current.skillID == skillID && current.skillLevel == skillLevel)
				{
					result = current.upgradeCost;
				}
			}
			return result;
		}
	}
}
