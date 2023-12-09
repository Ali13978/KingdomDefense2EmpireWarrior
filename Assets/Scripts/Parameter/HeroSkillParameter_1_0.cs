using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_1_0 : HeroSkillParameterBasic
	{
		public List<Hero1Skill0Param> listParam = new List<Hero1Skill0Param>();

		public void AddParamToList(Hero1Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.damage);
		}

		public Hero1Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
