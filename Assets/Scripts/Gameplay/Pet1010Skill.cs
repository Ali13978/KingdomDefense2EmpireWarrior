using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Pet1010Skill : HeroSkillCommon
	{
		private float atkbuffPercentage;

		private float hpBuffPercentage;

		private float houndTotalMangicDam;

		private int numOfHoundProjectiles;

		private float houndDamageRange;

		private float houndCooldownDuration;

		public GameObject projectileDraRagePrefab;

		public GameObject explodeDraRagePrefab;

		public Transform barrelPos;

		public float moveToTargetSpeed;

		public float disToAttackPos;

		public float projFlyDuration = 0.3f;

		public float delayBtwShoot = 0.4f;

		private Vector3 targetPos;

		private int magicDamagePerHit;

		private float skillRange;

		private float houndCooldownCountdown;

		private HeroModel heroModel;

		private List<Dragon10Skill0Projectile> projList = new List<Dragon10Skill0Projectile>();

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			PetConfigData petConfigData = heroModel.PetConfigData;
			if (petConfigData == null)
			{
				UnityEngine.Debug.LogError(" pet config data is nulllllll ");
			}
			atkbuffPercentage = petConfigData.Skillvalues[0];
			hpBuffPercentage = petConfigData.Skillvalues[1];
			houndTotalMangicDam = petConfigData.Skillvalues[2];
			numOfHoundProjectiles = petConfigData.Skillvalues[3];
			houndDamageRange = (float)petConfigData.Skillvalues[4] / GameData.PIXEL_PER_UNIT;
			houndCooldownDuration = (float)petConfigData.Skillvalues[5] / 1000f;
			houndCooldownCountdown = houndCooldownDuration;
			HeroModel petOwner = heroModel.PetOwner;
			if (petOwner == null)
			{
				UnityEngine.Debug.LogError(" ower model is null");
			}
			petOwner.BuffsHolder.AddBuff("BuffAttackByPercentage", new Buff(isPositive: true, atkbuffPercentage, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
			petOwner.BuffsHolder.AddBuff("BuffHpByPercentage", new Buff(isPositive: true, hpBuffPercentage, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
			SingletonMonoBehaviour<MapController>.Instance.OnEnemyReachGate += OnEnemyReachGate;
		}

		public override void Update()
		{
			base.Update();
			for (int num = projList.Count - 1; num >= 0; num--)
			{
				if (!projList[num].OnUpdate(Time.deltaTime))
				{
					projList.RemoveAt(num);
					List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(targetPos, new CommonAttackDamage(0, magicDamagePerHit, isTargetAir: true, skillRange));
					for (int num2 = listEnemiesInRange.Count - 1; num2 >= 0; num2--)
					{
						if (GameTools.IsValidEnemy(listEnemiesInRange[num2]))
						{
							listEnemiesInRange[num2].ProcessDamage(DamageType.Magic, new CommonAttackDamage(0, magicDamagePerHit, isTargetAir: true, skillRange));
						}
					}
				}
			}
			houndCooldownCountdown -= Time.deltaTime;
		}

		private void OnEnemyReachGate(Vector2 targetPosition)
		{
			if (!(houndCooldownCountdown > 0f) && heroModel.IsAlive)
			{
				houndCooldownCountdown = houndCooldownDuration;
				StartCoroutine(CastSkill(new Vector3(targetPosition.x, targetPosition.y, 0f), (float)numOfHoundProjectiles * delayBtwShoot, houndDamageRange, houndTotalMangicDam, moveNearTarget: false));
			}
		}

		public void TriggerBabyDragonRage(Vector3 targetPos, float skillDuration, float skillRange, float magicDamage)
		{
			if (heroModel.IsAlive)
			{
				StartCoroutine(CastSkill(targetPos, skillDuration, skillRange, magicDamage));
			}
		}

		private IEnumerator CastSkill(Vector3 targetPos, float skillDuration, float skillRange, float magicDamage, bool moveNearTarget = true)
		{
			this.targetPos = targetPos;
			this.skillRange = skillRange;
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			float disToTarget = (targetPos - heroModel.transform.position).magnitude;
			float moveToTargetDuration = disToTarget / moveToTargetSpeed;
			int numOfJump = Mathf.CeilToInt(disToTarget / 2f);
			heroModel.SetSpecialStateDuration(skillDuration + ((!moveNearTarget) ? 0f : moveToTargetDuration));
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animRun);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animRun);
			float x = targetPos.x;
			Vector3 position = heroModel.transform.position;
			float lookSide = (x - position.x > 0f) ? 1 : (-1);
			heroModel.transform.localScale = new Vector3(lookSide, 1f, 1f);
			if (moveNearTarget)
			{
				ShortcutExtensions.DOJump(endValue: targetPos - new Vector3(lookSide * disToAttackPos, 0f, 0f), target: heroModel.transform, jumpPower: 0.7f, numJumps: numOfJump, duration: moveToTargetDuration).SetEase(Ease.Linear);
				yield return new WaitForSeconds(moveToTargetDuration);
			}
			heroModel.GetAnimationController().ToSpecialState(HeroAnimationController.animActiveSkill, skillDuration);
			int numOfProjectiles = Mathf.FloorToInt((skillDuration - projFlyDuration * 0.5f) / delayBtwShoot);
			magicDamagePerHit = Mathf.CeilToInt(magicDamage * 1f / (float)numOfProjectiles);
			for (int i = 0; i < numOfProjectiles; i++)
			{
				Vector2 offset = Random.insideUnitCircle * skillRange;
				Vector3 projTargetPos = targetPos + new Vector3(offset.x, offset.y, 0f);
				projList.Add(new Dragon10Skill0Projectile(projectileDraRagePrefab, explodeDraRagePrefab, barrelPos.position, projTargetPos, 2f, projFlyDuration * Random.Range(0.75f, 1.1f)));
				yield return new WaitForSeconds(delayBtwShoot);
			}
		}
	}
}
