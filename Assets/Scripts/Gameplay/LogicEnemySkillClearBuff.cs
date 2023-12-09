using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class LogicEnemySkillClearBuff : EnemyController
	{
		[Space]
		[Header("Parameters")]
		[SerializeField]
		private float coolDownTimeMillisecond;

		[SerializeField]
		private float delayTimeToClearBuffs;

		[SerializeField]
		private bool activeAtStart;

		private float coolDownTime;

		private float coolDownTimeTracking;

		private bool skillReady;

		private List<string> ignoreBuffKeys = new List<string>
		{
			"Slow",
			"Burning"
		};

		public override void Initialize()
		{
			base.Initialize();
			base.EnemyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (ShouldCastSkill() && IsCooldownSkillDone())
			{
				CustomCancelInvoke(CastSkillClearBuffs);
				CustomInvoke(CastSkillClearBuffs, delayTimeToClearBuffs / 1000f);
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			coolDownTime = coolDownTimeMillisecond / 1000f;
			coolDownTimeTracking = ((!activeAtStart) ? coolDownTime : 0f);
			skillReady = true;
		}

		public override void Update()
		{
			base.Update();
			if (skillReady && IsEnemyAlive() && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				coolDownTimeTracking = Mathf.MoveTowards(coolDownTimeTracking, 0f, Time.deltaTime);
			}
		}

		private bool ShouldCastSkill()
		{
			bool result = false;
			foreach (string ignoreBuffKey in ignoreBuffKeys)
			{
				float buffsValue = base.EnemyModel.BuffsHolder.GetBuffsValue(ignoreBuffKeys);
				if (buffsValue > 0f)
				{
					result = true;
				}
			}
			return result;
		}

		private bool IsCooldownSkillDone()
		{
			return coolDownTimeTracking == 0f;
		}

		private void CastSkillClearBuffs()
		{
			base.EnemyModel.BuffsHolder.RemoveBuffs(ignoreBuffKeys);
			base.EnemyModel.EnemyEffectController.RemoveAllFXs();
			base.EnemyModel.EnemyEffectController.SetNormalColor();
			UnityEngine.Debug.Log("clear all buffs");
			coolDownTimeTracking = coolDownTime;
		}
	}
}
