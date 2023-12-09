using Data;
using LifetimePopup;
using MyCustom;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class PowerUpItem : CustomMonoBehaviour
	{
		[SerializeField]
		private int powerUpItemID;

		[SerializeField]
		private Text quantityText;

		private int quantity;

		public void RefreshQuantity()
		{
			quantity = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(powerUpItemID);
			quantityText.text = quantity.ToString();
		}

		public void OpenPowerupItemTab()
		{
			CustomInvoke(DoOpen, Time.deltaTime);
		}

		private void DoOpen()
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(1);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.TabsGroupController.HighLightButton(1);
		}
	}
}
