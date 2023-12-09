using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_5_0 : HeroSkillParameterBasic
	{
		public List<Hero5Skill0Param> listParam = new List<Hero5Skill0Param>();

		public void AddParamToList(Hero5Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.heal_amount);
		}

		public Hero5Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
