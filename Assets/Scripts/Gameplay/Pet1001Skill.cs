using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Pet1001Skill : HeroSkillCommon
	{
		public bool readMaxTargetHpFromConfig;

		private HeroModel heroModel;

		private bool unLock;

		private float dragBackValue;

		private float cooldownTime;

		private float speed;

		private float timeTracking;

		private bool isCastingSkill;

		private bool isMoveToTarget;

		private EnemyModel target;

		private Vector3 targetCachePosition = Vector3.zero;

		[SerializeField]
		private float animationTime;

		[SerializeField]
		private Transform attachPoint;

		private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

		private Vector3 outsidePosition = Vector3.zero;

		private int maxTargetHp = 300;

		public override void Update()
		{
			base.Update();
			if (!unLock)
			{
				return;
			}
			if (isMoveToTarget)
			{
				ChangeAnimationRun(heroModel.transform.position, targetCachePosition);
				return;
			}
			if (isCastingSkill)
			{
				ChangeAnimationRun(heroModel.transform.position, outsidePosition);
				return;
			}
			if (IsCooldownDone())
			{
				TryToCastSkill();
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			dragBackValue = heroModel.PetConfigData.Skillvalues[0];
			cooldownTime = (float)heroModel.PetConfigData.Skillvalues[1] / 1000f;
			if (readMaxTargetHpFromConfig)
			{
				maxTargetHp = heroModel.PetConfigData.Skillvalues[2];
			}
			speed = heroModel.PetConfigData.Speed;
			timeTracking = 2f;
			unLock = true;
			isCastingSkill = false;
			isMoveToTarget = false;
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void GetTarget()
		{
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			int num = listActiveEnemy.Count - 1;
			while (true)
			{
				if (num >= 0)
				{
					if (heroModel.IsInMeleeRange(listActiveEnemy[num]) && !listActiveEnemy[num].IsUnderground && !listActiveEnemy[num].IsInTunnel && !EnemyParameterManager.Instance.IsBoss(listActiveEnemy[num].Id) && listActiveEnemy[num].EnemyHealthController.OriginHealth <= maxTargetHp)
					{
						break;
					}
					num--;
					continue;
				}
				return;
			}
			target = listActiveEnemy[num];
		}

		private void TryToCastSkill()
		{
			GetTarget();
			if (target != null)
			{
				if (target.IsAlive)
				{
					float specialStateDuration = GameTools.MoveToAttackPosition(heroModel, target, speed, OnMoveToTargetComplete);
					heroModel.GetAnimationController().ToRunState();
					isMoveToTarget = true;
					targetCachePosition = target.transform.position;
					ChangeAnimationRun(heroModel.transform.position, target.transform.position);
					target.SetSpecialStateDuration(specialStateDuration);
					target.SetSpecialStateAnimationName(EnemyAnimationController.animIdle);
					target.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animIdle);
					target.EnemyAnimationController.ToIdleState();
				}
				timeTracking = cooldownTime;
			}
			else
			{
				timeTracking = 1f;
			}
		}

		private void OnMoveToTargetComplete()
		{
			if (target != null && target.IsAlive)
			{
				StartCoroutine(CastSkill());
			}
			isMoveToTarget = false;
		}

		private IEnumerator CastSkill()
		{
			isCastingSkill = true;
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(animationTime);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
			yield return new WaitForSeconds(animationTime);
			outsidePosition = GetOutsideVector();
			float timeToMoveToTarget = GameTools.TimeMoveToPosition(heroModel, 2f * speed, outsidePosition, OnMoveOutsideComplete);
			heroModel.GetAnimationController().ToRunState();
			if (GameTools.IsValidEnemy(target))
			{
				target.SetSpecialStateDuration(timeToMoveToTarget);
				target.SetSpecialStateAnimationName(EnemyAnimationController.animIdle);
				target.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animIdle);
				target.EnemyAnimationController.ToIdleState();
				target.IsInTunnel = true;
				target.transform.SetParent(attachPoint);
				target.transform.localPosition = Vector3.zero;
				target.transform.localScale = Vector3.one;
				target.EnemyHealthController.HideHealthBar();
			}
		}

		private void OnMoveOutsideComplete()
		{
			target.transform.SetParent(null);
			if (target.IsAlive)
			{
				if (EnemyParameterManager.Instance.IsEnemyHasMoreThanOneLife(target.Id))
				{
					GameData instance = SingletonMonoBehaviour<GameData>.Instance;
					int totalEnemy = instance.TotalEnemy;
					Enemy originalParameter = target.OriginalParameter;
					instance.TotalEnemy = totalEnemy - originalParameter.lifeCount;
				}
				else
				{
					SingletonMonoBehaviour<GameData>.Instance.TotalEnemy--;
				}
				target.gameObject.SetActive(value: false);
				target.Dead();
				target.ReturnPool(5f);
			}
			target = null;
			isCastingSkill = false;
		}

		private void ChangeAnimationRun(Vector3 currentPosition, Vector3 assignedPosition)
		{
			if (assignedPosition.x - currentPosition.x > 0f)
			{
				heroModel.transform.localScale = Vector3.one;
			}
			else
			{
				heroModel.transform.localScale = invertXVector;
			}
		}

		private Vector3 GetOutsideVector()
		{
			Vector3 result = new Vector3(0f, 0f, 0f);
			if (Random.Range(0, 100) < 50)
			{
				float newX = 7f;
				float newY = Random.Range(-5f, 5f);
				result.Set(newX, newY, 0f);
			}
			else
			{
				float newX2 = -7f;
				float newY2 = Random.Range(-5f, 5f);
				result.Set(newX2, newY2, 0f);
			}
			return result;
		}
	}
}
