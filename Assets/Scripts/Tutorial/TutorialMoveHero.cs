using Data;
using Gameplay;
using Middle;
using UnityEngine;

namespace Tutorial
{
	public class TutorialMoveHero : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_HERO_MOVE;

		public void MoveHeroToPosition()
		{
			RaycastHit2D mapHit = Physics2D.Raycast(getTargetVector(), Vector3.back, 5f);
			RaycastHit2D entityHit = Physics2D.Raycast(getTargetVector(), Vector3.back, 5f);
			ClickInputData inputData = new ClickInputData(ClickInputPhase.Up, mapHit, entityHit);
			InputFilterManager.Instance.OnHandleInput_ClickMapToMoveHero(inputData);
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Hoàn thành tut di chuyển hero!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			bool result = false;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				if (!GameplayTutorialManager.Instance.IsTutorialMap())
				{
					SaveTutorialPassed();
				}
				result = (!ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && GameplayTutorialManager.Instance.IsTutorialMap());
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
