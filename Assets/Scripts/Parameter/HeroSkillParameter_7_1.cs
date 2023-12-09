using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_7_1 : HeroSkillParameterBasic
	{
		public List<Hero7Skill1Param> listParam = new List<Hero7Skill1Param>();

		public void AddParamToList(Hero7Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.percent_health_activate);
			mainParam1.Add(param.slow_percent);
		}

		public Hero7Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
