using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_7_2 : HeroSkillParameterBasic
	{
		public List<Hero7Skill2Param> listParam = new List<Hero7Skill2Param>();

		public void AddParamToList(Hero7Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.chance_to_cast);
			mainParam1.Add(param.physics_damage);
		}

		public Hero7Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
