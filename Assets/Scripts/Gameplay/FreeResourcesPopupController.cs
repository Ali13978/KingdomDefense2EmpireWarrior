using DG.Tweening;
using LifetimePopup;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class FreeResourcesPopupController : GameplayPopupController
	{
		[SerializeField]
		private GameObject groupButton;

		[Space]
		[SerializeField]
		private GameplayVideoButtonController videoMoney;

		[SerializeField]
		private GameplayVideoButtonController videoLife;

		private bool videoMoneyChanged;

		private bool videoLifeChanged;

		[Space]
		[SerializeField]
		private ReadDataAdsReward readDataAdsReward;

		public GameplayVideoButtonController VideoMoney
		{
			get
			{
				return videoMoney;
			}
			private set
			{
				videoMoney = value;
			}
		}

		public GameplayVideoButtonController VideoLife
		{
			get
			{
				return videoLife;
			}
			private set
			{
				videoLife = value;
			}
		}

		public ReadDataAdsReward ReadDataAdsReward
		{
			get
			{
				return readDataAdsReward;
			}
			set
			{
				readDataAdsReward = value;
			}
		}

		public void Init()
		{
			VideoMoney.RefreshStatus();
			VideoLife.RefreshStatus();
			Open();
		}

		public void PlayVideo_Money()
		{
			if (VideoPlayerManager.Instance.CheckIfVideoExits())
			{
				VideoPlayerManager.Instance.playVideoGameplay_ForMoney();
				return;
			}
			string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(19);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
		}

		public void PlayVideo_Life()
		{
			if (VideoPlayerManager.Instance.CheckIfVideoExits())
			{
				VideoPlayerManager.Instance.playVideoGameplay_ForLife();
				return;
			}
			string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(19);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(value: true);
			groupButton.gameObject.SetActive(value: true);
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(groupButton.transform.DOLocalMoveY(-10f, timeToOpen));
			sequence.OnComplete(LateAnimationOpen);
			GameplayManager.Instance.gameSpeedController.PauseGame();
		}

		private void LateAnimationOpen()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(groupButton.transform.DOLocalMoveY(0f, 0.1f));
		}

		public override void Close()
		{
			base.Close();
			base.transform.DOKill();
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(groupButton.transform.DOLocalMoveY(800f, timeToClose)).OnComplete(LateAnimationClose);
		}

		private void LateAnimationClose()
		{
			base.gameObject.SetActive(value: false);
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
		}

		public override void Update()
		{
			base.Update();
			if (!videoLifeChanged & SingletonMonoBehaviour<GameData>.Instance.PlayedGameplayVideo_ForLife)
			{
				VideoLife.IsPlayed = true;
				VideoLife.RefreshStatus();
				videoLifeChanged = true;
			}
			if (!videoMoneyChanged & SingletonMonoBehaviour<GameData>.Instance.PlayedGameplayVideo_ForMoney)
			{
				VideoMoney.IsPlayed = true;
				VideoMoney.RefreshStatus();
				videoMoneyChanged = true;
			}
		}
	}
}
