using DG.Tweening;
using LifetimePopup;
using Middle;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class ContinueButtonController : ButtonController
	{
		[SerializeField]
		private ChestGroupController chestGroupController;

		[SerializeField]
		private DOTweenAnimation tweenAnimation;

		[SerializeField]
		private Image image;

		public override void OnClick()
		{
			base.OnClick();
			if (chestGroupController.isAvailableChestToOpen() && SingletonMonoBehaviour<GameData>.Instance.isAvailableOpenChestTurn())
			{
				chestGroupController.AutoOpenChest();
				CustomInvoke(DoContinue, 1f);
			}
			else
			{
				DoContinue();
			}
		}

		public void UpdateStatus()
		{
			if (!chestGroupController.isAvailableChestToOpen() && !SingletonMonoBehaviour<GameData>.Instance.isAvailableOpenChestTurn())
			{
				ShowHighlightStatus();
			}
			else
			{
				ShowNormalStatus();
			}
		}

		private void ShowNormalStatus()
		{
			tweenAnimation.DOPause();
			Color color = image.color;
			color.a = 44f / 51f;
			image.color = color;
		}

		private void ShowHighlightStatus()
		{
			tweenAnimation.DOPlay();
			Color color = image.color;
			color.a = 1f;
			image.color = color;
		}

		private void DoContinue()
		{
			Loading.Instance.ShowLoading();
			CustomInvoke(DoLoad, 1f);
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
		}

		private void DoLoad()
		{
			VideoPlayerManager.Instance.TryToShowInterstitialAds_EndGame();
			ModeManager.Instance.gameMode = GameMode.CampaignMode;
			GameApplication.Instance.LoadScene(GameApplication.WorldMapSceneName);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.TryToOpenPopupAskRate(SingletonMonoBehaviour<GameData>.Instance.MapID);
		}
	}
}
