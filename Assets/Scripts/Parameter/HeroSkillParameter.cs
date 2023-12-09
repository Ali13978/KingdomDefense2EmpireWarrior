using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter
	{
		private static HeroSkillParameter _instance;

		private Dictionary<int, HeroSkillParameterBasic[]> parameterDictionary = new Dictionary<int, HeroSkillParameterBasic[]>
		{
			{
				0,
				new HeroSkillParameterBasic[4]
			},
			{
				1,
				new HeroSkillParameterBasic[4]
			},
			{
				2,
				new HeroSkillParameterBasic[4]
			},
			{
				3,
				new HeroSkillParameterBasic[4]
			},
			{
				4,
				new HeroSkillParameterBasic[4]
			},
			{
				5,
				new HeroSkillParameterBasic[4]
			},
			{
				6,
				new HeroSkillParameterBasic[4]
			},
			{
				7,
				new HeroSkillParameterBasic[4]
			},
			{
				8,
				new HeroSkillParameterBasic[4]
			},
			{
				9,
				new HeroSkillParameterBasic[4]
			},
			{
				10,
				new HeroSkillParameterBasic[4]
			},
			{
				11,
				new HeroSkillParameterBasic[4]
			}
		};

		public static HeroSkillParameter Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new HeroSkillParameter();
				}
				return _instance;
			}
		}

		public void SetHeroSkillParameter(int heroID, int skillID, HeroSkillParameterBasic heroSkillParameterBasic)
		{
			parameterDictionary[heroID][skillID] = heroSkillParameterBasic;
		}

		public HeroSkillParameterBasic GetHeroSkillsParameter(int heroID, int skillID)
		{
			return parameterDictionary[heroID][skillID];
		}

		public int GetNumberOfMainParam(int heroID, int skillID)
		{
			int num = 0;
			if (parameterDictionary[heroID][skillID].mainParam0.Count > 0)
			{
				num++;
			}
			if (parameterDictionary[heroID][skillID].mainParam1.Count > 0)
			{
				num++;
			}
			return num;
		}

		public List<float> GetMainParams0(int heroID, int skillID)
		{
			return parameterDictionary[heroID][skillID].mainParam0;
		}

		public List<float> GetMainParams1(int heroID, int skillID)
		{
			return parameterDictionary[heroID][skillID].mainParam1;
		}
	}
}
