using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace RewardPopup
{
	public class SingleHeroGroupController : MonoBehaviour
	{
		[SerializeField]
		private Image heroAvatar;

		[SerializeField]
		private Image[] itemsAvatar;

		[SerializeField]
		private Text[] itemsAmount;

		public void InitValue(string bundleID)
		{
			OfferBundleSingleHero offerBundleSingleHero = SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.ReadDataOfferBundle.GetOfferBundleSingleHero(bundleID);
			heroAvatar.sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_hero_{offerBundleSingleHero.heroID}");
			for (int i = 0; i < itemsAvatar.Length; i++)
			{
				itemsAvatar[i].sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{i}");
				itemsAmount[i].text = offerBundleSingleHero.itemsAmount[i].ToString();
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
