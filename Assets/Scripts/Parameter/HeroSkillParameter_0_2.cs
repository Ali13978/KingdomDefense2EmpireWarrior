using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_0_2 : HeroSkillParameterBasic
	{
		public List<Hero0Skill2Param> listParam = new List<Hero0Skill2Param>();

		public void AddParamToList(Hero0Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.armor);
		}

		public Hero0Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
