using Data;
using DG.Tweening;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero11Skill0 : HeroSkillCommon
	{
		public GameObject explodePrefab;

		public GameObject behitOnEnemyFx;

		public float flyL2RDuration;

		public float outsideX;

		public float delayCreateExplode = 0.3f;

		private int heroID = 11;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private Hero11Skill0Param skillParams;

		private float skillHalfHeight;

		private int fireRoadDamage;

		private float fireRoadDuration;

		private HeroModel heroModel;

		private float moveOutDur = 0.5f;

		private float countdownCreateExplode;

		private void Start()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition += OnHeroCastActiveSkill;
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition -= OnHeroCastActiveSkill;
		}

		public override float GetCooldownTime()
		{
			return (float)skillParams.cooldown_time * 0.001f;
		}

		public override string GetUseType()
		{
			return skillParams.use_type;
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroSkillParameter_11_0).listParam[currentSkillLevel - 1];
			skillHalfHeight = (float)skillParams.skill_range / GameData.PIXEL_PER_UNIT;
			fireRoadDamage = skillParams.magic_damage;
			fireRoadDuration = (float)skillParams.fire_road_duration * 0.001f;
			GameEventCenter.Instance.Subscribe(GameEventType.OnAfterCalculateMagicDamage, new DamageInfoSubscriberData(GameTools.GetUniqueId(), OnAfterCalculateDamage));
		}

		public void OnAfterCalculateDamage(CommonAttackDamage damageInfo)
		{
			if (damageInfo.sourceId == heroModel.HeroID)
			{
				if (damageInfo.targetEnemyModel.EnemyHealthController.CurrentHealth <= damageInfo.magicDamage)
				{
					ObjectPool.Spawn(behitOnEnemyFx, damageInfo.targetEnemyModel.transform.position);
				}
				else
				{
					ObjectPool.Spawn(behitOnEnemyFx, damageInfo.targetEnemyModel.transform, new Vector3(Random.Range(-0.06f, 0.06f), Random.Range(0.03f, 0.1f), 0f));
				}
			}
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
			heroModel.SetSpecialStateDuration(flyL2RDuration + moveOutDur * 2f);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
			Vector3 oldPos = heroModel.transform.position;
			heroModel.transform.localScale = new Vector3(1f, 1f, 1f);
			float x = outsideX;
			Vector3 position = heroModel.transform.position;
			new Vector3(x, position.y, 0f);
			heroModel.transform.DOMoveX(outsideX, moveOutDur);
			yield return new WaitForSeconds(moveOutDur);
			heroModel.transform.position = new Vector3(0f - outsideX, targetPos.y, 0f);
			heroModel.transform.DOMove(new Vector3(outsideX, targetPos.y, 0f), flyL2RDuration).SetEase(Ease.Linear);
			float flyL2RCountdown = flyL2RDuration;
			while (flyL2RCountdown > 0f)
			{
				yield return null;
				flyL2RCountdown -= Time.deltaTime;
				countdownCreateExplode -= Time.deltaTime;
				if (countdownCreateExplode <= 0f)
				{
					countdownCreateExplode = delayCreateExplode;
					SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
					ObjectPool.Spawn(explodePrefab, heroModel.transform.position);
					List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(heroModel.transform.position, new CommonAttackDamage(0, 0, isTargetAir: true, skillHalfHeight));
					for (int i = 0; i < listEnemiesInRange.Count; i++)
					{
						listEnemiesInRange[i].ProcessDamage(DamageType.Magic, new CommonAttackDamage(0, fireRoadDamage));
					}
				}
			}
			heroModel.transform.position = new Vector3(0f - outsideX, oldPos.y, 0f);
			heroModel.transform.DOMove(oldPos, moveOutDur).SetEase(Ease.Linear);
		}
	}
}
