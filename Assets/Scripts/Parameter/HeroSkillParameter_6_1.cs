using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_6_1 : HeroSkillParameterBasic
	{
		public List<Hero6Skill1Param> listParam = new List<Hero6Skill1Param>();

		public void AddParamToList(Hero6Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.percent_attack_damage_bonus);
			mainParam1.Add((float)param.duration / 1000f);
		}

		public Hero6Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
