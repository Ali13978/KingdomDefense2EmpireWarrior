using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_8_1 : HeroSkillParameterBasic
	{
		public List<Hero8Skill1Param> listParam = new List<Hero8Skill1Param>();

		public void AddParamToList(Hero8Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage);
			mainParam1.Add(param.attack_damage_decrease_percentage);
		}

		public Hero8Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
