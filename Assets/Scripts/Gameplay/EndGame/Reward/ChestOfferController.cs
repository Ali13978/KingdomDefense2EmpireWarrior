using Data;
using DG.Tweening;
using LifetimePopup;
using MyCustom;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class ChestOfferController : CustomMonoBehaviour
	{
		[SerializeField]
		private GameObject groupWatchVideo;

		[SerializeField]
		private GameObject titleOR;

		[Space]
		[SerializeField]
		private Image imageButtonBuyGem;

		[SerializeField]
		private DOTweenAnimation tweenAnimationBuyGemButton;

		[Space]
		[SerializeField]
		private Text turnAmountToBuyEachText;

		[SerializeField]
		private Text gemAmountToBuyText;

		private int turnAmountToBuyEach;

		private int gemAmountToBuy;

		public static bool changedVideoStatus;

		public void Init()
		{
			Open();
			turnAmountToBuyEach = SingletonMonoBehaviour<GameplayDataReader>.Instance.ReadDataLuckyChest.GetTurnAmountToBuyEach();
			int currentPlayCount = ReadWriteDataMap.Instance.GetCurrentPlayCount(SingletonMonoBehaviour<GameData>.Instance.MapID);
			UnityEngine.Debug.Log("current map play count = " + currentPlayCount);
			if (SingletonMonoBehaviour<GameData>.Instance.MapID == 0)
			{
				gemAmountToBuy = 0;
				gemAmountToBuyText.text = Singleton<NotificationDescription>.Instance.GetNotiContent(22);
			}
			else
			{
				gemAmountToBuy = SingletonMonoBehaviour<GameplayDataReader>.Instance.ReadDataLuckyChest.GetGemAmountToBuyTurn();
				gemAmountToBuyText.text = gemAmountToBuy.ToString();
			}
			turnAmountToBuyEachText.text = turnAmountToBuyEach.ToString();
			int num = 2 - SingletonMonoBehaviour<GameData>.Instance.CurrentOpenChestOffer;
			if (num == 1)
			{
				groupWatchVideo.SetActive(value: false);
				titleOR.SetActive(value: false);
			}
		}

		public void GetMoreTurnByVideoOffer()
		{
			if (VideoPlayerManager.Instance.CheckIfVideoExits())
			{
				VideoPlayerManager.Instance.playVideoGameplay_ForOpenChestOffer();
				SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.EndGameRewardPopupController.UpdateContinueButtonStatus();
				SendEvent_FreeChestOffer(FreeChestOfferType.WatchVideo);
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(18);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
			else
			{
				string notiContent2 = Singleton<NotificationDescription>.Instance.GetNotiContent(19);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent2, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		public void GetMoreTurnByPayGem()
		{
			int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			if (currentGem >= gemAmountToBuy)
			{
				SingletonMonoBehaviour<GameData>.Instance.ChangeOpenChestTurn(turnAmountToBuyEach);
				SingletonMonoBehaviour<GameData>.Instance.ChangeOpenChestOffer();
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(-gemAmountToBuy, isDispatchEventChange: true);
				Close();
				SendEvent_FreeChestOffer(FreeChestOfferType.PayGem);
				SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.EndGameRewardPopupController.UpdateContinueButtonStatus();
			}
			else
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(20);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: true, isShowButtonGoToStore: true);
			}
		}

		private void SendEvent_FreeChestOffer(FreeChestOfferType type)
		{
			int mapID = SingletonMonoBehaviour<GameData>.Instance.MapID + 1;
			int offerCount = 2 - SingletonMonoBehaviour<GameData>.Instance.CurrentOpenChestOffer;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_FreeChestOffer(mapID, type, offerCount);
		}

		public void Open()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Close()
		{
			base.gameObject.SetActive(value: false);
		}

		private void Update()
		{
			if (!changedVideoStatus && SingletonMonoBehaviour<GameData>.Instance.PlayedVideoLucky)
			{
				Close();
				changedVideoStatus = true;
				return;
			}
			int num = 2 - SingletonMonoBehaviour<GameData>.Instance.CurrentOpenChestOffer;
			if (num == 0)
			{
				Color color = imageButtonBuyGem.color;
				color.a = 25f / 51f;
				imageButtonBuyGem.color = color;
				tweenAnimationBuyGemButton.DOPause();
			}
			if (num == 1)
			{
				Color color2 = imageButtonBuyGem.color;
				color2.a = 1f;
				imageButtonBuyGem.color = color2;
				tweenAnimationBuyGemButton.DOPlay();
			}
		}
	}
}
