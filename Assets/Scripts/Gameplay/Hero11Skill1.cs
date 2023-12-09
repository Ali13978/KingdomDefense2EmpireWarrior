using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero11Skill1 : HeroSkillCommon
	{
		public float novaCrashDuration;

		public float delayDealdam;

		public float disTriggerAttack = 1f;

		private int heroID = 11;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private HeroModel heroModel;

		private Hero11Skill1Param skillParams;

		private float cooldownDuration;

		private float cooldownCountdown;

		private float skillRange;

		private float sqDisTriggerAttack;

		private bool unlocked;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unlocked = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroSkillParameter_11_1).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.1f;
			sqDisTriggerAttack = disTriggerAttack * disTriggerAttack;
			skillRange = (float)skillParams.skill_range / GameData.PIXEL_PER_UNIT;
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
					StartCoroutine(CastSkill());
				}
			}
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(novaCrashDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
			yield return new WaitForSeconds(delayDealdam);
			SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
			List<EnemyModel> targetList = GameTools.GetListEnemiesInRange(heroModel.transform.position, new CommonAttackDamage(0, skillParams.magic_damage, isTargetAir: false, skillRange));
			for (int num = targetList.Count - 1; num >= 0; num--)
			{
				if (GameTools.IsValidEnemy(targetList[num]))
				{
					targetList[num].ProcessDamage(DamageType.Magic, new CommonAttackDamage(0, skillParams.magic_damage, isTargetAir: true, skillRange));
				}
			}
		}
	}
}
