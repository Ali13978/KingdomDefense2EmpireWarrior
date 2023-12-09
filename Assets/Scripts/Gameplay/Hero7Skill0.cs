using Data;
using DG.Tweening;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero7Skill0 : HeroSkillCommon
	{
		private int heroID = 7;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int physicsDamage;

		private int magicDamage;

		private float skillRange;

		private float cooldownTime;

		private string description;

		private string useType;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeToAttack;

		[SerializeField]
		private float delayTimeBetweenAttackFrame;

		[SerializeField]
		private int attackFrame;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

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
			HeroSkillParameter_7_0 heroSkillParameter_7_ = new HeroSkillParameter_7_0();
			heroSkillParameter_7_ = (HeroSkillParameter_7_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_7_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_7_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_BLOOD_SPREAD, 10);
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
			heroModel.transform.DOMove(targetPosition, 0f).SetEase(Ease.Linear).OnComplete(MoveToAssignedPositionComplete);
		}

		private void MoveToAssignedPositionComplete()
		{
			StartCoroutine(CastSkill());
		}

		private IEnumerator CastSkill()
		{
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
			List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(base.gameObject, new CommonAttackDamage(physicsDamage, magicDamage, skillRange));
			if (listEnemiesInRange.Count > 0)
			{
				UnityEngine.Debug.Log("co enemy!");
				heroModel.SetSpecialStateDuration(animDuration);
				heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
				heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
				yield return new WaitForSeconds(delayTimeToAttack);
				for (int i = 0; i < attackFrame; i++)
				{
					damageToAOERange.CastDamage(DamageType.Range, new CommonAttackDamage(physicsDamage / attackFrame, magicDamage / attackFrame, skillRange, isIgnoreArmor: true, isNotPlayIgnoreArmorEffect: true));
					yield return new WaitForSeconds(delayTimeBetweenAttackFrame);
				}
				foreach (EnemyModel item in listEnemiesInRange)
				{
					EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_BLOOD_SPREAD);
					effect.Init(0.33f, item.transform);
				}
			}
			else
			{
				UnityEngine.Debug.Log("khong co enemy!");
			}
			heroModel.SetAssignedPosition(heroModel.transform.position);
		}
	}
}
