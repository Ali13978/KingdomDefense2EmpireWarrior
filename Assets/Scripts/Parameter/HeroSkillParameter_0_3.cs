using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_0_3 : HeroSkillParameterBasic
	{
		public List<Hero0Skill3Param> listParam = new List<Hero0Skill3Param>();

		public void AddParamToList(Hero0Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.armor_per_unit);
		}

		public Hero0Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
