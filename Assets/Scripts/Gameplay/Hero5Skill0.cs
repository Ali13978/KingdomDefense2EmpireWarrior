using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Skill0 : HeroSkillCommon
	{
		private int heroID = 5;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int healAmount;

		private float skillRange;

		private float cooldownTime;

		private string description;

		private string useType;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private Hero5Skill0HealingBomb healingBomb;

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
			HeroSkillParameter_5_0 heroSkillParameter_5_ = new HeroSkillParameter_5_0();
			heroSkillParameter_5_ = (HeroSkillParameter_5_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			healAmount = heroSkillParameter_5_.getParam(currentSkillLevel - 1).heal_amount;
			skillRange = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_5_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_5_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(healingBomb.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_HEAL_0);
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
			CastHealingStatue(targetPosition);
		}

		private void CastHealingStatue(Vector2 targetPosition)
		{
			Hero5Skill0HealingBomb hero5Skill0HealingBomb = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHero5Skill0HealingBomb();
			hero5Skill0HealingBomb.transform.position = targetPosition;
			hero5Skill0HealingBomb.Init(healAmount, skillRange);
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
		}
	}
}
