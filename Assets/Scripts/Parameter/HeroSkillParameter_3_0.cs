using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_3_0 : HeroSkillParameterBasic
	{
		public List<Hero3Skill0Param> listParam = new List<Hero3Skill0Param>();

		public void AddParamToList(Hero3Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage);
		}

		public Hero3Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
