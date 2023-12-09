using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_6_2 : HeroSkillParameterBasic
	{
		public List<Hero6Skill2Param> listParam = new List<Hero6Skill2Param>();

		public void AddParamToList(Hero6Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage);
		}

		public Hero6Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
