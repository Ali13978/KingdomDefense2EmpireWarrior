using Data;
using DG.Tweening;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero10Skill0 : HeroSkillCommon
	{
		public GameObject projectilePrefab;

		public GameObject explodePrefab;

		public Transform barrelPos;

		public float moveToTargetDuration;

		public float disToAttackPos;

		public float projFlyDuration = 0.3f;

		public float delayBtwShoot = 0.2f;

		private int heroID = 10;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private Hero10Skill0Param skillParams;

		private float skillDuration;

		private float skillRange;

		private HeroModel heroModel;

		private List<Dragon10Skill0Projectile> projList = new List<Dragon10Skill0Projectile>();

		private Vector3 targetPos3;

		private int magicDamagePerHit;

		private void Start()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition += OnHeroCastActiveSkill;
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition -= OnHeroCastActiveSkill;
		}

		public override string GetUseType()
		{
			return skillParams.use_type;
		}

		public override float GetCooldownTime()
		{
			return (float)skillParams.cooldown_time * 0.001f;
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroSkillParameter_10_0).listParam[currentSkillLevel - 1];
			skillDuration = (float)skillParams.duration * 0.001f;
			skillRange = (float)skillParams.skill_range / GameData.PIXEL_PER_UNIT;
		}

		public void OnHeroCastActiveSkill(int heroId, Vector2 targetPos)
		{
			if (heroID == heroId)
			{
				StartCoroutine(CastSkill(targetPos));
			}
		}

		private IEnumerator CastSkill(Vector2 targetPos)
		{
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[heroID].DoCooldown();
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(skillDuration + moveToTargetDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animRun);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animRun);
			targetPos3 = new Vector3(targetPos.x, targetPos.y, 0f);
			if (heroModel.GetPet() != null)
			{
				(heroModel.GetPet().HeroSkillController.GetSkill(0) as Pet1010Skill).TriggerBabyDragonRage(targetPos3, skillDuration + moveToTargetDuration, skillRange, skillParams.magic_damage);
			}
			float x = targetPos.x;
			Vector3 position = heroModel.transform.position;
			float lookSide = (x - position.x > 0f) ? 1 : (-1);
			heroModel.transform.localScale = new Vector3(lookSide, 1f, 1f);
			ShortcutExtensions.DOMove(endValue: targetPos3 - new Vector3(lookSide * disToAttackPos, 0f, 0f), target: heroModel.transform, duration: moveToTargetDuration).SetEase(Ease.Linear);
			yield return new WaitForSeconds(moveToTargetDuration);
			heroModel.GetAnimationController().ToSpecialState(HeroAnimationController.animActiveSkill, skillDuration);
			int numOfProjectiles = Mathf.FloorToInt((skillDuration - projFlyDuration * 0.5f) / delayBtwShoot);
			magicDamagePerHit = Mathf.CeilToInt((float)skillParams.magic_damage * 1f / (float)numOfProjectiles);
			for (int i = 0; i < numOfProjectiles; i++)
			{
				Vector2 offset = Random.insideUnitCircle * skillRange;
				Vector3 projTargetPos = targetPos3 + new Vector3(offset.x, offset.y, 0f);
				projList.Add(new Dragon10Skill0Projectile(projectilePrefab, explodePrefab, barrelPos.position, projTargetPos, 0.1f, projFlyDuration * Random.Range(0.75f, 1.1f)));
				yield return new WaitForSeconds(delayBtwShoot);
			}
		}

		public override void Update()
		{
			base.Update();
			for (int num = projList.Count - 1; num >= 0; num--)
			{
				if (!projList[num].OnUpdate(Time.deltaTime))
				{
					projList.RemoveAt(num);
					List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(targetPos3, new CommonAttackDamage(0, magicDamagePerHit, isTargetAir: true, skillRange));
					for (int num2 = listEnemiesInRange.Count - 1; num2 >= 0; num2--)
					{
						if (GameTools.IsValidEnemy(listEnemiesInRange[num2]))
						{
							listEnemiesInRange[num2].ProcessDamage(DamageType.Magic, new CommonAttackDamage(0, magicDamagePerHit, isTargetAir: true, skillRange));
						}
					}
				}
			}
		}
	}
}
