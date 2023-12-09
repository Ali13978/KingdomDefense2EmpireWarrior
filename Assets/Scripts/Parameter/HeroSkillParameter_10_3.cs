using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_10_3 : HeroSkillParameterBasic
	{
		public List<Hero10Skill3Param> listParam = new List<Hero10Skill3Param>();

		public void AddParamToList(Hero10Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.magic_damage);
		}
	}
}
