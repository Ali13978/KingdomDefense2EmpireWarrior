using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Skill3 : HeroSkillCommon
	{
		private int heroID = 3;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private float skillRange;

		private int physicsDamage;

		private int magicDamage;

		private float timeStep;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		private float offsetHigh = 6f;

		[SerializeField]
		private Hero3Skill3SunStrike sunStrike;

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive))
			{
				if (IsCooldownDone())
				{
					StartCoroutine(CastSkill());
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_3_3 heroSkillParameter_3_ = new HeroSkillParameter_3_3();
			heroSkillParameter_3_ = (HeroSkillParameter_3_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			skillRange = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			physicsDamage = heroSkillParameter_3_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_3_.getParam(currentSkillLevel - 1).magic_damage;
			timeStep = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).time_step / 1000f;
			cooldownTime = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_3_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(sunStrike.gameObject);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(1f);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_2);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_2);
			CastSunStrike();
		}

		private void CastSunStrike()
		{
			EnemyModel enemyWithHighestHealth = SingletonMonoBehaviour<GameData>.Instance.getEnemyWithHighestHealth(isFlyEnemy: true, isUndergroundEnemy: false);
			if ((bool)enemyWithHighestHealth)
			{
				Hero3Skill3SunStrike hero3Skill3SunStrike = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHero3Skill3SunStrike();
				hero3Skill3SunStrike.transform.position = enemyWithHighestHealth.transform.position;
				Hero3Skill3SunStrike hero3Skill3SunStrike2 = hero3Skill3SunStrike;
				int num = physicsDamage;
				int num2 = magicDamage;
				float aoeRange = skillRange;
				float lifeTime = timeStep;
				float num3 = offsetHigh;
				Vector3 position = enemyWithHighestHealth.transform.position;
				hero3Skill3SunStrike2.Init(num, num2, aoeRange, lifeTime, num3 - position.y);
			}
			timeTracking = cooldownTime;
		}
	}
}
