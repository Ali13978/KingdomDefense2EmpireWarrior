using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_8_2 : HeroSkillParameterBasic
	{
		public List<Hero8Skill2Param> listParam = new List<Hero8Skill2Param>();

		public void AddParamToList(Hero8Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.duration / 1000f);
		}

		public Hero8Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
