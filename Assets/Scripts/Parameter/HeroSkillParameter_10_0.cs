using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_10_0 : HeroSkillParameterBasic
	{
		public List<Hero10Skill0Param> listParam = new List<Hero10Skill0Param>();

		public void AddParamToList(Hero10Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.duration * 0.001f);
			mainParam1.Add(param.magic_damage);
		}
	}
}
