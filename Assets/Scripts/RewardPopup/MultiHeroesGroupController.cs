using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace RewardPopup
{
	public class MultiHeroesGroupController : MonoBehaviour
	{
		[SerializeField]
		private Image[] heroesAvatar;

		public void InitValue(string bundleID)
		{
			OfferComboHeroes offerBundleComboHeroes = SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.ReadDataOfferBundle.GetOfferBundleComboHeroes(bundleID);
			for (int i = 0; i < heroesAvatar.Length; i++)
			{
				heroesAvatar[i].sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_hero_{offerBundleComboHeroes.listHeroesID[i]}");
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
