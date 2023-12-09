using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_2_3 : HeroSkillParameterBasic
	{
		public List<Hero2Skill3Param> listParam = new List<Hero2Skill3Param>();

		public void AddParamToList(Hero2Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.damage);
		}

		public Hero2Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
