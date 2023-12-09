using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_8_0 : HeroSkillParameterBasic
	{
		public List<Hero8Skill0Param> listParam = new List<Hero8Skill0Param>();

		public void AddParamToList(Hero8Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage * param.number_of_projectile);
		}

		public Hero8Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
