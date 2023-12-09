using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace RewardPopup
{
	public class ItemsGroupController : MonoBehaviour
	{
		[SerializeField]
		private Text gemAmount;

		[SerializeField]
		private Image[] itemsAvatar;

		[SerializeField]
		private Text[] itemsAmount;

		public void InitValue(string bundleID)
		{
			OfferBundleGemNItems offerBundleItems = SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.ReadDataOfferBundle.GetOfferBundleItems(bundleID);
			gemAmount.text = offerBundleItems.gemAmount.ToString();
			for (int i = 0; i < itemsAvatar.Length; i++)
			{
				itemsAvatar[i].sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{i}");
				itemsAmount[i].text = offerBundleItems.itemsAmount[i].ToString();
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
