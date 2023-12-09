using Data;
using System.Collections.Generic;
using UnityEngine;

namespace MapLevel
{
	public class PowerUpItemGroupController : MonoBehaviour
	{
		[SerializeField]
		private List<PowerUpItem> listPowerUpItem = new List<PowerUpItem>();

		private void Awake()
		{
			ReadWriteDataPowerUpItem.Instance.OnItemQuantityChangeEvent += Instance_OnItemQuantityChangeEvent;
		}

		private void OnDestroy()
		{
			ReadWriteDataPowerUpItem.Instance.OnItemQuantityChangeEvent -= Instance_OnItemQuantityChangeEvent;
		}

		private void Instance_OnItemQuantityChangeEvent()
		{
			RefreshPowerupItems();
		}

		public void RefreshPowerupItems()
		{
			foreach (PowerUpItem item in listPowerUpItem)
			{
				item.RefreshQuantity();
			}
		}
	}
}
