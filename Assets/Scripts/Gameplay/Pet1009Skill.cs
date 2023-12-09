using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Pet1009Skill : HeroSkillCommon
	{
		private HeroModel heroModel;

		private bool unLock;

		private EnemyModel target;

		private float defenseBuffPercentage;

		private int aoeDamage;

		private float aoeRange;

		private float cooldownTime;

		private float timeTracking;

		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		public override void Update()
		{
			base.Update();
			if (unLock)
			{
				if (IsCooldownDone())
				{
					StartCoroutine(CastSkill());
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unLock = true;
			PetConfigData petConfigData = heroModel.PetConfigData;
			defenseBuffPercentage = petConfigData.Skillvalues[0];
			aoeDamage = petConfigData.Skillvalues[1];
			aoeRange = (float)petConfigData.Skillvalues[2] / GameData.PIXEL_PER_UNIT;
			cooldownTime = petConfigData.Skillvalues[3];
			timeTracking = cooldownTime;
			BuffToOwner();
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitBulletsFromHeroes(heroModel.HeroID, 1);
		}

		private void BuffToOwner()
		{
			HeroModel petOwner = heroModel.PetOwner;
			petOwner.BuffsHolder.AddBuff("BuffDeffenseByPercentage", new Buff(isPositive: true, defenseBuffPercentage, 999999f), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			target = heroModel.GetCurrentTarget();
			timeTracking = cooldownTime;
			if (GameTools.IsValidEnemy(target))
			{
				heroModel.SetSpecialStateDuration(animDuration);
				heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
				heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
				yield return new WaitForSeconds(delayTimeCastSkill);
				BulletModel bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetForHero(heroModel.HeroID, 1);
				bullet.transform.position = gunPos.position;
				bullet.gameObject.SetActive(value: true);
				bullet.InitFromHero(heroModel, new CommonAttackDamage(aoeDamage, 0, aoeRange), target);
			}
		}
	}
}
