using Data;
using DG.Tweening;
using Middle;
using MyCustom;
using Parameter;
using Services.PlatformSpecific;
using SSR.Core.Architecture;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class PowerUpItem : CustomMonoBehaviour
	{
		public OrderedEventDispatcher OnCastItemSkillEvent;

		[SerializeField]
		private PowerUpItemUseType useType;

		[SerializeField]
		private Text quantity;

		[SerializeField]
		private Image imageCooldown;

		public int powerUpItemID;

		private float cooldownTime;

		private string buffkey = "Slow";

		private Button button;

		[SerializeField]
		private Image imageIcon;

		[SerializeField]
		private GameObject buttonBuyMore;

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		public void Init(float _cooldownTime)
		{
			cooldownTime = _cooldownTime;
		}

		public void RefreshQuantity()
		{
			int num = 0;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				num = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(powerUpItemID);
				RefreshStatusByQuantity(num);
				break;
			case GameMode.DailyTrialMode:
				num = SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.GetDailyItemQuantity(powerUpItemID);
				RefreshStatusByLimitGiven(num);
				break;
			case GameMode.TournamentMode:
			{
				num = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(powerUpItemID);
				bool isReachedLimit = false;
				if ((bool)SingletonMonoBehaviour<GameData>.Instance)
				{
					isReachedLimit = SingletonMonoBehaviour<GameData>.Instance.PowerupItemData.IsReachedLimitUse();
				}
				RefreshStatusByLimitUse(num, isReachedLimit);
				break;
			}
			}
		}

		private void RefreshStatusByQuantity(int itemAmount)
		{
			if (itemAmount >= 1)
			{
				ViewEnable();
				buttonBuyMore.SetActive(value: false);
			}
			else
			{
				ViewDisable();
				buttonBuyMore.SetActive(value: true);
			}
			quantity.text = itemAmount.ToString();
		}

		private void RefreshStatusByLimitGiven(int itemAmount)
		{
			if (itemAmount >= 1)
			{
				ViewEnable();
			}
			else
			{
				ViewDisable();
			}
			buttonBuyMore.SetActive(value: false);
			quantity.text = itemAmount.ToString();
		}

		private void RefreshStatusByLimitUse(int itemAmount, bool isReachedLimit)
		{
			if (isReachedLimit)
			{
				ViewDisable();
				buttonBuyMore.SetActive(value: false);
			}
			else if (itemAmount >= 1)
			{
				ViewEnable();
				buttonBuyMore.SetActive(value: false);
			}
			else
			{
				ViewDisable();
				buttonBuyMore.SetActive(value: true);
			}
			quantity.text = itemAmount.ToString();
		}

		public void OnClick()
		{
			switch (useType)
			{
			case PowerUpItemUseType.TapToUse:
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.selectingItemID = powerUpItemID;
				CastItemSkill();
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.ClosePopup();
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.ResumeOldGameSpeed();
				break;
			case PowerUpItemUseType.TapNClickOutSideToUse:
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.selectingItemID = powerUpItemID;
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.ClosePopup();
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.DisplayItemIsSelecting();
				break;
			}
		}

		public void CastItemSkill()
		{
			OnCastItemSkillEvent.Dispatch();
			DoCooldown();
			CustomInvoke(ResetValueAfterUse, Time.deltaTime);
			ChangeItemQuantity();
			SendEventUsePowerupItem();
			if (ModeManager.Instance.gameMode == GameMode.TournamentMode)
			{
				SingletonMonoBehaviour<GameData>.Instance.PowerupItemData.IncreaseUseAmount();
			}
		}

		private void ChangeItemQuantity()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(powerUpItemID, -1);
				break;
			case GameMode.DailyTrialMode:
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.ChangeItemQuantity(powerUpItemID, -1);
				SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.RefreshQuantityAllItems();
				break;
			case GameMode.TournamentMode:
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(powerUpItemID, -1);
				break;
			}
		}

		private void SendEventUsePowerupItem()
		{
			string name = Singleton<PowerupItemDescription>.Instance.GetName(powerUpItemID);
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_UsePowerupItem(name);
		}

		private void ResetValueAfterUse()
		{
			SingletonMonoBehaviour<PowerUpItemPopupController>.Instance.selectingItemID = -1;
		}

		public void DoCooldown()
		{
			DOTween.To(() => 0f, delegate(float x)
			{
				imageCooldown.fillAmount = x;
			}, 1f, cooldownTime).SetEase(Ease.Linear).OnComplete(CooldownComplete);
			imageCooldown.gameObject.SetActive(value: true);
		}

		private void CooldownComplete()
		{
			imageCooldown.gameObject.SetActive(value: false);
		}

		public void ViewEnable()
		{
			ReadyToUse();
			imageIcon.color = Color.white;
		}

		public void ViewDisable()
		{
			NotReadyToUse();
			imageIcon.color = Color.gray;
		}

		public void ReadyToUse()
		{
			button.enabled = true;
		}

		public void NotReadyToUse()
		{
			button.enabled = false;
		}
	}
}
