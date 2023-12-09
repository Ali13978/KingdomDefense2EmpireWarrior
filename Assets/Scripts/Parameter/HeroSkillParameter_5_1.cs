using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_5_1 : HeroSkillParameterBasic
	{
		public List<Hero5Skill1Param> listParam = new List<Hero5Skill1Param>();

		public void AddParamToList(Hero5Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.bonus_damage);
		}

		public Hero5Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
