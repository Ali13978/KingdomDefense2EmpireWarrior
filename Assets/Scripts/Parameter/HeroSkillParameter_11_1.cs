using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_11_1 : HeroSkillParameterBasic
	{
		public List<Hero11Skill1Param> listParam = new List<Hero11Skill1Param>();

		public void AddParamToList(Hero11Skill1Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.magic_damage);
		}
	}
}
