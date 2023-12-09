using Data;
using Gameplay;
using Middle;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUseHeroSkill : TutorialUnit
	{
		private static string TUTORIAL_USE_HERO_SKILL = "TutorialUseHeroSkill";

		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_HERO_SKILL;

		private TutorialUseHeroSkillInMap tut;

		public void ShowTutorialUseHeroSkill()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag(TUTORIAL_USE_HERO_SKILL);
			if ((bool)gameObject)
			{
				tut = gameObject.GetComponent<TutorialUseHeroSkillInMap>();
			}
			if (tut != null)
			{
				tut.Show();
			}
		}

		public void HideTutorialUseHeroSkill()
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag(TUTORIAL_USE_HERO_SKILL);
			if ((bool)gameObject)
			{
				tut = gameObject.GetComponent<TutorialUseHeroSkillInMap>();
			}
			if (tut != null)
			{
				tut.Hide();
			}
		}

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Hoàn thành tut sử dụng skill hero!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			bool result = false;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				if (SingletonMonoBehaviour<GameData>.Instance.PlayerKnowHowToUseSkill || !GameplayTutorialManager.Instance.IsTutorialMap())
				{
					SaveTutorialPassed();
				}
				result = (!ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && !SingletonMonoBehaviour<GameData>.Instance.PlayerKnowHowToUseSkill && SingletonMonoBehaviour<GameData>.Instance.MapID == 0);
				break;
			case GameMode.DailyTrialMode:
				result = false;
				break;
			case GameMode.TournamentMode:
				result = false;
				break;
			}
			return result;
		}
	}
}
