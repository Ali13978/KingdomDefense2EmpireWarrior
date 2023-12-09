using Data;
using Parameter;
using System.Collections;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class Hero2Skill0 : HeroSkillCommon
	{
		private int heroID = 2;

		private int skillID;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private Hero heroParameter;

		private int numberClone;

		private float parameter_scale;

		private float duration;

		private float cooldownTime;

		private string description;

		private string useType;

		private RaycastHit2D hit;

		[SerializeField]
		private LayerMask avaiableMovingLayerMask;

		private void Start()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition += Instance_onCastHeroSkillToAssignedPosition;
		}

		private new void Update()
		{
			if (unLock && HeroesManager.Instance.HeroSkillIDChoosing == heroID)
			{
				GameplayTutorialManager.Instance.TutorialUseHeroSkill.TryMoveToStep(1);
			}
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onCastHeroSkillToAssignedPosition -= Instance_onCastHeroSkillToAssignedPosition;
		}

		private void Instance_onCastHeroSkillToAssignedPosition(int heroID, Vector2 targetPosition)
		{
			if (this.heroID == heroID)
			{
				StartCoroutine(CastSkill(targetPosition));
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			this.heroModel = heroModel;
			unLock = true;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			heroParameter = HeroParameter.Instance.GetHeroParameter(heroID, currentLevel);
			HeroSkillParameter_2_0 heroSkillParameter_2_ = new HeroSkillParameter_2_0();
			heroSkillParameter_2_ = (HeroSkillParameter_2_0)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			numberClone = heroSkillParameter_2_.getParam(currentSkillLevel - 1).number_clone;
			parameter_scale = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).parameter_Scale / 100f;
			duration = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_2_.getParam(currentSkillLevel - 1).description;
			useType = heroSkillParameter_2_.getParam(currentSkillLevel - 1).use_type;
			SingletonMonoBehaviour<SpawnAlly>.Instance.PushAlliesToPool(102, 102, 0);
		}

		public override float GetCooldownTime()
		{
			return cooldownTime;
		}

		public override string GetUseType()
		{
			return useType;
		}

		private IEnumerator CastSkill(Vector2 targetPosition)
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			CreateClones(targetPosition);
			heroModel.SetSpecialStateDuration(0.5f);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
		}

		private void CreateClones(Vector2 targetPosition)
		{
			UnityEngine.Debug.Log("Hero 2 Cast Skill 0");
			SingletonMonoBehaviour<GameData>.Instance.PlayerKnowHowToUseSkill = true;
			GameplayTutorialManager.Instance.TutorialUseHeroSkill.SetTutorialPassed();
			int num = 0;
			for (int i = 0; i < numberClone; i++)
			{
				AllyModel allyModel = SingletonMonoBehaviour<SpawnAlly>.Instance.Get(102, 102);
				allyModel.transform.position = targetPosition;
				allyModel.InitFromHero(heroParameter, parameter_scale, duration);
				allyModel.gameObject.SetActive(value: true);
				num++;
			}
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.listSelectHeroSkillButton[2].DoCooldown();
		}
	}
}
