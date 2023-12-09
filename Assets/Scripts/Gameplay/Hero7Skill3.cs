using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero7Skill3 : HeroSkillCommon
	{
		private int heroID = 7;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int chanceToCastSkill;

		private int physicsDamage;

		private int magicDamage;

		private float skillRange;

		private float countdownTime;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private HeroSkillAOECommon skillObject;

		public override void Update()
		{
			base.Update();
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && unLock && (!heroModel || heroModel.IsAlive))
			{
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
			HeroSkillParameter_7_3 heroSkillParameter_7_ = new HeroSkillParameter_7_3();
			heroSkillParameter_7_ = (HeroSkillParameter_7_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			chanceToCastSkill = heroSkillParameter_7_.getParam(currentSkillLevel - 1).chance_to_cast;
			physicsDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			countdownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).countdown_time / 1000f;
			cooldownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_7_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			heroModel.OnAttackEvent += HeroModel_OnAttackEvent;
			InitFXs();
		}

		private void HeroModel_OnAttackEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && IsAbleToCastSkill() && unLock && (bool)heroModel.currentTarget)
			{
				StartCoroutine(CastSkill(heroModel.currentTarget));
			}
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.TIMER_BOMB_EXPLOSION);
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(skillObject.gameObject);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private bool IsAbleToCastSkill()
		{
			return chanceToCastSkill <= Random.Range(0, 100);
		}

		private IEnumerator CastSkill(EnemyModel target)
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_2);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_2);
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			HeroSkillAOECommon bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHeroSkillAOECommon("Hero7Skill3TimerBomb");
			bullet.transform.position = target.transform.position;
			bullet.Init_DamgeAfterTime(new CommonAttackDamage(physicsDamage, magicDamage, skillRange), countdownTime, SpawnFX.TIMER_BOMB_EXPLOSION, 1f);
		}
	}
}
