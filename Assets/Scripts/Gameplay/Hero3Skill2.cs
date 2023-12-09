using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Skill2 : HeroSkillCommon
	{
		private int heroID = 3;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private float skillRange;

		private int slowPercent;

		private int damageBurn;

		private float duration;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		private string buffKey = "Slow";

		[SerializeField]
		private Hero3Skill2IceTrap iceTrap;

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
			HeroSkillParameter_3_2 heroSkillParameter_3_ = new HeroSkillParameter_3_2();
			heroSkillParameter_3_ = (HeroSkillParameter_3_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			skillRange = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			slowPercent = heroSkillParameter_3_.getParam(currentSkillLevel - 1).slow_percent;
			damageBurn = heroSkillParameter_3_.getParam(currentSkillLevel - 1).damage_burn;
			duration = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_3_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(iceTrap.gameObject);
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
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_1);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_1);
			CastIceTrap();
		}

		private void CastIceTrap()
		{
			EnemyModel currentTarget = heroModel.currentTarget;
			if ((bool)currentTarget)
			{
				Hero3Skill2IceTrap hero3Skill2IceTrap = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHero3Skill2IceTrap();
				hero3Skill2IceTrap.transform.position = currentTarget.transform.position;
				hero3Skill2IceTrap.Init(damageBurn, buffKey, slowPercent, duration);
			}
			timeTracking = cooldownTime;
		}
	}
}
