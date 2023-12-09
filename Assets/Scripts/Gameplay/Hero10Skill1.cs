using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero10Skill1 : HeroSkillCommon
	{
		public GameObject steelFxPrefab;

		public float disTriggerAttack = 1f;

		public float delayDealdam = 0.3f;

		public int numOfSteelProj = 5;

		private int heroID = 10;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private float sqDisTriggerAttack;

		private float cooldownDuration;

		private float cooldownCountdown;

		private float skillRange;

		private HeroModel heroModel;

		private Hero10Skill1Param skillParams;

		private bool unlocked;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unlocked = true;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroSkillParameter_10_1).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.7f;
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
					StartCoroutine(CastSkill(heroModel.GetCurrentTarget().transform.position));
				}
			}
		}

		private IEnumerator CastSkill(Vector3 targetPos)
		{
			heroModel.SetSpecialStateDuration(delayDealdam * 2.5f);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0, delayDealdam * 2.5f);
			yield return new WaitForSeconds(0.3f);
			ObjectPool.Spawn(steelFxPrefab, targetPos);
			for (int i = 0; i < numOfSteelProj - 1; i++)
			{
				Vector2 offset = Random.insideUnitCircle * skillRange;
				ObjectPool.Spawn(position: targetPos + new Vector3(offset.x, offset.y, 0f), prefab: steelFxPrefab);
				yield return new WaitForSeconds(0.12f);
				SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
			}
			EffectAttack ccData = new EffectAttack
			{
				buffKey = "DefDown",
				damageFXType = DamageFXType.DefDown,
				debuffChance = 100,
				debuffEffectDuration = (float)skillParams.def_down_duration * 0.001f,
				debuffEffectValue = skillParams.def_down_percent
			};
			List<EnemyModel> targetList = GameTools.GetListEnemiesInRange(targetPos, new CommonAttackDamage(0, skillParams.magic_damage, isTargetAir: false, skillRange));
			for (int num = targetList.Count - 1; num >= 0; num--)
			{
				if (GameTools.IsValidEnemy(targetList[num]))
				{
					targetList[num].ProcessDamage(DamageType.Magic, new CommonAttackDamage(0, skillParams.magic_damage, isTargetAir: true, skillRange), ccData);
				}
			}
		}
	}
}
