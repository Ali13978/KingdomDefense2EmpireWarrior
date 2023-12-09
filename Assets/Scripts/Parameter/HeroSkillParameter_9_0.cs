using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_9_0 : HeroSkillParameterBasic
	{
		public List<Hero9Skill0Param> listParam = new List<Hero9Skill0Param>();

		public void AddParamToList(Hero9Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.number_of_minion);
			mainParam1.Add((float)param.minion_lifetime / 1000f);
		}

		public Hero9Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
