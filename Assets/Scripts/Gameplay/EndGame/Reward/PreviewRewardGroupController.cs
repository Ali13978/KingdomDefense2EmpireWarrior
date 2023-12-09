using Parameter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class PreviewRewardGroupController : MonoBehaviour
	{
		[SerializeField]
		private Image[] listItem;

		[SerializeField]
		private Text[] listItemQuantity;

		public void InitListPreviewItems()
		{
			List<int> listItemsPreview = LuckyChestParameter.Instance.GetListItemsPreview();
			UnityEngine.Debug.Log(listItem);
			for (int i = 0; i < listItem.Length; i++)
			{
				SetItemAvatar(i, listItemsPreview[i]);
			}
		}

		private void SetItemAvatar(int listItemIndex, int itemID)
		{
			switch (itemID)
			{
			case 0:
			{
				List<int> listHeroID = HeroParameter.Instance.GetListHeroID();
				listHeroID.Remove(0);
				listHeroID.Remove(1);
				listHeroID.Remove(2);
				if (listHeroID.Count > 0)
				{
					int num = listHeroID[Random.Range(0, listHeroID.Count)];
					listItem[listItemIndex].sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_hero_{num}");
					listItemQuantity[listItemIndex].text = string.Empty;
				}
				else
				{
					listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
					listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				}
				break;
			}
			case 1:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_hero_offer");
				listItemQuantity[listItemIndex].text = string.Empty;
				break;
			case 2:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_hero_1_max");
				listItemQuantity[listItemIndex].text = string.Empty;
				break;
			case 3:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_hero_2_max");
				listItemQuantity[listItemIndex].text = string.Empty;
				break;
			case 4:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
				listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				break;
			case 5:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
				listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				break;
			case 6:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
				listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				break;
			case 7:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_0");
				listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				break;
			case 8:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_1");
				listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				break;
			case 9:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_2");
				listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				break;
			case 10:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_3");
				listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				break;
			case 11:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_pw_1");
				listItemQuantity[listItemIndex].text = LuckyChestParameter.Instance.GetChestValue(itemID, 0).ToString();
				break;
			case 12:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_hero_0");
				listItemQuantity[listItemIndex].text = string.Empty;
				break;
			case 13:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_hero_1");
				listItemQuantity[listItemIndex].text = string.Empty;
				break;
			case 14:
				listItem[listItemIndex].sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_hero_offer");
				listItemQuantity[listItemIndex].text = string.Empty;
				break;
			}
		}
	}
}
