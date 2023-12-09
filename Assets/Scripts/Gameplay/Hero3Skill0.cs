using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Skill0 : HeroSkillCommon
	{
		private int heroID = 3;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int physicsDamage;

		private int magicDamage;

		private float skillRange;

		private float cooldownTime;

		private float meteorSpeed;

		private float duration;

		private string description;

		private string useType;

		[SerializeField]
		private Hero3Skill0Meteor meteorPrefab;

		private float offsetHigh = 6f;

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
			HeroSkillParameter_3_0 heroSkillParameter_3_ = new HeroSkillParameter_3_0();
			heroSkillParameter_3_ = (HeroSkillParameter_3_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_3_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_3_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			meteorSpeed = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).meteor_speed / GameData.PIXEL_PER_UNIT;
			duration = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_3_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_3_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(meteorPrefab.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.METEOR_EXPLOSION2);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.METEOR_SELF_EXPLOSION);
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
			heroModel.SetSpecialStateDuration(1f);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
			yield return new WaitForSeconds(0.5f);
			CreateMeteor(targetPosition);
		}

		private void CreateMeteor(Vector2 targetPosition)
		{
			Hero3Skill0Meteor hero3Skill0Meteor = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHero3Skill0Meteor();
			hero3Skill0Meteor.transform.position = new Vector2(targetPosition.x, offsetHigh);
			hero3Skill0Meteor.Init(physicsDamage, magicDamage, skillRange, duration, meteorSpeed, base.transform.position, targetPosition, offsetHigh - targetPosition.y);
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[3].DoCooldown();
		}
	}
}
