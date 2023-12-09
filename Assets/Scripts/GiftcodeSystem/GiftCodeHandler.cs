using Data;
using LifetimePopup;
using Parameter;
using UnityEngine;
using WorldMap;

namespace GiftcodeSystem
{
	public class GiftCodeHandler : MonoBehaviour
	{
		private void Awake()
		{
			SingletonMonoBehaviour<WorldMapManager>.Instance.GiftCodeManager.onGiftCodeSubmited += GiftCodeManager_onGiftCodeSubmited;
		}

		private void OnDestroy()
		{
			SingletonMonoBehaviour<WorldMapManager>.Instance.GiftCodeManager.onGiftCodeSubmited -= GiftCodeManager_onGiftCodeSubmited;
		}

		private void GiftCodeManager_onGiftCodeSubmited(ReceivedGiftCodeMessage obj)
		{
			if (obj.bonus != null)
			{
				string giftCodeType = GiftCodeStaticVariable.GetGiftCodeType(obj.bonus);
				UnityEngine.Debug.Log(giftCodeType);
				if (giftCodeType == null)
				{
					return;
				}
				if (!(giftCodeType == "HERO"))
				{
					if (!(giftCodeType == "GEMS"))
					{
						if (giftCodeType == "HERONGEM")
						{
							ProcessGiftCodeHeroNGem(obj);
						}
					}
					else
					{
						ProcessGiftCodeGems(obj);
					}
				}
				else
				{
					ProcessGiftCodeHero(obj);
				}
			}
			else
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(121);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		private void ProcessGiftCodeHero(ReceivedGiftCodeMessage obj)
		{
			GiftCodeHeroNGem giftCodeHeroNGem = GiftCodeStaticVariable.GetGiftCodeHeroNGem(obj.bonus);
			int num = -1;
			num = ((!string.IsNullOrEmpty(giftCodeHeroNGem.heroid)) ? int.Parse(giftCodeHeroNGem.heroid) : 0);
			if (!ReadWriteDataHero.Instance.IsHeroOwned(num))
			{
				ReadWriteDataHero.Instance.UnlockHero(num);
				RewardItem[] array = new RewardItem[1];
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.SingleHero;
				rewardItem.itemID = num;
				rewardItem.isDisplayQuantity = false;
				array[0] = rewardItem;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			}
			else
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(122);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		private void ProcessGiftCodeGems(ReceivedGiftCodeMessage obj)
		{
			GiftCodeGems giftCodeGems = GiftCodeStaticVariable.GetGiftCodeGems(obj.bonus);
			int num = int.Parse(giftCodeGems.gems);
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(num, isDispatchEventChange: true);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = num;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
		}

		private void ProcessGiftCodeHeroNGem(ReceivedGiftCodeMessage obj)
		{
			GiftCodeHeroNGem giftCodeHeroNGem = GiftCodeStaticVariable.GetGiftCodeHeroNGem(obj.bonus);
			int num = int.Parse(giftCodeHeroNGem.heroid);
			int num2 = int.Parse(giftCodeHeroNGem.gems);
			if (!ReadWriteDataHero.Instance.IsHeroOwned(num))
			{
				ReadWriteDataHero.Instance.UnlockHero(num);
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(num2, isDispatchEventChange: true);
				RewardItem[] array = new RewardItem[2];
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.SingleHero;
				rewardItem.itemID = num;
				rewardItem.isDisplayQuantity = false;
				array[0] = rewardItem;
				RewardItem rewardItem2 = new RewardItem();
				rewardItem2.rewardType = RewardType.Gem;
				rewardItem2.value = num2;
				rewardItem2.isDisplayQuantity = true;
				array[1] = rewardItem2;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			}
			else
			{
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(num2, isDispatchEventChange: true);
				RewardItem[] array2 = new RewardItem[1];
				RewardItem rewardItem3 = new RewardItem();
				rewardItem3.rewardType = RewardType.Gem;
				rewardItem3.value = num2;
				rewardItem3.isDisplayQuantity = true;
				array2[0] = rewardItem3;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array2);
			}
		}
	}
}
