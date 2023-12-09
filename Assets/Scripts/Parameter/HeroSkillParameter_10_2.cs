using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_10_2 : HeroSkillParameterBasic
	{
		public List<Hero10Skill2Param> listParam = new List<Hero10Skill2Param>();

		public void AddParamToList(Hero10Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physic_damage);
		}
	}
}
