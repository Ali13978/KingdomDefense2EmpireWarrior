using Data;
using DG.Tweening;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero2Skill1 : HeroSkillCommon
	{
		private int heroID = 2;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private float armorBonus;

		private float duration;

		private float cooldownTime;

		private string description;

		private float cooldownTimeTracking;

		private Tweener tween;

		private string buffKey = "IncreaseArmorPhysics";

		[SerializeField]
		private float animationTime;

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive))
			{
				cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_2_1 heroSkillParameter_2_ = new HeroSkillParameter_2_1();
			heroSkillParameter_2_ = (HeroSkillParameter_2_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			armorBonus = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).armorBonus / 100f;
			duration = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_2_.getParam(currentSkillLevel - 1).description;
			cooldownTimeTracking = cooldownTime;
			heroModel.OnBeHitEvent += HeroModel_OnBeHitEvent;
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_HEAL_1);
		}

		private void HeroModel_OnBeHitEvent()
		{
			if (IsCooldownDone() && unLock)
			{
				StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			AddPassiveArmor();
			CastArmorFX();
			heroModel.SetSpecialStateDuration(animationTime);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
		}

		private void CastArmorFX()
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_1);
			effect.transform.position = base.transform.position;
			effect.Init(duration, base.transform, heroModel.GetComponent<SpriteRenderer>().sprite.rect.width);
		}

		private void AddPassiveArmor()
		{
			heroModel.BuffsHolder.AddBuff(buffKey, new Buff(isPositive: true, armorBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
			cooldownTimeTracking = cooldownTime;
		}

		private bool IsCooldownDone()
		{
			return cooldownTimeTracking == 0f;
		}
	}
}
