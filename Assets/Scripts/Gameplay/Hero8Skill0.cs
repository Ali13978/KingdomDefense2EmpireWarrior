using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero8Skill0 : HeroSkillCommon
	{
		private int heroID = 8;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int numberOfProjectile;

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
		private float delayTimeBetweenMissile;

		[Space]
		[SerializeField]
		private BulletModel missilePrefab;

		[SerializeField]
		private string missileName;

		[SerializeField]
		private Transform[] listGunBarrel;

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
			HeroSkillParameter_8_0 heroSkillParameter_8_ = new HeroSkillParameter_8_0();
			heroSkillParameter_8_ = (HeroSkillParameter_8_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberOfProjectile = heroSkillParameter_8_.getParam(currentSkillLevel - 1).number_of_projectile;
			physicsDamage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_8_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_8_.getParam(currentSkillLevel - 1).use_type;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(missilePrefab.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.GROUND_AIMING_1);
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
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
			yield return new WaitForSeconds(delayTimeToAttack);
			EffectController fx = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.GROUND_AIMING_1);
			fx.Init(3.25f, targetPosition);
			for (int i = 0; i < numberOfProjectile; i++)
			{
				yield return new WaitForSeconds(delayTimeBetweenMissile);
				BulletModel bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetBulletByName(missileName);
				bullet.transform.position = listGunBarrel[Random.Range(0, listGunBarrel.Length)].position;
				bullet.InitFromHero(heroModel, new CommonAttackDamage(physicsDamage, magicDamage, skillRange), targetPosition);
			}
		}
	}
}
