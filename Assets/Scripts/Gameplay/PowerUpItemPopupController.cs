using Data;
using DG.Tweening;
using Middle;
using Parameter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class PowerUpItemPopupController : SingletonMonoBehaviour<PowerUpItemPopupController>
	{
		[Space]
		[Header("Popup ")]
		[SerializeField]
		private GameObject popup;

		[SerializeField]
		private float timeToOpen;

		[SerializeField]
		private float timeToClose;

		private bool toggle;

		[SerializeField]
		private GameObject imageCancel;

		[Space]
		[Header("List Item")]
		public List<PowerUpItem> listPowerUpItem = new List<PowerUpItem>();

		[Space]
		[Header("List Item Icon")]
		[HideInInspector]
		public int selectingItemID = -1;

		[SerializeField]
		private Sprite originIcon;

		[SerializeField]
		private Image currentItemIcon;

		private bool isOnAnimation;

		private int[] listItemQuantityDailyTrial = new int[4];

		private float gameOldSpeed = 1f;

		private float slowSpeed = 0.3f;

		private void Awake()
		{
			GetListPowerUpItems();
			selectingItemID = -1;
			isOnAnimation = false;
			ReadWriteDataPowerUpItem.Instance.OnItemQuantityChangeEvent += Instance_OnItemQuantityChangeEvent;
		}

		private void Start()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				RefreshQuantityAllItems();
				break;
			case GameMode.DailyTrialMode:
				InitDataDailyTrial();
				RefreshQuantityAllItems();
				break;
			case GameMode.TournamentMode:
				RefreshQuantityAllItems();
				break;
			}
			GameEventCenter.Instance.Subscribe(GameEventType.OnClickButton, new ClickButtonSubscriberData(GameTools.GetUniqueId(), HandleBtnClicked));
		}

		private void OnDestroy()
		{
			ReadWriteDataPowerUpItem.Instance.OnItemQuantityChangeEvent -= Instance_OnItemQuantityChangeEvent;
		}

		public void InitDataDailyTrial()
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			listItemQuantityDailyTrial = DailyTrialParameter.Instance.getListInputItem(currentDayIndex);
			UnityEngine.Debug.Log(listItemQuantityDailyTrial);
		}

		public int GetDailyItemQuantity(int itemID)
		{
			int num = -1;
			return listItemQuantityDailyTrial[itemID];
		}

		public void ChangeItemQuantity(int itemID, int amount)
		{
			listItemQuantityDailyTrial[itemID] += amount;
		}

		private void Instance_OnItemQuantityChangeEvent()
		{
			RefreshQuantityAllItems();
		}

		public void RefreshQuantityAllItems()
		{
			foreach (PowerUpItem item in listPowerUpItem)
			{
				item.RefreshQuantity();
			}
		}

		public void CastItemSkill()
		{
			if (selectingItemID >= 0)
			{
				listPowerUpItem[selectingItemID].CastItemSkill();
				DisplayItemIsNotSelecting();
			}
		}

		public void DisplayItemIsSelecting()
		{
			imageCancel.SetActive(value: true);
			currentItemIcon.sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{selectingItemID}");
			currentItemIcon.SetNativeSize();
		}

		public void DisplayItemIsNotSelecting()
		{
			imageCancel.SetActive(value: false);
			currentItemIcon.sprite = originIcon;
			currentItemIcon.SetNativeSize();
			ResumeOldGameSpeed();
		}

		private void GetListPowerUpItems()
		{
			listPowerUpItem = new List<PowerUpItem>(GetComponentsInChildren<PowerUpItem>(includeInactive: true));
		}

		public void OnClick()
		{
			UISoundManager.Instance.PlayClick();
			GameEventCenter.Instance.Trigger(GameEventType.OnClickButton, new ClickedObjectData(ClickedObjectType.ItemSkillBtn));
		}

		public void HandleBtnClicked(ClickedObjectData clickedObjData)
		{
			if (selectingItemID >= 0)
			{
				selectingItemID = -1;
				DisplayItemIsNotSelecting();
				UnityEngine.Debug.Log("bỏ chọn2");
			}
			else if (clickedObjData.clickedObjType == ClickedObjectType.ItemSkillBtn)
			{
				toggle = !toggle;
				if (toggle)
				{
					OpenPopup();
					return;
				}
				ResumeOldGameSpeed();
				ClosePopup();
			}
			else if (toggle)
			{
				toggle = false;
				ResumeOldGameSpeed();
				ClosePopup();
			}
		}

		private void TurnOnItemInteract()
		{
			foreach (PowerUpItem item in listPowerUpItem)
			{
				item.ReadyToUse();
			}
		}

		private void TurnOffItemInteract()
		{
			foreach (PowerUpItem item in listPowerUpItem)
			{
				item.NotReadyToUse();
			}
		}

		public void OpenPopup()
		{
			if (!isOnAnimation)
			{
				gameOldSpeed = GameplayManager.Instance.gameSpeedController.GameSpeed;
				isOnAnimation = true;
				popup.transform.DOLocalMove(Vector3.zero, timeToOpen).OnComplete(OnOpenComplete);
				popup.transform.DOScale(1f, timeToOpen);
				UISoundManager.Instance.PlayOpenPopup();
				toggle = true;
				TurnOffItemInteract();
			}
		}

		private void OnOpenComplete()
		{
			isOnAnimation = false;
			TurnOnItemInteract();
			RefreshQuantityAllItems();
		}

		public void ClosePopup()
		{
			if (!isOnAnimation)
			{
				CustomInvoke(DoClose, timeToClose);
			}
		}

		private void DoClose()
		{
			TurnOffItemInteract();
			UISoundManager.Instance.PlayClosePopup();
			isOnAnimation = true;
			popup.transform.DOLocalMove(new Vector3(220f, -450f, 0f), timeToOpen).OnComplete(OnCloseComplete);
			popup.transform.DOScale(0f, timeToOpen);
			toggle = false;
		}

		private void OnCloseComplete()
		{
			isOnAnimation = false;
		}

		public void ResumeOldGameSpeed()
		{
		}
	}
}
