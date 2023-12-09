using UnityEngine;

namespace Gameplay
{
	public class AllySkillSlash : AllyController
	{
		private bool unLock;

		private float duration;

		private float cooldownTime;

		private string description;

		private float cooldownTimeTracking;

		public void Init(float duration, float cooldownTime, string description, bool isActiveAtStart)
		{
			unLock = true;
			this.duration = duration;
			this.cooldownTime = cooldownTime;
			this.description = description;
			cooldownTimeTracking = ((!isActiveAtStart) ? cooldownTime : 0f);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			unLock = false;
		}

		public override void Update()
		{
			base.Update();
			if (unLock && (bool)base.AllyModel && base.AllyModel.IsAlive)
			{
				cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
			}
		}

		private bool IsCooldownDone()
		{
			return cooldownTimeTracking == 0f;
		}

		public void TryToCastskill()
		{
			if (unLock && IsCooldownDone() && (bool)base.AllyModel.currentTarget && base.AllyModel.IsInMeleeRange(base.AllyModel.currentTarget))
			{
				CastSkill();
			}
		}

		private void CastSkill()
		{
			if (IsEmptySpecialState())
			{
				base.AllyModel.SetSpecialStateDuration(duration);
				base.AllyModel.SetSpecialStateAnimationName(AllyAnimationController.animActiveSkill);
				base.AllyModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, AllyAnimationController.animActiveSkill);
				cooldownTimeTracking = cooldownTime;
			}
		}

		private bool IsEmptySpecialState()
		{
			return !(base.AllyModel.GetFSMController().GetCurrentState() is NewHeroSpecialState);
		}
	}
}
