using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_9_2 : HeroSkillParameterBasic
	{
		public List<Hero9Skill2Param> listParam = new List<Hero9Skill2Param>();

		public void AddParamToList(Hero9Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.health_amount);
		}

		public Hero9Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
