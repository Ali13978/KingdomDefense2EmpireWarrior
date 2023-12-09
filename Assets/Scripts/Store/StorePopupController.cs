using Data;
using Gameplay;
using Services.PlatformSpecific;
using UnityEngine;

namespace Store
{
	public class StorePopupController : GameplayPopupController
	{
		[Space]
		[Header("Controllers")]
		[SerializeField]
		private ItemPowerUpGroupController itemPowerUpGroupController;

		[SerializeField]
		private GemPackGroupController gemPackGroupController;

		[SerializeField]
		private SaleBundleGroupController saleBundleGroupController;

		[SerializeField]
		private TabsGroupController tabsGroupController;

		[SerializeField]
		private ReadDataShopItemAttribute readDataShopItemAttribute;

		[Space]
		[Header("Gem Controller")]
		[SerializeField]
		private TotalGem gemController;

		public ItemPowerUpGroupController ItemPowerUpGroupController
		{
			get
			{
				return itemPowerUpGroupController;
			}
			set
			{
				itemPowerUpGroupController = value;
			}
		}

		public GemPackGroupController GemPackGroupController
		{
			get
			{
				return gemPackGroupController;
			}
			set
			{
				gemPackGroupController = value;
			}
		}

		public SaleBundleGroupController SaleBundleGroupController
		{
			get
			{
				return saleBundleGroupController;
			}
			set
			{
				saleBundleGroupController = value;
			}
		}

		public TabsGroupController TabsGroupController
		{
			get
			{
				return tabsGroupController;
			}
			set
			{
				tabsGroupController = value;
			}
		}

		public ReadDataShopItemAttribute ReadDataShopItemAttribute
		{
			get
			{
				return readDataShopItemAttribute;
			}
			set
			{
				readDataShopItemAttribute = value;
			}
		}

		private void Start()
		{
			InitDefaultData();
		}

		private void OnEnable()
		{
			InitDefaultData();
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			InitDefaultData();
			UpdateGemStatus();
			SendEventOpenPanel();
		}

		private void SendEventOpenPanel()
		{
			int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			int maxMapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked() + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_OpenStore(currentGem, maxMapIDUnlocked);
		}

		private void InitDefaultData()
		{
			ItemPowerUpGroupController.InitItemsInformation();
			GemPackGroupController.EnableScroll();
			SaleBundleGroupController.InitItemsInformation();
			SaleBundleGroupController.RefreshItemStatus();
		}

		public void ShowBuyEffect()
		{
		}

		public void UpdateGemStatus()
		{
			gemController.UpdateGemMessage();
		}

		public void PlayAnimationNotEnoughGem()
		{
			gemController.PlayAnimationNotEnoughGem();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			GemPackGroupController.DisableScroll();
		}
	}
}
