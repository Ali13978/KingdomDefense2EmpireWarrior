using Data;
using Middle;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class GroupControllTowerButtonsForBuy : PopupSingleton
	{
		[SerializeField]
		private List<ControllTowerButtonController> listControllButton = new List<ControllTowerButtonController>();

		[SerializeField]
		private Sprite[] buttonImage;

		[SerializeField]
		private Sprite lockImage;

		private void Awake()
		{
			SingletonMonoBehaviour<GameData>.Instance.OnMoneyChange += Instance_OnMoneyChange;
		}

		public void InitButtonsStatusByWave()
		{
			for (int i = 0; i < listControllButton.Count; i++)
			{
				bool isAllowedToUse = false;
				switch (ModeManager.Instance.gameMode)
				{
				case GameMode.CampaignMode:
					isAllowedToUse = MapRuleParameter.Instance.IsTowerAllowed_Campaign(SingletonMonoBehaviour<GameData>.Instance.MapID, i);
					break;
				case GameMode.DailyTrialMode:
				{
					int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
					int currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
					isAllowedToUse = MapRuleParameter.Instance.IsTowerAllowed_Daily(currentWave, i);
					break;
				}
				case GameMode.TournamentMode:
				{
					string currentSeasonID = MapRuleParameter.Instance.GetCurrentSeasonID();
					isAllowedToUse = MapRuleParameter.Instance.IsTowerAllowed_Tournament(currentSeasonID, i);
					break;
				}
				}
				listControllButton[i].Init(isAllowedToUse, buttonImage[i], lockImage);
			}
			RefreshStatusAll();
		}

		private void Instance_OnMoneyChange()
		{
			CustomInvoke(RefreshStatusAll, 0.2f);
		}

		private void OnDestroy()
		{
			SingletonMonoBehaviour<GameData>.Instance.OnMoneyChange -= Instance_OnMoneyChange;
		}

		public void DisableConfirmOtherButtons(ControllTowerButtonController NoDisableButton)
		{
			foreach (ControllTowerButtonController item in listControllButton)
			{
				if (!item.Equals(NoDisableButton))
				{
					item.DisableConfirm();
				}
			}
		}

		public void DisableConfirmAllButton()
		{
			foreach (ControllTowerButtonController item in listControllButton)
			{
				item.DisableConfirm();
			}
		}

		protected override void OnClickOutsideUp()
		{
			base.OnClickOutsideUp();
			DisableConfirmAllButton();
		}

		public void RefreshStatusAll()
		{
			foreach (ControllTowerButtonController item in listControllButton)
			{
				item.UpdateBuyState();
			}
		}
	}
}
