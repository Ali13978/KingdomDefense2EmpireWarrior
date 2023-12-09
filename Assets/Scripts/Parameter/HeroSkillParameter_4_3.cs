using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_4_3 : HeroSkillParameterBasic
	{
		public List<Hero4Skill3Param> listParam = new List<Hero4Skill3Param>();

		public void AddParamToList(Hero4Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.attack_speed_bonus);
		}

		public Hero4Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
