using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_5_3 : HeroSkillParameterBasic
	{
		public List<Hero5Skill3Param> listParam = new List<Hero5Skill3Param>();

		public void AddParamToList(Hero5Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.arrow_number);
		}

		public Hero5Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
