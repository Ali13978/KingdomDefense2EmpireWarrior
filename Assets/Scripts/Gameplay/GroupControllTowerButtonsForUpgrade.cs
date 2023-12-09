using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class GroupControllTowerButtonsForUpgrade : PopupSingleton
	{
		[SerializeField]
		private List<ControllTowerButtonController> listControllButton = new List<ControllTowerButtonController>();

		private void Awake()
		{
			SingletonMonoBehaviour<GameData>.Instance.OnMoneyChange += Instance_OnMoneyChange;
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
