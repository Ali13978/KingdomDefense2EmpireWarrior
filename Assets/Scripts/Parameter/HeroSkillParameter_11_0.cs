using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_11_0 : HeroSkillParameterBasic
	{
		public List<Hero11Skill0Param> listParam = new List<Hero11Skill0Param>();

		public void AddParamToList(Hero11Skill0Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.magic_damage);
		}
	}
}
