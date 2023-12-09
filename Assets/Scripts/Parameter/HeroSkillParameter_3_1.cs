using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_3_1 : HeroSkillParameterBasic
	{
		public List<Hero3Skill1Param> listParam = new List<Hero3Skill1Param>();

		public void AddParamToList(Hero3Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.attack_speed_increase);
			mainParam1.Add(param.magic_damage_bonus);
		}

		public Hero3Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
