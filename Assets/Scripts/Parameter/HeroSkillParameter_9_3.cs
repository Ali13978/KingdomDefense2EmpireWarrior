using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_9_3 : HeroSkillParameterBasic
	{
		public List<Hero9Skill3Param> listParam = new List<Hero9Skill3Param>();

		public void AddParamToList(Hero9Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.parameter_Scale);
			mainParam1.Add((float)param.duration / 1000f);
		}

		public Hero9Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
