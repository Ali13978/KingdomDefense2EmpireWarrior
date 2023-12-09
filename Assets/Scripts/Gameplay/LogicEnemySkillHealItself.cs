using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class LogicEnemySkillHealItself : EnemyController
	{
		[Space]
		[Header("Parameters")]
		[SerializeField]
		private int healthRestoreAmount;

		[SerializeField]
		private float criticalHealthPercentage;

		[SerializeField]
		private float coolDownTimeMillisecond;

		[SerializeField]
		private float minSpeed = 0.05f;

		[SerializeField]
		private float attackAnimationDuration = 1f;

		[SerializeField]
		private bool isActiveSkillOnAppear;

		private float coolDownTime;

		private float coolDownTimeTracking;

		private bool skillReady;

		private bool isCastingSkill;

		private WaitForSeconds waitForAnimation;

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnAppear()
		{
			base.OnAppear();
			coolDownTime = coolDownTimeMillisecond / 1000f;
			if (isActiveSkillOnAppear)
			{
				coolDownTimeTracking = 0f;
			}
			else
			{
				coolDownTimeTracking = coolDownTime;
			}
			skillReady = true;
			isCastingSkill = false;
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_HEAL_0);
		}

		private void Start()
		{
			waitForAnimation = new WaitForSeconds(attackAnimationDuration);
		}

		public override void Update()
		{
			base.Update();
			if (skillReady && !isCastingSkill && IsEnemyAlive() && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				if (IsCooldownSkillDone() && ShouldCastSkill())
				{
					StartCoroutine(CastSkillHeal());
				}
				coolDownTimeTracking = Mathf.MoveTowards(coolDownTimeTracking, 0f, Time.deltaTime);
			}
		}

		private bool IsCooldownSkillDone()
		{
			return coolDownTimeTracking == 0f;
		}

		private bool ShouldCastSkill()
		{
			bool result = false;
			if ((float)base.EnemyModel.EnemyHealthController.CurrentHealth <= criticalHealthPercentage / 100f * (float)base.EnemyModel.EnemyHealthController.OriginHealth)
			{
				result = true;
			}
			return result;
		}

		private IEnumerator CastSkillHeal()
		{
			isCastingSkill = true;
			if (!IsCurrentSpeedGreaterThanMinSpeed())
			{
				yield return null;
			}
			if (!IsEnemyAlive())
			{
				yield return null;
			}
			base.EnemyModel.SetSpecialStateDuration(attackAnimationDuration);
			base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimationController.animSpecialAttack);
			base.EnemyModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animSpecialAttack);
			base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
			base.EnemyModel.EnemyHealthController.AddHealth(healthRestoreAmount);
			EffectController fx = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_0);
			fx.Init(attackAnimationDuration, base.EnemyModel.transform);
			UnityEngine.Debug.Log("heal " + healthRestoreAmount);
			yield return waitForAnimation;
			coolDownTimeTracking = coolDownTime;
			isCastingSkill = false;
		}

		public void OnDisable()
		{
			isCastingSkill = false;
		}
	}
}
