using Data;
using DG.Tweening;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero10Skill3 : HeroSkillCommon
	{
		public GameObject lazeObj;

		public float disTriggerAttack = 1f;

		public float delayDealdam = 0.2f;

		public float beamDuration = 1f;

		private int heroID = 10;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private HeroModel heroModel;

		private Hero10Skill3Param skillParams;

		private float cooldownDuration;

		private float cooldownCountdown;

		private float skillRange;

		private float sqDisTriggerAttack;

		private bool unlocked;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unlocked = true;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroSkillParameter_10_3).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.5f;
			sqDisTriggerAttack = disTriggerAttack * disTriggerAttack;
			skillRange = (float)skillParams.skill_range / GameData.PIXEL_PER_UNIT;
			lazeObj.transform.localScale = new Vector3(1f, 0f, 1f);
			lazeObj.gameObject.SetActive(value: false);
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
				else if (IsEmptySpecialState() && GameTools.IsValidEnemy(heroModel.GetCurrentTarget()) && !heroModel.GetCurrentTarget().IsAir && SingletonMonoBehaviour<GameData>.Instance.SqrDistance(heroModel.transform.position, heroModel.GetCurrentTarget().transform.position) < sqDisTriggerAttack)
				{
					cooldownCountdown = cooldownDuration;
					StartCoroutine(CastSkill(heroModel.GetCurrentTarget().transform.position));
				}
			}
		}

		private IEnumerator CastSkill(Vector3 targetPos)
		{
			float moveToAtkPos = 0.5f;
			heroModel.SetSpecialStateDuration(delayDealdam + beamDuration + moveToAtkPos);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_2);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_2, delayDealdam + beamDuration);
			float disToTarget = 1.2f;
			Vector3 localScale = heroModel.transform.localScale;
			ShortcutExtensions.DOMove(endValue: targetPos + new Vector3((float)((!(localScale.x > 0f)) ? 1 : (-1)) * disToTarget, 0f, 0f), target: heroModel.transform, duration: moveToAtkPos);
			yield return new WaitForSeconds(moveToAtkPos);
			heroModel.GetAnimationController().ToSpecialState(HeroAnimationController.animPassiveSkill_2, delayDealdam + beamDuration);
			lazeObj.SetActive(value: true);
			lazeObj.transform.DOScaleY(1f, delayDealdam);
			yield return new WaitForSeconds(delayDealdam);
			SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
			EffectAttack ccData = new EffectAttack
			{
				buffKey = "Stun",
				damageFXType = DamageFXType.Stun,
				debuffChance = 100,
				debuffEffectDuration = (float)skillParams.stun_duration * 0.001f
			};
			List<EnemyModel> targetList = GameTools.GetListEnemiesInRange(targetPos, new CommonAttackDamage(0, skillParams.magic_damage, isTargetAir: false, skillRange));
			for (int num = targetList.Count - 1; num >= 0; num--)
			{
				if (GameTools.IsValidEnemy(targetList[num]))
				{
					targetList[num].ProcessDamage(DamageType.Magic, new CommonAttackDamage(0, skillParams.magic_damage, isTargetAir: true, skillRange), ccData);
				}
			}
			yield return new WaitForSeconds(beamDuration);
			lazeObj.transform.DOScaleY(0f, delayDealdam);
		}
	}
}
