using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Skill0 : HeroSkillCommon
	{
		private int heroID = 4;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int physicsDamage;

		private float skillRange;

		private float duration;

		private float cooldownTime;

		private string description;

		private string useType;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		private string buffKey = "Slow";

		[SerializeField]
		private Hero4Skill0Breakdown breakdown;

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
			HeroSkillParameter_4_0 heroSkillParameter_4_ = new HeroSkillParameter_4_0();
			heroSkillParameter_4_ = (HeroSkillParameter_4_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_4_.getParam(currentSkillLevel - 1).physics_damage;
			skillRange = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			duration = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_4_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_4_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(breakdown.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_STUN);
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
			yield return new WaitForSeconds(delayTimeCastSkill);
			Breakdown(targetPosition);
		}

		private void Breakdown(Vector2 targetPosition)
		{
			Hero4Skill0Breakdown hero4Skill0Breakdown = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHero4Skill0Breakdown();
			hero4Skill0Breakdown.transform.position = targetPosition;
			hero4Skill0Breakdown.Init(physicsDamage, skillRange, buffKey, duration);
			SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
		}
	}
}
