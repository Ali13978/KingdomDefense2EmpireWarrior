using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero9Skill3 : HeroSkillCommon
	{
		private int heroID = 9;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private Hero heroParameter;

		private int numberClone;

		private float parameter_scale;

		private float duration;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private Transform clonePosition;

		public override void Update()
		{
			base.Update();
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && unLock && (!heroModel || heroModel.IsAlive))
			{
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			heroParameter = HeroParameter.Instance.GetHeroParameter(heroID, currentLevel);
			HeroSkillParameter_9_3 heroSkillParameter_9_ = new HeroSkillParameter_9_3();
			heroSkillParameter_9_ = (HeroSkillParameter_9_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberClone = heroSkillParameter_9_.getParam(currentSkillLevel - 1).number_clone;
			parameter_scale = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).parameter_Scale / 100f;
			duration = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_9_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime * 0.1f;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnAlly>.Instance.PushAlliesToPool(109, 109, 0);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && unLock)
			{
				StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_2);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_2);
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			AllyModel clone = SingletonMonoBehaviour<SpawnAlly>.Instance.Get(109, 109);
			clone.InitFromHero(heroParameter, parameter_scale, duration);
			clone.gameObject.SetActive(value: true);
			clone.transform.position = clonePosition.position;
			clone.SetAssignedPosition(clonePosition.position);
		}
	}
}
