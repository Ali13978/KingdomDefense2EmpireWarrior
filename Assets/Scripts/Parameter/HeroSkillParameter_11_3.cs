using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_11_3 : HeroSkillParameterBasic
	{
		public List<Hero11Skill3Param> listParam = new List<Hero11Skill3Param>();

		public void AddParamToList(Hero11Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add((float)param.heal_duration * 0.001f);
		}
	}
}
