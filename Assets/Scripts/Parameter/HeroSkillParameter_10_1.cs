using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_10_1 : HeroSkillParameterBasic
	{
		public List<Hero10Skill1Param> listParam = new List<Hero10Skill1Param>();

		public void AddParamToList(Hero10Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.magic_damage);
			mainParam1.Add(param.def_down_percent);
		}
	}
}
