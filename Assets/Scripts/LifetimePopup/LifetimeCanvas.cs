using ApplicationEntry;
using Data;
using FreeResources;
using OfferPopup;
using Store;
using UnityEngine;

namespace LifetimePopup
{
	public class LifetimeCanvas : SingletonMonoBehaviour<LifetimeCanvas>
	{
		[SerializeField]
		private RewardPopupController rewardPopupController;

		[SerializeField]
		private NotifyPopupController notifyPopupController;

		[SerializeField]
		private ToastPopupController toastPopupController;

		[SerializeField]
		private StorePopupController storePopupController;

		[SerializeField]
		private OfferPopupController offerPopupController;

		[SerializeField]
		private FreeResourcesPopupController freeResourcesPopupController;

		[SerializeField]
		private AskToRatePopupController askToRatePopupController;

		[SerializeField]
		private LoadingProgressPopupController loadingProgressPopupController;

		[SerializeField]
		private AskToBuyPopupController askToBuyPopupController;

		public RewardPopupController RewardPopupController
		{
			get
			{
				return rewardPopupController;
			}
			private set
			{
				rewardPopupController = value;
			}
		}

		public NotifyPopupController NotifyPopupController
		{
			get
			{
				return notifyPopupController;
			}
			set
			{
				notifyPopupController = value;
			}
		}

		public ToastPopupController ToastPopupController
		{
			get
			{
				return toastPopupController;
			}
			set
			{
				toastPopupController = value;
			}
		}

		public StorePopupController StorePopupController
		{
			get
			{
				return storePopupController;
			}
			set
			{
				storePopupController = value;
			}
		}

		public OfferPopupController OfferPopupController
		{
			get
			{
				return offerPopupController;
			}
			set
			{
				offerPopupController = value;
			}
		}

		public FreeResourcesPopupController FreeResourcesPopupController
		{
			get
			{
				return freeResourcesPopupController;
			}
			set
			{
				freeResourcesPopupController = value;
			}
		}

		public AskToRatePopupController AskToRatePopupController
		{
			get
			{
				return askToRatePopupController;
			}
			set
			{
				askToRatePopupController = value;
			}
		}

		public LoadingProgressPopupController LoadingProgressPopupController
		{
			get
			{
				return loadingProgressPopupController;
			}
			set
			{
				loadingProgressPopupController = value;
			}
		}

		public AskToBuyPopupController AskToBuyPopupController
		{
			get
			{
				return askToBuyPopupController;
			}
			set
			{
				askToBuyPopupController = value;
			}
		}

		public bool IsLoadingProgress()
		{
			return LoadingProgressPopupController.IsGameObjectActive();
		}

		public void TryToOpenPopupAskRate(int currentMapID)
		{
			int chanceToShowAskRating = ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.GetChanceToShowAskRating();
			if ((currentMapID == 2 || currentMapID == 4) && !ReadWriteData.Instance.IsUserRated() && ReadWriteDataMap.Instance.GetStarEarnedByMap(currentMapID) == 3 && Random.Range(0, 100) < chanceToShowAskRating)
			{
				PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.ratePopupPrefab, PopupPriorityEnum.Normal);
			}
		}
	}
}
