using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_7_0 : HeroSkillParameterBasic
	{
		public List<Hero7Skill0Param> listParam = new List<Hero7Skill0Param>();

		public void AddParamToList(Hero7Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage);
		}

		public Hero7Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
