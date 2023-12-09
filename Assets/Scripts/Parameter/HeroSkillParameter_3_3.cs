using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_3_3 : HeroSkillParameterBasic
	{
		public List<Hero3Skill3Param> listParam = new List<Hero3Skill3Param>();

		public void AddParamToList(Hero3Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage);
		}

		public Hero3Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
