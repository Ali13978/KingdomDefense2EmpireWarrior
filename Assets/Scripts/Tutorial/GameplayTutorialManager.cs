using Data;
using Gameplay;
using Middle;
using MyCustom;
using UnityEngine;

namespace Tutorial
{
	public class GameplayTutorialManager : CustomMonoBehaviour
	{
		private string tutorialID = ReadWriteDataTutorial.TUTORIAL_ID_BUILD_TOWER;

		[Space]
		[SerializeField]
		private TutorialUseHeroSkill tutorialUseHeroSkill;

		[Space]
		[SerializeField]
		private TutorialMoveHero tutorialMoveHero;

		[Space]
		[SerializeField]
		private float delayTimeToStartTutorialControlHero;

		public TutorialUseHeroSkill TutorialUseHeroSkill
		{
			get
			{
				return tutorialUseHeroSkill;
			}
			set
			{
				tutorialUseHeroSkill = value;
			}
		}

		public TutorialMoveHero TutorialMoveHero
		{
			get
			{
				return tutorialMoveHero;
			}
			set
			{
				tutorialMoveHero = value;
			}
		}

		public static GameplayTutorialManager Instance
		{
			get;
			set;
		}

		private void Awake()
		{
			if ((bool)Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				Instance = this;
			}
		}

		private void Start()
		{
			SingletonMonoBehaviour<SpawnEnemy>.Instance.onDispatchEventTutorialHeroSkill += Instance_onDispatchEventTutorialHeroSkill;
		}

		private void Instance_onDispatchEventTutorialHeroSkill()
		{
			CheckConditionAfterTime();
		}

		private void CheckConditionAfterTime()
		{
			CustomInvoke(DoCheck, delayTimeToStartTutorialControlHero / GameplayManager.Instance.gameSpeedController.GameSpeed);
		}

		private void DoCheck()
		{
			TutorialUseHeroSkill.CheckCondition();
		}

		public bool IsTutorialDone()
		{
			return ReadWriteDataTutorial.Instance.GetTutorialStatus(tutorialID);
		}

		public bool IsTutorialMap()
		{
			return ModeManager.Instance.gameMode == GameMode.CampaignMode && SingletonMonoBehaviour<GameData>.Instance.MapID == 0;
		}
	}
}
