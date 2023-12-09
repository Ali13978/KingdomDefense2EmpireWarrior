using UnityEngine;

namespace Gameplay
{
	public class Pet1008Skill : HeroSkillCommon
	{
		private HeroModel heroModel;

		private HeroModel ownerModel;

		private bool unLock;

		private float duration;

		private float cooldownTime;

		private float hpBuffPercentage;

		private float timeTracking;

		private string buffKey = "Stun";

		private EnemyModel target;

		[SerializeField]
		private float animDuration;

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

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unLock = true;
			duration = heroModel.PetConfigData.Skillvalues[0];
			cooldownTime = heroModel.PetConfigData.Skillvalues[1];
			hpBuffPercentage = heroModel.PetConfigData.Skillvalues[2];
			timeTracking = cooldownTime;
			ownerModel = heroModel.PetOwner;
			ownerModel.BuffsHolder.AddBuff("BuffHpByPercentage", new Buff(isPositive: true, hpBuffPercentage, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
			InitFXs();
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void TryToCastSkill()
		{
			target = heroModel.GetCurrentTarget();
			if (GameTools.IsValidEnemy(target))
			{
				heroModel.SetSpecialStateDuration(animDuration);
				heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
				heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
				target.ProcessEffect(buffKey, 100, duration, DamageFXType.Stun);
			}
			timeTracking = cooldownTime;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_STUN);
		}
	}
}
