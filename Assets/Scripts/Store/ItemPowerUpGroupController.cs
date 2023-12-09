using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Store
{
	public class ItemPowerUpGroupController : MonoBehaviour
	{
		[SerializeField]
		private List<ItemPowerUp> listItemPowerUp = new List<ItemPowerUp>();

		public List<ItemPowerUp> ListItemPowerUp
		{
			get
			{
				return listItemPowerUp;
			}
			set
			{
				listItemPowerUp = value;
			}
		}

		private void Start()
		{
			UpdateItemsQuantity();
			ReadWriteDataPowerUpItem.Instance.OnItemQuantityChangeEvent += Instance_OnItemQuantityChangeEvent;
		}

		private void Instance_OnItemQuantityChangeEvent()
		{
			UpdateItemsQuantity();
		}

		private void UpdateItemsQuantity()
		{
			foreach (ItemPowerUp item in ListItemPowerUp)
			{
				item.UpdateItemsQuantity();
			}
		}

		public void InitItemsInformation()
		{
			foreach (ItemPowerUp item in ListItemPowerUp)
			{
				item.InitInfor();
			}
		}
	}
}
