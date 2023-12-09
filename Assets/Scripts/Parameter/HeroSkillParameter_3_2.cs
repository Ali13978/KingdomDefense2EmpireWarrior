using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_3_2 : HeroSkillParameterBasic
	{
		public List<Hero3Skill2Param> listParam = new List<Hero3Skill2Param>();

		public void AddParamToList(Hero3Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.damage_burn);
		}

		public Hero3Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
