using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_5_2 : HeroSkillParameterBasic
	{
		public List<Hero5Skill2Param> listParam = new List<Hero5Skill2Param>();

		public void AddParamToList(Hero5Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.enemy_affected);
		}

		public Hero5Skill2Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
