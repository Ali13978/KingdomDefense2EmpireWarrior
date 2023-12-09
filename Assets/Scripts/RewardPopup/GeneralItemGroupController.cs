using System.Collections.Generic;
using UnityEngine;

namespace RewardPopup
{
	public class GeneralItemGroupController : MonoBehaviour
	{
		[SerializeField]
		private List<GeneralItem> listItems = new List<GeneralItem>();

		public void InitListItems(RewardItem[] listData)
		{
			HideAllItems();
			for (int i = 0; i < listData.Length; i++)
			{
				listItems[i].Init(listData[i]);
			}
		}

		public void HideAllItems()
		{
			foreach (GeneralItem listItem in listItems)
			{
				listItem.Hide();
			}
		}
	}
}
