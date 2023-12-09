using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_8_3 : HeroSkillParameterBasic
	{
		public List<Hero8Skill3Param> listParam = new List<Hero8Skill3Param>();

		public void AddParamToList(Hero8Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.attack_range_bonus_percentage);
		}

		public Hero8Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
