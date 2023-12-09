using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_0_0 : HeroSkillParameterBasic
	{
		public List<Hero0Skill0Param> listParam = new List<Hero0Skill0Param>();

		public void AddParamToList(Hero0Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.number_clone);
			mainParam1.Add(param.parameter_Scale);
		}

		public Hero0Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
