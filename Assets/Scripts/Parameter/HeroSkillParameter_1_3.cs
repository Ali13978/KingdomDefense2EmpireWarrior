using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_1_3 : HeroSkillParameterBasic
	{
		public List<Hero1Skill3Param> listParam = new List<Hero1Skill3Param>();

		public void AddParamToList(Hero1Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.slow_percent);
		}

		public Hero1Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
