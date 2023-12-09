using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Skill3 : HeroSkillCommon
	{
		private int heroID = 5;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int arrowNumber;

		private float skillRange;

		private int damagePhysics;

		private float cooldownTime;

		private float duration;

		private float delayTime;

		private float timeTracking;

		private string description;

		private bool isCastSkill;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private Hero5Skill3LightningSpear lightningSpear;

		[SerializeField]
		private Transform gunBarrel;

		private void Start()
		{
			SingletonMonoBehaviour<MapController>.Instance.OnEnemyReachGate += Instance_OnEnemyReachGate;
		}

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive))
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
			HeroSkillParameter_5_3 heroSkillParameter_5_ = new HeroSkillParameter_5_3();
			heroSkillParameter_5_ = (HeroSkillParameter_5_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			arrowNumber = heroSkillParameter_5_.getParam(currentSkillLevel - 1).arrow_number;
			skillRange = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			damagePhysics = heroSkillParameter_5_.getParam(currentSkillLevel - 1).damage_physics;
			cooldownTime = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			duration = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).duration / 1000f;
			delayTime = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).delay_time / 1000f;
			description = heroSkillParameter_5_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(lightningSpear.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.LIGHTNING_EXPLOSION_2);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void Instance_OnEnemyReachGate(Vector2 targetPosition)
		{
			TryToCastSkill(targetPosition);
		}

		private void TryToCastSkill(Vector2 targetPosition)
		{
			if (unLock && IsCooldownDone() && !isCastSkill)
			{
				StartCoroutine(CastSkill(targetPosition));
			}
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			UnityEngine.Debug.Log("Chironia cast skill!");
			isCastSkill = true;
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
			for (int i = 0; i < arrowNumber; i++)
			{
				Hero5Skill3LightningSpear lSpear = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHero5Skill3LightningSpear();
				lSpear.transform.position = gunBarrel.position;
				lSpear.Init(targetPosition: targetPosition + Random.insideUnitCircle * 0.3f, skillRange: skillRange, damagePhysics: damagePhysics, duration: duration);
				yield return new WaitForSeconds(delayTime);
			}
			timeTracking = cooldownTime;
			isCastSkill = false;
		}
	}
}
