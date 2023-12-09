using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_6_3 : HeroSkillParameterBasic
	{
		public List<Hero6Skill3Param> listParam = new List<Hero6Skill3Param>();

		public void AddParamToList(Hero6Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage);
		}

		public Hero6Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
