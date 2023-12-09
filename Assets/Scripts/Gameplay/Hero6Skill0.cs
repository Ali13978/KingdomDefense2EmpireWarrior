using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero6Skill0 : HeroSkillCommon
	{
		private int heroID = 6;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int physicsDamage;

		private int magicDamage;

		private float stunDuration;

		private float skillRange;

		private float cooldownTime;

		private string description;

		private string useType;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private float effectLifeTime;

		private string buffKey = "Stun";

		[SerializeField]
		private HeroSkillAOECommon skillObject;

		private EffectAttack effectAttackSender;

		private void Start()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition += Instance_onCastHeroSkillToAssignedPosition;
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition -= Instance_onCastHeroSkillToAssignedPosition;
		}

		private void Instance_onCastHeroSkillToAssignedPosition(int heroID, Vector2 targetPosition)
		{
			if (this.heroID == heroID)
			{
				StartCoroutine(CastSkill(targetPosition));
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_6_0 heroSkillParameter_6_ = new HeroSkillParameter_6_0();
			heroSkillParameter_6_ = (HeroSkillParameter_6_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).magic_damage;
			stunDuration = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).stun_duration / 1000f;
			skillRange = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_6_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_6_.getParam(currentSkillLevel - 1).use_type;
			effectAttackSender = default(EffectAttack);
			effectAttackSender.buffKey = buffKey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = 100;
			effectAttackSender.debuffEffectDuration = stunDuration;
			effectAttackSender.damageFXType = DamageFXType.Electric;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(skillObject.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_ELECTRIC);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_FADE_SCREEN);
		}

		public override float GetCooldownTime()
		{
			return cooldownTime;
		}

		public override string GetUseType()
		{
			return useType;
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
			EffectController fxCanvas = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_FADE_SCREEN);
			fxCanvas.transform.position = Vector3.zero;
			float fadeTime = animDuration;
			fxCanvas.Init(fadeTime);
			fxCanvas.DoFadeIn(fadeTime / 2f, 20f / 51f);
			yield return new WaitForSeconds(delayTimeCastSkill);
			fxCanvas.DoFadeOut(fadeTime / 2f, 20f / 51f);
			HeroSkillAOECommon bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHeroSkillAOECommon("Hero6Skill0Thunder");
			bullet.transform.position = targetPosition;
			bullet.Init_DamageImmediately(new CommonAttackDamage(physicsDamage, magicDamage, skillRange), effectAttackSender, effectLifeTime);
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
		}
	}
}
