using Data;
using LifetimePopup;
using Parameter;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class EndGameRewardPopupController : GameplayPopupController
	{
		[Space]
		[Header("Controllers")]
		[SerializeField]
		private UnlockCountGroupController unlockCountGroupController;

		[SerializeField]
		private ChestGroupController chestGroupController;

		[SerializeField]
		private ChestOfferController chestOfferController;

		[SerializeField]
		private PreviewRewardGroupController previewRewardGroupController;

		[Space]
		[SerializeField]
		private GameObject continueGroup;

		[SerializeField]
		private ContinueButtonController continueButton;

		[SerializeField]
		private Button autoOpenChestButton;

		private ParameterInOneTurn[] turnsData;

		private int currentChestOpened;

		[Space]
		[Header("Tutorial")]
		[SerializeField]
		private TutorialOpenLuckyChest tutorialOpenLuckyChest;

		[SerializeField]
		private TutorialGetFreeLuckyChestByVideo tutorialGetFreeLuckyChest;

		public ParameterInOneTurn[] TurnsData
		{
			get
			{
				return turnsData;
			}
			set
			{
				turnsData = value;
			}
		}

		private void Awake()
		{
			SingletonMonoBehaviour<GameData>.Instance.OnOpenChestTurnChange += Instance_OnOpenChestTurnChange;
		}

		private void Start()
		{
			if (tutorialOpenLuckyChest.IsTutorialDone() || !GameplayTutorialManager.Instance.IsTutorialMap())
			{
				continueGroup.SetActive(value: true);
			}
			else
			{
				continueGroup.SetActive(value: false);
			}
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			GetDataRateByTurns();
			previewRewardGroupController.InitListPreviewItems();
			currentChestOpened = 0;
			tutorialOpenLuckyChest.CheckCondition();
			tutorialGetFreeLuckyChest.CheckCondition();
		}

		private void GetDataRateByTurns()
		{
			turnsData = new ParameterInOneTurn[3];
			for (int i = 0; i < TurnsData.Length; i++)
			{
				ParameterInOneTurn parameterInOneTurn = new ParameterInOneTurn();
				parameterInOneTurn.turnID = i;
				parameterInOneTurn.heroRate = LuckyChestParameter.Instance.GetChestRate(0, i);
				parameterInOneTurn.heroSellOfferRate_common = LuckyChestParameter.Instance.GetChestRate(1, i);
				parameterInOneTurn.heroBonusExpRate_1 = LuckyChestParameter.Instance.GetChestRate(2, i);
				parameterInOneTurn.heroBonusExpRate_2 = LuckyChestParameter.Instance.GetChestRate(3, i);
				parameterInOneTurn.gemBonusRate_common = LuckyChestParameter.Instance.GetChestRate(4, i);
				parameterInOneTurn.gemBonusRate_unCommon = LuckyChestParameter.Instance.GetChestRate(5, i);
				parameterInOneTurn.gemBonusRate_rare = LuckyChestParameter.Instance.GetChestRate(6, i);
				parameterInOneTurn.powerUpItemBonusRate_Freezing_Common = LuckyChestParameter.Instance.GetChestRate(7, i);
				parameterInOneTurn.powerUpItemBonusRate_MeteorStrike_Common = LuckyChestParameter.Instance.GetChestRate(8, i);
				parameterInOneTurn.powerUpItemBonusRate_HealingPotion_Common = LuckyChestParameter.Instance.GetChestRate(9, i);
				parameterInOneTurn.powerUpItemBonusRate_GoldChest_Common = LuckyChestParameter.Instance.GetChestRate(10, i);
				parameterInOneTurn.powerUpItemBonusRate_MeteorStrike_Rare = LuckyChestParameter.Instance.GetChestRate(11, i);
				parameterInOneTurn.heroWukongRate = LuckyChestParameter.Instance.GetChestRate(12, i);
				parameterInOneTurn.heroAshiRate = LuckyChestParameter.Instance.GetChestRate(13, i);
				parameterInOneTurn.heroSellOfferRate_unCommon = LuckyChestParameter.Instance.GetChestRate(14, i);
				TurnsData[i] = parameterInOneTurn;
			}
		}

		private void Instance_OnOpenChestTurnChange()
		{
			if (!SingletonMonoBehaviour<GameData>.Instance.isAvailableOpenChestTurn() && SingletonMonoBehaviour<GameData>.Instance.isAvailableOpenChestOffer())
			{
				chestOfferController.Init();
				continueButton.gameObject.SetActive(value: true);
			}
			if (SingletonMonoBehaviour<GameData>.Instance.isAvailableOpenChestTurn() && chestGroupController.isAvailableChestToOpen())
			{
				autoOpenChestButton.interactable = true;
			}
			else
			{
				autoOpenChestButton.interactable = false;
			}
		}

		public void EnableContinueGroup()
		{
			continueGroup.SetActive(value: true);
		}

		public void UpdateContinueButtonStatus()
		{
			continueButton.UpdateStatus();
		}

		public void RewardHandler(int chestID, int rewardID, int rewardValue)
		{
			switch (rewardID)
			{
			case 0:
			{
				List<int> listHeroIDNotOwned = ReadWriteDataHero.Instance.GetListHeroIDNotOwned();
				listHeroIDNotOwned.Remove(0);
				listHeroIDNotOwned.Remove(1);
				if (listHeroIDNotOwned.Count > 0)
				{
					int num2 = listHeroIDNotOwned[Random.Range(0, listHeroIDNotOwned.Count)];
					if (!ReadWriteDataHero.Instance.IsHeroOwned(num2))
					{
						ReadWriteDataHero.Instance.UnlockHero(num2);
					}
					chestGroupController.DisplayReward(chestID, "hero_" + num2, isDisplayRewardValue: false);
				}
				else
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: false);
					chestGroupController.DisplayReward(chestID, "gem", isDisplayRewardValue: true);
				}
				break;
			}
			case 1:
				if (!ReadWriteDataHero.Instance.IsHeroOwned(0))
				{
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.InitSingleHeroOffer(0, OfferType.OneTime);
					chestGroupController.DisplayReward(chestID, "hero_offer", isDisplayRewardValue: false);
					UnityEngine.Debug.Log("hien thi goi offer hero" + 0);
				}
				else
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: false);
					chestGroupController.DisplayReward(chestID, "gem", isDisplayRewardValue: true);
				}
				break;
			case 2:
				ReadWriteDataHero.Instance.LevelUp(1);
				chestGroupController.DisplayReward(chestID, "hero_1_max", isDisplayRewardValue: false);
				break;
			case 3:
				ReadWriteDataHero.Instance.LevelUp(2);
				chestGroupController.DisplayReward(chestID, "hero_2_max", isDisplayRewardValue: false);
				break;
			case 4:
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: false);
				chestGroupController.DisplayReward(chestID, "gem", isDisplayRewardValue: true);
				break;
			case 5:
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: false);
				chestGroupController.DisplayReward(chestID, "gem", isDisplayRewardValue: true);
				break;
			case 6:
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: false);
				chestGroupController.DisplayReward(chestID, "gem", isDisplayRewardValue: true);
				break;
			case 7:
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(0, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_0", isDisplayRewardValue: true);
				break;
			case 8:
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(1, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_1", isDisplayRewardValue: true);
				break;
			case 9:
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(2, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_2", isDisplayRewardValue: true);
				break;
			case 10:
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(3, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_3", isDisplayRewardValue: true);
				break;
			case 11:
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(1, rewardValue);
				chestGroupController.DisplayReward(chestID, "pw_1", isDisplayRewardValue: true);
				break;
			case 12:
				if (!ReadWriteDataHero.Instance.IsHeroOwned(0))
				{
					ReadWriteDataHero.Instance.UnlockHero(0);
					chestGroupController.DisplayReward(chestID, "hero_0", isDisplayRewardValue: false);
				}
				else
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: false);
					chestGroupController.DisplayReward(chestID, "gem", isDisplayRewardValue: true);
				}
				break;
			case 13:
				if (!ReadWriteDataHero.Instance.IsHeroOwned(1))
				{
					ReadWriteDataHero.Instance.UnlockHero(1);
					chestGroupController.DisplayReward(chestID, "hero_1", isDisplayRewardValue: false);
				}
				else
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: false);
					chestGroupController.DisplayReward(chestID, "gem", isDisplayRewardValue: true);
				}
				break;
			case 14:
			{
				List<int> list = new List<int>();
				list.Add(3);
				list.Add(4);
				list.Add(5);
				list.Add(6);
				list.Add(7);
				list.Add(8);
				list.Add(9);
				if (list.Count > 0)
				{
					int num = list[Random.Range(0, list.Count)];
					if (!ReadWriteDataHero.Instance.IsHeroOwned(num))
					{
						SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.InitSingleHeroOffer(num, OfferType.OneTime);
						UnityEngine.Debug.Log("hien thi goi offer hero" + num);
					}
					chestGroupController.DisplayReward(chestID, "hero_offer", isDisplayRewardValue: false);
				}
				else
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: false);
					chestGroupController.DisplayReward(chestID, "gem", isDisplayRewardValue: true);
				}
				break;
			}
			}
		}

		public void ChangeChestQuantity()
		{
			currentChestOpened++;
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
