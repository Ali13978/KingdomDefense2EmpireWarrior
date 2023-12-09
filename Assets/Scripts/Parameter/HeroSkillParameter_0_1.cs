using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_0_1 : HeroSkillParameterBasic
	{
		public List<Hero0Skill1Param> listParam = new List<Hero0Skill1Param>();

		public void AddParamToList(Hero0Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.damage * ((float)param.duration / 1000f) * 4f);
		}

		public Hero0Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
