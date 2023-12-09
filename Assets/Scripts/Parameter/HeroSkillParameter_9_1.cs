using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_9_1 : HeroSkillParameterBasic
	{
		public List<Hero9Skill1Param> listParam = new List<Hero9Skill1Param>();

		public void AddParamToList(Hero9Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.enemy_affected);
			mainParam1.Add((float)param.knock_back_duration / 1000f);
		}

		public Hero9Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
