using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_2_2 : HeroSkillParameterBasic
	{
		public List<Hero2Skill2Param> listParam = new List<Hero2Skill2Param>();

		public void AddParamToList(Hero2Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.count_crit);
		}

		public Hero2Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
