using Data;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class HeroSkillController : HeroController
	{
		[SerializeField]
		private List<HeroSkillCommon> listHeroSkill = new List<HeroSkillCommon>();

		public void InitHeroSkills()
		{
			int currentHeroLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(base.HeroModel.HeroID);
			for (int i = 0; i < 4; i++)
			{
				if (HeroParameter.Instance.GetSkillPoint(base.HeroModel.HeroID, currentHeroLevel, i) > 0)
				{
					listHeroSkill[i].Init(base.HeroModel);
				}
			}
		}

		public void InitPetSkills()
		{
			foreach (HeroSkillCommon item in listHeroSkill)
			{
				item.Init(base.HeroModel);
			}
		}

		public float GetActiveSkillCooldownTime()
		{
			float cooldownTime = listHeroSkill[0].GetCooldownTime();
			return listHeroSkill[0].GetCooldownTime();
		}

		public string GetActiveSkillUseType()
		{
			return listHeroSkill[0].GetUseType();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			foreach (HeroSkillCommon item in listHeroSkill)
			{
				item.OnHeroReturnPool();
			}
		}

		public HeroSkillCommon GetSkill(int skillIndex)
		{
			if (skillIndex < listHeroSkill.Count)
			{
				return listHeroSkill[skillIndex];
			}
			return null;
		}
	}
}
