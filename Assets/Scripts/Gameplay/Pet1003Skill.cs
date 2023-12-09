using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Pet1003Skill : HeroSkillCommon
	{
		private enum NightwingState
		{
			cooldown,
			findTarget,
			attack
		}

		public Transform gunBarrel;

		private int minPhysicsDam;

		private int maxPhysicsDam;

		private float damageToHpRatio;

		private float buffHpPercent;

		private float delayAttack;

		private float detectRange;

		private HeroModel petModel;

		private HeroModel ownerModel;

		private EnemyModel curTarget;

		private GameObject projectile;

		private float countdown;

		private float atkDuration = 0.3f;

		private float flyHeight = 0.3f;

		private NightwingState curState;

		private CommonAttackDamage commonAttackDamageSender;

		private string explodeFx = "FireBall";

		public override void Init(HeroModel heroModel)
		{
			minPhysicsDam = heroModel.PetConfigData.Skillvalues[0];
			maxPhysicsDam = heroModel.PetConfigData.Skillvalues[1];
			damageToHpRatio = heroModel.PetConfigData.Skillvalues[2] / 100;
			buffHpPercent = heroModel.PetConfigData.Skillvalues[3];
			delayAttack = (float)heroModel.PetConfigData.Skillvalues[4] / 1000f;
			detectRange = (float)heroModel.PetConfigData.Skillvalues[5] / GameData.PIXEL_PER_UNIT;
			petModel = heroModel;
			ownerModel = petModel.PetOwner;
			ownerModel.BuffsHolder.AddBuff("BuffHpByPercentage", new Buff(isPositive: true, buffHpPercent, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
			TrashMan.InitPool(GameTools.pet1003BulletPath);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_HEAL_0);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(explodeFx);
		}

		public override void Update()
		{
			base.Update();
			switch (curState)
			{
			case NightwingState.cooldown:
				countdown -= Time.deltaTime;
				if (countdown <= 0f)
				{
					curState = NightwingState.findTarget;
				}
				break;
			case NightwingState.findTarget:
				countdown -= Time.deltaTime;
				if (countdown <= 0f)
				{
					countdown = 0.3f;
					curTarget = FindTarget();
					if (curTarget != null)
					{
						StartCoroutine(CastSkill());
					}
				}
				break;
			}
		}

		public EnemyModel FindTarget(float customDetectRange = -1f)
		{
			if (customDetectRange < 0f)
			{
				customDetectRange = detectRange;
			}
			float num = customDetectRange * customDetectRange;
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			for (int num2 = listActiveEnemy.Count - 1; num2 >= 0; num2--)
			{
				if (!listActiveEnemy[num2].IsUnderground && SingletonMonoBehaviour<GameData>.Instance.SqrDistance(petModel.transform.position, listActiveEnemy[num2].transform.position) < num)
				{
					return listActiveEnemy[num2];
				}
			}
			return null;
		}

		public IEnumerator CastSkill()
		{
			curState = NightwingState.attack;
			GameTools.CalculateAttackPosition(petModel, curTarget, petModel.GetSpeed() * 2f, out Vector3 attackPos, out float movingTime);
			Vector3 startPos = petModel.transform.position;
			petModel.SetSpecialStateDuration(movingTime * 2f);
			petModel.SetSpecialStateAnimationName(HeroAnimationController.animRun);
			petModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState);
			curTarget.SetSpecialStateDuration(movingTime + atkDuration + 0.1f);
			curTarget.SetSpecialStateAnimationName(EnemyAnimationController.animIdle);
			curTarget.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState);
			LookTowardTargetPos(attackPos);
			petModel.transform.DOJump(attackPos, flyHeight, 1, movingTime);
			yield return new WaitForSeconds(movingTime);
			if (FireProjectile())
			{
				petModel.GetAnimationController().ToSpecialState(HeroAnimationController.animActiveSkill, atkDuration);
				yield return new WaitForSeconds(atkDuration);
				if (projectile != null)
				{
					TrashMan.despawn(projectile);
				}
				DealDamageToTarget();
			}
			yield return new WaitForSeconds(0.1f);
			petModel.GetAnimationController().ToRunState();
			LookTowardTargetPos(startPos);
			petModel.transform.DOJump(startPos, 0f - flyHeight, 1, movingTime);
			yield return new WaitForSeconds(movingTime);
			countdown = delayAttack;
			curState = NightwingState.cooldown;
		}

		private void LookTowardTargetPos(Vector3 targetPos)
		{
			float x = targetPos.x;
			Vector3 position = petModel.transform.position;
			if (x > position.x)
			{
				petModel.transform.localScale = Vector3.one;
			}
			else
			{
				petModel.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}

		public bool FireProjectile()
		{
			if (!GameTools.IsValidEnemy(curTarget))
			{
				curTarget = FindTarget(detectRange * 0.5f);
			}
			if (!GameTools.IsValidEnemy(curTarget))
			{
				return false;
			}
			LookTowardTargetPos(curTarget.transform.position);
			projectile = TrashMan.spawn(GameTools.pet1003BulletName, gunBarrel.transform.position);
			projectile.transform.up = (gunBarrel.transform.position - curTarget.transform.position).normalized;
			projectile.transform.DOMove(curTarget.transform.position, atkDuration);
			return true;
		}

		private void DealDamageToTarget()
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = Random.Range(minPhysicsDam, maxPhysicsDam + 1);
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(explodeFx);
			effect.transform.position = curTarget.transform.position;
			effect.Init(1f);
			int currentHealth = curTarget.EnemyHealthController.CurrentHealth;
			curTarget.ProcessDamage(DamageType.Range, commonAttackDamageSender);
			int currentHealth2 = curTarget.EnemyHealthController.CurrentHealth;
			if (currentHealth2 < currentHealth && ownerModel.IsAlive)
			{
				int hpAmount = Mathf.RoundToInt((float)(currentHealth - currentHealth2) * damageToHpRatio);
				ownerModel.IncreaseHealth(hpAmount);
				EffectController effect2 = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_0);
				effect2.transform.position = ownerModel.transform.position;
				effect2.Init(1.5f, ownerModel.transform, ownerModel.GetComponent<SpriteRenderer>().sprite.rect.width);
			}
		}
	}
}
