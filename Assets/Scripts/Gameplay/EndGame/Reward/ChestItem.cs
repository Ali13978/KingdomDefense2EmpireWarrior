using LifetimePopup;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class ChestItem : ButtonController
	{
		[SerializeField]
		private int chestID;

		private Button button;

		private Animator animatorChest;

		private bool isOpened;

		private int currentTurn;

		private ParameterInOneTurn param;

		[SerializeField]
		private Image rewardImage;

		[SerializeField]
		private Text rewardValueText;

		private bool isDisplayRewardValue;

		private int rewardID;

		private int rewardValue;

		[SerializeField]
		private float timeToOpen;

		public bool IsOpened
		{
			get
			{
				return isOpened;
			}
			set
			{
				isOpened = value;
			}
		}

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = GetComponent<Button>();
			animatorChest = GetComponentInChildren<Animator>();
		}

		public void Init()
		{
		}

		public override void OnClick()
		{
			base.OnClick();
			if (SingletonMonoBehaviour<GameData>.Instance.isAvailableOpenChestTurn())
			{
				GetRewardParam();
				GetReward();
				PlayAnimChest();
				LockButton();
				SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.EndGameRewardPopupController.UpdateContinueButtonStatus();
			}
			else
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(17);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		private void GetReward()
		{
			SingletonMonoBehaviour<GameData>.Instance.ChangeOpenChestTurn(-1);
			SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.EndGameRewardPopupController.ChangeChestQuantity();
		}

		private void GetRewardParam()
		{
			currentTurn = 2 - SingletonMonoBehaviour<GameData>.Instance.CurrentOpenChestOffer;
			UnityEngine.Debug.Log("Lượt mở thứ " + currentTurn);
			param = SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.EndGameRewardPopupController.TurnsData[currentTurn];
			UnityEngine.Debug.Log(param);
			List<int> list = new List<int>();
			list.Add(param.heroRate);
			list.Add(param.heroSellOfferRate_common);
			list.Add(param.heroBonusExpRate_1);
			list.Add(param.heroBonusExpRate_2);
			list.Add(param.gemBonusRate_common);
			list.Add(param.gemBonusRate_unCommon);
			list.Add(param.gemBonusRate_rare);
			list.Add(param.powerUpItemBonusRate_Freezing_Common);
			list.Add(param.powerUpItemBonusRate_MeteorStrike_Common);
			list.Add(param.powerUpItemBonusRate_HealingPotion_Common);
			list.Add(param.powerUpItemBonusRate_GoldChest_Common);
			list.Add(param.powerUpItemBonusRate_MeteorStrike_Rare);
			list.Add(param.heroWukongRate);
			list.Add(param.heroAshiRate);
			list.Add(param.heroSellOfferRate_unCommon);
			List<int> listRate = list;
			rewardID = GetRewardByRate(listRate);
			rewardValue = LuckyChestParameter.Instance.GetChestValue(rewardID, currentTurn);
			SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.EndGameRewardPopupController.RewardHandler(chestID, rewardID, rewardValue);
		}

		private int GetRewardByRate(List<int> listRate)
		{
			int result = -1;
			int num = Random.Range(0, 100);
			if (num < listRate[0])
			{
				result = 0;
			}
			for (int i = 1; i < listRate.Count; i++)
			{
				if (num >= listRate[i - 1] && num <= listRate[i])
				{
					result = i;
				}
			}
			return result;
		}

		private void LockButton()
		{
			button.interactable = false;
			isOpened = true;
		}

		private void PlayAnimChest()
		{
			animatorChest.SetTrigger("Anim");
		}

		public void DisplayReward(string rewardName, bool isDisplayRewardValue)
		{
			StartCoroutine(DoDisplay(rewardName, isDisplayRewardValue));
		}

		private IEnumerator DoDisplay(string rewardName, bool isDisplayRewardValue)
		{
			yield return new WaitForSeconds(timeToOpen / Time.timeScale);
			rewardImage.gameObject.SetActive(value: true);
			rewardImage.sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_{rewardName}");
			rewardValueText.gameObject.SetActive(isDisplayRewardValue);
			rewardValueText.text = rewardValue.ToString();
			UISoundManager.Instance.PlayluckyChestSound();
		}
	}
}
