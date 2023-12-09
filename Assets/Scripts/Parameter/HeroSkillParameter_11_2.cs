using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_11_2 : HeroSkillParameterBasic
	{
		public List<Hero11Skill2Param> listParam = new List<Hero11Skill2Param>();

		public void AddParamToList(Hero11Skill2Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.magic_damage);
		}
	}
}
