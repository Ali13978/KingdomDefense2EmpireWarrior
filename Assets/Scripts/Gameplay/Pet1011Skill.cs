using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Pet1011Skill : HeroSkillCommon
	{
		public float flyToWoundedAllySpd = 3f;

		public float disToWoundedAlly = 1.5f;

		public float healDuration = 1f;

		public GameObject activateHealFx;

		private float atkbuffPercentage;

		private float hpBuffPercentage;

		private float cooldownDuration;

		private float healProportion;

		private HeroModel heroModel;

		private float cooldownCountdown;

		private bool isCastingSkill;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			base.Init(heroModel);
			this.heroModel = heroModel;
			PetConfigData petConfigData = heroModel.PetConfigData;
			atkbuffPercentage = petConfigData.Skillvalues[3];
			hpBuffPercentage = petConfigData.Skillvalues[4];
			healProportion = (float)petConfigData.Skillvalues[5] * 0.01f;
			cooldownDuration = (float)petConfigData.Skillvalues[6] * 0.001f;
			cooldownCountdown = cooldownDuration * 0.2f;
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_HEAL_0);
			HeroModel petOwner = heroModel.PetOwner;
			if (petOwner == null)
			{
				UnityEngine.Debug.LogError(" ower model is null");
			}
			petOwner.BuffsHolder.AddBuff("BuffAttackByPercentage", new Buff(isPositive: true, atkbuffPercentage, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
			petOwner.BuffsHolder.AddBuff("BuffHpByPercentage", new Buff(isPositive: true, hpBuffPercentage, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
		}

		public override void Update()
		{
			base.Update();
			cooldownCountdown -= Time.deltaTime;
			if (!(cooldownCountdown <= 0f) || isCastingSkill)
			{
				return;
			}
			cooldownCountdown = cooldownDuration;
			List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
			CharacterModel characterModel = null;
			float num = 2f;
			for (int num2 = listActiveAlly.Count - 1; num2 >= 0; num2--)
			{
				float num3 = (float)listActiveAlly[num2].GetCurHp() * 1f / (float)listActiveAlly[num2].GetMaxHp();
				if (GameTools.IsValidCharacter(listActiveAlly[num2]) && num > num3)
				{
					characterModel = listActiveAlly[num2];
					num = num3;
				}
			}
			if (characterModel != null && num < 1f)
			{
				StartCoroutine(CastSkill(characterModel));
			}
			else
			{
				cooldownCountdown = cooldownDuration * 0.1f;
			}
		}

		private IEnumerator CastSkill(CharacterModel pickedAlly)
		{
			isCastingSkill = true;
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			if (!GameTools.IsValidCharacter(pickedAlly))
			{
				isCastingSkill = false;
				yield break;
			}
			Vector3 healPos = pickedAlly.transform.position + new Vector3(0f - disToWoundedAlly, 0.3f, 0f);
			float flyTowardWoundedAllyDur = (healPos - heroModel.transform.position).magnitude / flyToWoundedAllySpd;
			heroModel.SetSpecialStateDuration(healDuration + flyTowardWoundedAllyDur);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animRun);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animRun);
			Transform transform = heroModel.transform;
			float x = healPos.x;
			Vector3 position = heroModel.transform.position;
			transform.localScale = new Vector3((x > position.x) ? 1 : (-1), 1f, 1f);
			heroModel.transform.DOMove(healPos, flyTowardWoundedAllyDur);
			yield return new WaitForSeconds(flyTowardWoundedAllyDur);
			if (GameTools.IsValidCharacter(pickedAlly))
			{
				heroModel.GetAnimationController().ToSpecialState(HeroAnimationController.animPassiveSkill_0, healDuration);
				Transform transform2 = heroModel.transform;
				Vector3 position2 = pickedAlly.transform.position;
				float x2 = position2.x;
				Vector3 position3 = heroModel.transform.position;
				transform2.localScale = new Vector3((x2 > position3.x) ? 1 : (-1), 1f, 1f);
				ObjectPool.Spawn(activateHealFx, heroModel.transform.position);
				int hpAmount = Mathf.RoundToInt((float)pickedAlly.GetMaxHp() * healProportion);
				pickedAlly.IncreaseHealth(hpAmount);
				EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_0);
				effect.transform.position = pickedAlly.transform.position;
				effect.Init(1.5f, pickedAlly.BuffsHolder.transform, pickedAlly.GetComponent<SpriteRenderer>().sprite.rect.width);
			}
			yield return new WaitForSeconds(healDuration);
			isCastingSkill = false;
		}
	}
}
