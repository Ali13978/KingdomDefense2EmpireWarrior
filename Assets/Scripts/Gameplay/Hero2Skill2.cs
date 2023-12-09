using Data;
using Parameter;

namespace Gameplay
{
	public class Hero2Skill2 : HeroSkillCommon
	{
		private int heroID = 2;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int attackCountToCrit;

		private string description;

		private int currentAttackCount;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_2_2 heroSkillParameter_2_ = new HeroSkillParameter_2_2();
			heroSkillParameter_2_ = (HeroSkillParameter_2_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			attackCountToCrit = heroSkillParameter_2_.getParam(currentSkillLevel - 1).count_crit;
			description = heroSkillParameter_2_.getParam(currentSkillLevel - 1).description;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_CRITICAL);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (unLock)
			{
				currentAttackCount++;
				if (currentAttackCount == attackCountToCrit)
				{
					DamageCritEnemy();
					currentAttackCount = 0;
				}
			}
		}

		private void DamageCritEnemy()
		{
			if ((bool)heroModel.currentTarget)
			{
				heroModel.HeroAttackController.DamageToEnemy(heroModel.currentTarget);
				heroModel.currentTarget.EnemyEffectController.PlayDamageFX(DamageFXType.Critical, 2f);
			}
		}
	}
}
