using System.Collections.Generic;

namespace Parameter
{
	public class TowerDefaultSkillParameter
	{
		private List<TowerDefaultSkill> listTowerDefaultSkillParams = new List<TowerDefaultSkill>();

		private static TowerDefaultSkillParameter instance;

		public static TowerDefaultSkillParameter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TowerDefaultSkillParameter();
				}
				return instance;
			}
		}

		public void SetParameter(TowerDefaultSkill towerDefaultSkill)
		{
			int count = listTowerDefaultSkillParams.Count;
			if (count <= towerDefaultSkill.id)
			{
				listTowerDefaultSkillParams.Add(towerDefaultSkill);
			}
		}

		public TowerDefaultSkill GetTowerParameter(int towerID, int towerLevel)
		{
			TowerDefaultSkill result = default(TowerDefaultSkill);
			foreach (TowerDefaultSkill listTowerDefaultSkillParam in listTowerDefaultSkillParams)
			{
				TowerDefaultSkill current = listTowerDefaultSkillParam;
				if (current.towerID == towerID && current.towerLevel == towerLevel)
				{
					result = current;
				}
			}
			return result;
		}
	}
}
