using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_4_1 : HeroSkillParameterBasic
	{
		public List<Hero4Skill1Param> listParam = new List<Hero4Skill1Param>();

		public void AddParamToList(Hero4Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.duration / 1000f);
		}

		public Hero4Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
