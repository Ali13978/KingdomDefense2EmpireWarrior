using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_4_0 : HeroSkillParameterBasic
	{
		public List<Hero4Skill0Param> listParam = new List<Hero4Skill0Param>();

		public void AddParamToList(Hero4Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage);
		}

		public Hero4Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
