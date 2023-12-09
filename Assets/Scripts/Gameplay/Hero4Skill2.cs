using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Skill2 : HeroSkillCommon
	{
		private int heroID = 4;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int changeToStun;

		private float duration;

		private string description;

		private string buffkey = "Slow";

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_4_2 heroSkillParameter_4_ = new HeroSkillParameter_4_2();
			heroSkillParameter_4_ = (HeroSkillParameter_4_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			changeToStun = heroSkillParameter_4_.getParam(currentSkillLevel - 1).change_to_stun;
			duration = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).duration / 1000f;
			description = heroSkillParameter_4_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_STUN);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if ((bool)heroModel.currentTarget)
			{
				EnemyModel currentTarget = heroModel.currentTarget;
				if ((bool)currentTarget && Random.Range(0, 100) < changeToStun)
				{
					currentTarget.ProcessEffect(buffkey, 100, duration, DamageFXType.Stun);
				}
			}
		}
	}
}
