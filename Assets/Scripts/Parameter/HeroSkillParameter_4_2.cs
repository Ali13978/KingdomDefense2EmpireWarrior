using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_4_2 : HeroSkillParameterBasic
	{
		public List<Hero4Skill2Param> listParam = new List<Hero4Skill2Param>();

		public void AddParamToList(Hero4Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.change_to_stun);
		}

		public Hero4Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
