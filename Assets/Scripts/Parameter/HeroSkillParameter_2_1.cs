using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_2_1 : HeroSkillParameterBasic
	{
		public List<Hero2Skill1Param> listParam = new List<Hero2Skill1Param>();

		public void AddParamToList(Hero2Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.armorBonus);
		}

		public Hero2Skill1Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
