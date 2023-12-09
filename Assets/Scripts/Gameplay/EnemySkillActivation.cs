using SSR.Core.Architecture;
using UnityEngine;

namespace Gameplay
{
	public class EnemySkillActivation : EnemyController
	{
		[SerializeField]
		private EnemySkill enemySkill;

		[Space]
		[SerializeField]
		private OrderedEventDispatcher onSkillActivated = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onSkillDeactivated = new OrderedEventDispatcher();

		private bool activated;

		private float coolDownTimeTracking;

		private float activeTimeTracking;

		private float coolDownDuration;

		private float activeDuration;

		private bool shouldActivateSkill;

		private bool onSkill;

		public bool OnSkill
		{
			get
			{
				return onSkill;
			}
			private set
			{
				onSkill = value;
			}
		}

		public override void Initialize()
		{
			enemySkill.EnemySkillActivation = this;
		}

		public override void OnAppear()
		{
			activated = false;
		}

		public void Activate(bool activateSkillImmediately)
		{
			activated = true;
			coolDownTimeTracking = 0f;
			activeTimeTracking = 0f;
			coolDownDuration = enemySkill.CoolDownDuration;
			activeDuration = enemySkill.ActiveDuration;
			shouldActivateSkill = activateSkillImmediately;
			OnSkill = false;
		}

		public void DisruptSkill()
		{
			StopSkill();
		}

		public new void Update()
		{
			if (!activated)
			{
				return;
			}
			if (shouldActivateSkill)
			{
				ActivateSkill();
			}
			else
			{
				coolDownTimeTracking += Time.deltaTime;
				if (coolDownTimeTracking >= coolDownDuration)
				{
					coolDownTimeTracking = 0f;
					shouldActivateSkill = true;
					ActivateSkill();
				}
			}
			if (OnSkill)
			{
				activeTimeTracking += Time.deltaTime;
				if (activeTimeTracking >= activeDuration)
				{
					StopSkill();
				}
			}
		}

		private void ActivateSkill()
		{
			if (!OnSkill && enemySkill.OnActive())
			{
				OnSkill = true;
				shouldActivateSkill = false;
				activeTimeTracking = 0f;
				onSkillActivated.Dispatch();
			}
		}

		private void StopSkill()
		{
			if (OnSkill)
			{
				OnSkill = false;
				enemySkill.OnInactive();
				onSkillDeactivated.Dispatch();
			}
		}

		public void StopSkillImmediately(int cooldownIncreaseTime)
		{
			StopSkill();
			coolDownTimeTracking = -cooldownIncreaseTime;
			UnityEngine.Debug.Log("After cooldown increase " + coolDownTimeTracking);
		}
	}
}
