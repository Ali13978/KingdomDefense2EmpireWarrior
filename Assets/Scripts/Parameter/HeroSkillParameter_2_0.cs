using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_2_0 : HeroSkillParameterBasic
	{
		public List<Hero2Skill0Param> listParam = new List<Hero2Skill0Param>();

		public void AddParamToList(Hero2Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.parameter_Scale);
		}

		public Hero2Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
