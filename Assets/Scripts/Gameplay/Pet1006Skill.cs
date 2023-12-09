using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Pet1006Skill : HeroSkillCommon
	{
		private HeroModel heroModel;

		private bool unLock;

		private int damage;

		private int slowPercent;

		private float duration;

		private float cooldownTime;

		private float speed;

		private float skillRange;

		private float timeTracking;

		private string buffKey = "Slow";

		private EnemyModel target;

		[SerializeField]
		private float animationTime;

		public override void Update()
		{
			base.Update();
			if (unLock)
			{
				if (IsCooldownDone())
				{
					TryToCastSkill();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			slowPercent = heroModel.PetConfigData.Skillvalues[0];
			duration = heroModel.PetConfigData.Skillvalues[1];
			damage = heroModel.PetConfigData.Skillvalues[2];
			cooldownTime = heroModel.PetConfigData.Skillvalues[3];
			speed = heroModel.PetConfigData.Speed;
			skillRange = (float)heroModel.PetConfigData.Atk_range_max / GameData.PIXEL_PER_UNIT;
			timeTracking = cooldownTime;
			unLock = true;
			InitFXs();
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void TryToCastSkill()
		{
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			for (int num = listActiveEnemy.Count - 1; num >= 0; num--)
			{
				if (heroModel.IsInMeleeRange(listActiveEnemy[num]))
				{
					target = listActiveEnemy[num];
					break;
				}
			}
			if (GameTools.IsValidEnemy(target))
			{
				float specialStateDuration = GameTools.MoveToAttackPosition(heroModel, target, speed, OnMoveToTargetComplete);
				heroModel.SetSpecialStateDuration(specialStateDuration);
				heroModel.SetSpecialStateAnimationName(HeroAnimationController.animRun);
				heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState);
				target.SetSpecialStateDuration(specialStateDuration);
				target.SetSpecialStateAnimationName(EnemyAnimationController.animIdle);
				target.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animIdle);
				target.EnemyAnimationController.ToIdleState();
			}
			timeTracking = cooldownTime;
		}

		private void OnMoveToTargetComplete()
		{
			if (target != null && target.IsAlive)
			{
				CastSkill();
			}
		}

		private void CastSkill()
		{
			heroModel.SetSpecialStateDuration(animationTime);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
			heroModel.AddTarget(target);
			heroModel.LookAtEnemy();
			target.ProcessEffect(buffKey, slowPercent, duration, DamageFXType.Electric);
			ClearTarget();
		}

		private void ClearTarget()
		{
			target = null;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_ELECTRIC);
		}
	}
}
