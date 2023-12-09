using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_1_1 : HeroSkillParameterBasic
	{
		public List<Hero1Skill1Param> listParam = new List<Hero1Skill1Param>();

		public void AddParamToList(Hero1Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.bonus_crit * 2);
		}

		public Hero1Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
