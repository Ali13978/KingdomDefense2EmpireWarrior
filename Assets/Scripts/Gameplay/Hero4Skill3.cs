using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Skill3 : HeroSkillCommon
	{
		public CFX_AutoPlayAndDestruction attackUpFxSample;

		private int heroID = 4;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int generalAttackDamageBonus;

		private int attackSpeedBonus;

		private int movementSpeedBonus;

		private float duration;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		private string increasePhysicsDamageBuffkey = "IncreaseDamagePhysics";

		private string increaseMagicDamageBuffkey = "IncreaseDamageMagic";

		private string increaseAttackSpeedBuffkey = "IncreaseAttackSpeed";

		private string increaseMovementSpeedBuffkey = "IncreaseMovementSpeed";

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive))
			{
				if (IsCooldownDone())
				{
					StartCoroutine(CastSkill());
				}
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
			HeroSkillParameter_4_3 heroSkillParameter_4_ = new HeroSkillParameter_4_3();
			heroSkillParameter_4_ = (HeroSkillParameter_4_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			generalAttackDamageBonus = heroSkillParameter_4_.getParam(currentSkillLevel - 1).general_attack_damage_bonus;
			attackSpeedBonus = heroSkillParameter_4_.getParam(currentSkillLevel - 1).attack_speed_bonus;
			movementSpeedBonus = heroSkillParameter_4_.getParam(currentSkillLevel - 1).movement_speed_bonus;
			duration = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_4_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime * Random.Range(0.7f, 0.9f);
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.INFERNO_GOLEM_AURA);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(1f);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_2);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_2);
			AbilitiesBuff();
		}

		private void AbilitiesBuff()
		{
			List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
			foreach (CharacterModel item in listActiveAlly)
			{
				item.BuffsHolder.AddBuff(increasePhysicsDamageBuffkey, new Buff(isPositive: true, generalAttackDamageBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				item.BuffsHolder.AddBuff(increaseMagicDamageBuffkey, new Buff(isPositive: true, generalAttackDamageBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				item.BuffsHolder.AddBuff(increaseAttackSpeedBuffkey, new Buff(isPositive: true, attackSpeedBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				item.BuffsHolder.AddBuff(increaseMovementSpeedBuffkey, new Buff(isPositive: true, movementSpeedBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.INFERNO_GOLEM_AURA);
				effect.transform.position = item.transform.position;
				effect.Init(duration, item.transform);
				CFX_AutoPlayAndDestruction cFX_AutoPlayAndDestruction = ObjectPool.Spawn(attackUpFxSample, item.BuffsHolder.transform, new Vector3(0f, item.GetCharacterHeight() + 0.05f, 0f));
				cFX_AutoPlayAndDestruction.SetTimer(duration + 1f);
			}
			timeTracking = cooldownTime;
		}
	}
}
