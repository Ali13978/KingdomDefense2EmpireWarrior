using System.Collections.Generic;

namespace Parameter
{
	public class HeroSkillParameter_7_3 : HeroSkillParameterBasic
	{
		public List<Hero7Skill3Param> listParam = new List<Hero7Skill3Param>();

		public void AddParamToList(Hero7Skill3Param param)
		{
			listParam.Add(param);
			mainParam0.Add(param.physics_damage);
			mainParam1.Add((float)param.countdown_time / 1000f);
		}

		public Hero7Skill3Param getParam(int skillLevel)
		{
			return listParam[skillLevel];
		}
	}
}
