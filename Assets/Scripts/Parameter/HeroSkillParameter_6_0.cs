using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_6_0 : HeroSkillParameterBasic
	{
		public List<Hero6Skill0Param> listParam = new List<Hero6Skill0Param>();

		public void AddParamToList(Hero6Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.stun_duration / 1000f);
		}

		public Hero6Skill0Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
