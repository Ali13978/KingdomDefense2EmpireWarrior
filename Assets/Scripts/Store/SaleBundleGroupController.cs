using Data;
using System.Collections.Generic;
using UnityEngine;

namespace Store
{
	public class SaleBundleGroupController : MonoBehaviour
	{
		[SerializeField]
		private List<SaleBundleItem> listSpecialItem = new List<SaleBundleItem>();

		[SerializeField]
		private List<SaleBundleItem> listSaleBundleItem = new List<SaleBundleItem>();

		public List<SaleBundleItem> boosters = new List<SaleBundleItem>();

		public List<SaleBundleItem> ListSpecialItem
		{
			get
			{
				return listSpecialItem;
			}
			set
			{
				listSpecialItem = value;
			}
		}

		public List<SaleBundleItem> ListSaleBundleItem
		{
			get
			{
				return listSaleBundleItem;
			}
			set
			{
				listSaleBundleItem = value;
			}
		}

		public void InitItemsInformation()
		{
			foreach (SaleBundleItem item in listSaleBundleItem)
			{
				item.Init();
			}
			InitSpecialItem();
			for (int i = 0; i < boosters.Count; i++)
			{
				boosters[i].Init();
			}
		}

		private void InitSpecialItem()
		{
			int currentAvailableSpeciakPackIndex = ReadWriteDataSaleBundle.Instance.GetCurrentAvailableSpeciakPackIndex();
			for (int i = 0; i < listSpecialItem.Count; i++)
			{
				if (i == currentAvailableSpeciakPackIndex)
				{
					listSpecialItem[i].Init();
				}
				else
				{
					listSpecialItem[i].Hide();
				}
			}
		}

		public void RefreshItemStatus()
		{
			foreach (SaleBundleItem item in listSaleBundleItem)
			{
				item.RefreshStatus();
			}
			InitSpecialItem();
			foreach (SaleBundleItem item2 in listSpecialItem)
			{
				item2.RefreshStatus();
			}
		}
	}
}
