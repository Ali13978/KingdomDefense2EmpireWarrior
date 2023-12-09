using Data;
using DG.Tweening;
using Gameplay;
using LifetimePopup;
using Middle;
using Parameter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class DailyTrialResultPopupController : GameplayPopupController
	{
		[Space]
		[SerializeField]
		private Text wavePassedAmount;

		[Space]
		[Header("Controller")]
		[SerializeField]
		private RewardGroupController rewardGroupController;

		[SerializeField]
		private GameObject content;

		public void Init(BattleStatus battleStatus)
		{
			OpenWithScaleAnimation();
			rewardGroupController.Init(battleStatus);
			UpdateWavePassedAmount(battleStatus);
		}

		private void UpdateWavePassedAmount(BattleStatus battleStatus)
		{
			switch (battleStatus)
			{
			case BattleStatus.Victory:
			{
				int waveV = 0;
				tween = DOTween.To(() => 0, delegate(int x)
				{
					waveV = x;
					wavePassedAmount.text = waveV.ToString();
				}, SingletonMonoBehaviour<GameData>.Instance.CurrentWave, 2f).SetEase(Ease.Linear);
				break;
			}
			case BattleStatus.Defeat:
			{
				int waveD = 0;
				tween = DOTween.To(() => 0, delegate(int x)
				{
					waveD = x;
					wavePassedAmount.text = waveD.ToString();
				}, SingletonMonoBehaviour<GameData>.Instance.CurrentWave - 1, 2f).SetEase(Ease.Linear);
				break;
			}
			}
		}

		public void TryToContinue()
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			List<int> listInputHeroesID = DailyTrialParameter.Instance.getListInputHeroesID(currentDayIndex);
			if (currentDayIndex == 0)
			{
				Continue();
				return;
			}
			if (ReadWriteDataHero.Instance.IsHeroOwned(listInputHeroesID))
			{
				Continue();
				return;
			}
			if (currentDayIndex == 6)
			{
				content.SetActive(value: false);
				return;
			}
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.InitSingleHeroOffer(listInputHeroesID[0], OfferType.OneTime);
			content.SetActive(value: false);
		}

		public void Continue()
		{
			Loading.Instance.ShowLoading();
			Invoke("DoLoad", 1f);
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
			ModeManager.Instance.gameMode = GameMode.CampaignMode;
		}

		private void DoLoad()
		{
			GameApplication.Instance.LoadScene(GameApplication.WorldMapSceneName);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
