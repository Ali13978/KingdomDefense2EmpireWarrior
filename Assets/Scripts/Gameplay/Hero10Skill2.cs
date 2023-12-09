using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero10Skill2 : HeroSkillCommon
	{
		public GameObject biteFxPrefab;

		public float disTriggerAttack = 2f;

		public float delayDealdam = 0.3f;

		private int heroID = 10;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private HeroModel heroModel;

		private Hero10Skill2Param skillParams;

		private float cooldownDuration;

		private float cooldownCountdown;

		private float sqDisTriggerAttack;

		private bool unlocked;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unlocked = true;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroSkillParameter_10_2).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.2f;
			sqDisTriggerAttack = disTriggerAttack * disTriggerAttack;
		}

		public override void Update()
		{
			base.Update();
			if (unlocked)
			{
				if (cooldownCountdown > 0f)
				{
					cooldownCountdown -= Time.deltaTime;
				}
				else if (IsEmptySpecialState() && GameTools.IsValidEnemy(heroModel.GetCurrentTarget()) && SingletonMonoBehaviour<GameData>.Instance.SqrDistance(heroModel.transform.position, heroModel.GetCurrentTarget().transform.position) < sqDisTriggerAttack)
				{
					cooldownCountdown = cooldownDuration;
					StartCoroutine(CastSkill(heroModel.GetCurrentTarget()));
				}
			}
		}

		private IEnumerator CastSkill(EnemyModel target)
		{
			Vector3 targetPos = target.transform.position;
			heroModel.SetSpecialStateDuration(delayDealdam * 3f);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_1);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_1);
			yield return new WaitForSeconds(delayDealdam);
			ObjectPool.Spawn(biteFxPrefab, targetPos);
			if (GameTools.IsValidEnemy(target))
			{
				target.ProcessDamage(DamageType.Melee, new CommonAttackDamage(skillParams.physic_damage, 0, isTargetAir: true));
			}
		}
	}
}
