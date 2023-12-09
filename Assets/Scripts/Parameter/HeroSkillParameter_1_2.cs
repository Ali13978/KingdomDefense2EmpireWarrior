using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_1_2 : HeroSkillParameterBasic
	{
		public List<Hero1Skill2Param> listParam = new List<Hero1Skill2Param>();

		public void AddParamToList(Hero1Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.duration / 1000f);
		}

		public Hero1Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
