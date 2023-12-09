using DailyTrial;
using Data;
using Endless;
using GiftcodeSystem;
using Guide;
using HeroCamp;
using LifetimePopup;
using LinkGame;
using MapLevel;
using Tournament;
using UnityEngine;
using UnlockTheme;
using Upgrade;
using UserProfile;

namespace WorldMap
{
	public class UIRootController : SingletonMonoBehaviour<UIRootController>
	{
		[Space]
		[SerializeField]
		private float delayTimeToInitPopups;

		[Space]
		[SerializeField]
		private Transform popupParent;

		[Space]
		[Header("Prefab Panels")]
		[SerializeField]
		private GameObject heroCampPrefab;

		[SerializeField]
		private GameObject mapLevelSelectPrefab;

		[SerializeField]
		private GameObject upgradePrefab;

		[SerializeField]
		private GameObject settingPrefab;

		[SerializeField]
		private GameObject guidePrefab;

		[SerializeField]
		private GameObject unlockThemePrefab;

		[SerializeField]
		private GameObject endlessPrefab;

		[SerializeField]
		private GameObject tournamentPrefab;

		[SerializeField]
		private GameObject dailyTrialPrefab;

		[SerializeField]
		private GameObject eventQuestPrefab;

		[SerializeField]
		private GameObject linkGamePrefab;

		[SerializeField]
		private GameObject linkGameButton;

		[SerializeField]
		private GameObject giftCodePrefab;

		[SerializeField]
		private GameObject userProfilePrefab;

		[SerializeField]
		private GameObject DailyRewardPrefab;

		[Space]
		[Header("Map Themes Controller")]
		[SerializeField]
		private MapThemesController mapThemesController;

		[Space]
		[Header("UI Group")]
		[SerializeField]
		private GameObject buttonGroup_normal;

		[SerializeField]
		private GameObject buttonGroup_offer;

		[SerializeField]
		private GameObject buttonGroup_challenge;

		private bool isHeroCampPopupExist;

		private HeroCampPopupController _heroCampPopupController;

		private bool isMapLevelPopupExist;

		private MapLevelSelectPopupController _mapLevelSelectPopupController;

		private bool isUpgradePopupExist;

		private UpgradePopupController _upgradePopupController;

		private bool isSettingPopupExist;

		private SettingPopupController _settingPopupController;

		private bool isGuidePopupExist;

		private GuidePopupController _guidePopupController;

		private bool isUnlockThemePopupExist;

		private UnlockThemePopupController _unlockThemePopupController;

		private bool isEndlessPopupExist;

		private EndlessPopupController _endlessPopupController;

		private bool isTournamentPopupExist;

		private TournamentPopupController _tournamentPopupController;

		private bool isDailyTrialPopupExist;

		private DailyTrialPopupController _dailyTrialPopupController;

		private EventQuestPopup _eventQuestPopup;

		private bool isLinkGamePopupExist;

		private LinkGamePopupController _linkGamePopupController;

		private bool isGiftCodePopupExist;

		private GiftCodePopupController _giftCodePopupController;

		private bool isUserProfilePopupExist;

		private UserProfilePopupController _userProfilePopupController;

		public MapThemesController MapThemesController
		{
			get
			{
				return mapThemesController;
			}
			set
			{
				mapThemesController = value;
			}
		}

		public HeroCampPopupController heroCampPopupController
		{
			get
			{
				if (_heroCampPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(heroCampPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_heroCampPopupController = gameObject.GetComponent<HeroCampPopupController>();
					isHeroCampPopupExist = true;
				}
				return _heroCampPopupController;
			}
			set
			{
				_heroCampPopupController = value;
			}
		}

		public MapLevelSelectPopupController mapLevelSelectPopupController
		{
			get
			{
				if (_mapLevelSelectPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(mapLevelSelectPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_mapLevelSelectPopupController = gameObject.GetComponent<MapLevelSelectPopupController>();
					isMapLevelPopupExist = true;
				}
				return _mapLevelSelectPopupController;
			}
			set
			{
				_mapLevelSelectPopupController = value;
			}
		}

		public UpgradePopupController upgradePopupController
		{
			get
			{
				if (_upgradePopupController == null)
				{
					GameObject gameObject = Object.Instantiate(upgradePrefab);
					gameObject.transform.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_upgradePopupController = gameObject.GetComponent<UpgradePopupController>();
					isUpgradePopupExist = true;
				}
				return _upgradePopupController;
			}
			set
			{
				_upgradePopupController = value;
			}
		}

		public SettingPopupController settingPopupController
		{
			get
			{
				if (_settingPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(settingPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_settingPopupController = gameObject.GetComponent<SettingPopupController>();
					isSettingPopupExist = true;
				}
				return _settingPopupController;
			}
			set
			{
				_settingPopupController = value;
			}
		}

		public GuidePopupController guidePopupController
		{
			get
			{
				if (_guidePopupController == null)
				{
					GameObject gameObject = Object.Instantiate(guidePrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_guidePopupController = gameObject.GetComponent<GuidePopupController>();
					isGuidePopupExist = true;
				}
				return _guidePopupController;
			}
			set
			{
				_guidePopupController = value;
			}
		}

		public UnlockThemePopupController unlockThemePopupController
		{
			get
			{
				if (_unlockThemePopupController == null)
				{
					GameObject gameObject = Object.Instantiate(unlockThemePrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_unlockThemePopupController = gameObject.GetComponent<UnlockThemePopupController>();
					isUnlockThemePopupExist = true;
				}
				return _unlockThemePopupController;
			}
			set
			{
				_unlockThemePopupController = value;
			}
		}

		public EndlessPopupController endlessPopupController
		{
			get
			{
				if (_endlessPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(endlessPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_endlessPopupController = gameObject.GetComponent<EndlessPopupController>();
					isEndlessPopupExist = true;
				}
				return _endlessPopupController;
			}
			set
			{
				_endlessPopupController = value;
			}
		}

		public TournamentPopupController tournamentPopupController
		{
			get
			{
				if (_tournamentPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(tournamentPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_tournamentPopupController = gameObject.GetComponent<TournamentPopupController>();
					isTournamentPopupExist = true;
				}
				return _tournamentPopupController;
			}
			set
			{
				_tournamentPopupController = value;
			}
		}

		public DailyTrialPopupController dailyTrialPopupController
		{
			get
			{
				if (_dailyTrialPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(dailyTrialPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_dailyTrialPopupController = gameObject.GetComponent<DailyTrialPopupController>();
					isDailyTrialPopupExist = true;
				}
				return _dailyTrialPopupController;
			}
			set
			{
				_dailyTrialPopupController = value;
			}
		}

		public EventQuestPopup eventQuestPopup
		{
			get
			{
				if (_eventQuestPopup == null)
				{
					GameObject gameObject = Object.Instantiate(eventQuestPrefab, popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_eventQuestPopup = gameObject.GetComponent<EventQuestPopup>();
				}
				return _eventQuestPopup;
			}
			set
			{
				_eventQuestPopup = value;
			}
		}

		public LinkGamePopupController linkGamePopupController
		{
			get
			{
				if (_linkGamePopupController == null)
				{
					GameObject gameObject = Object.Instantiate(linkGamePrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_linkGamePopupController = gameObject.GetComponent<LinkGamePopupController>();
					isLinkGamePopupExist = true;
				}
				return _linkGamePopupController;
			}
			set
			{
				_linkGamePopupController = value;
			}
		}

		public GiftCodePopupController giftCodePopupController
		{
			get
			{
				if (_giftCodePopupController == null)
				{
					GameObject gameObject = Object.Instantiate(giftCodePrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_giftCodePopupController = gameObject.GetComponent<GiftCodePopupController>();
					isGiftCodePopupExist = true;
				}
				return _giftCodePopupController;
			}
			set
			{
				_giftCodePopupController = value;
			}
		}

		public UserProfilePopupController userProfilePopupController
		{
			get
			{
				if (_userProfilePopupController == null)
				{
					GameObject gameObject = Object.Instantiate(userProfilePrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_userProfilePopupController = gameObject.GetComponent<UserProfilePopupController>();
					isUserProfilePopupExist = true;
				}
				return _userProfilePopupController;
			}
			set
			{
				_userProfilePopupController = value;
			}
		}

		private void Start()
		{
			InitDefaultButtonGroups();
			CustomInvoke(InitDefaultPopups, delayTimeToInitPopups);
			RefrestLinkGameButtonStatus();
		}

		private void Update()
		{
			UpdateKeyBack();
		}

		private void InitDefaultButtonGroups()
		{
			int mapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked();
			if (mapIDUnlocked >= 1)
			{
				buttonGroup_normal.SetActive(value: true);
			}
			else
			{
				buttonGroup_normal.SetActive(value: false);
			}
			if (mapIDUnlocked >= 2)
			{
				buttonGroup_offer.SetActive(value: true);
				buttonGroup_challenge.SetActive(value: true);
			}
			else
			{
				buttonGroup_offer.SetActive(value: false);
				buttonGroup_challenge.SetActive(value: false);
			}
		}

		private void InitDefaultPopups()
		{
			heroCampPopupController.DefaultInit();
			upgradePopupController.DefaultInit();
			guidePopupController.DefaultInit();
		}

		private void UpdateKeyBack()
		{
			if (!Input.GetKeyUp(KeyCode.Escape))
			{
				return;
			}
			if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.AskToRatePopupController.isOpen)
			{
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.AskToRatePopupController.Close();
			}
			else if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.isOpen)
			{
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Close();
			}
			else if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.isOpen)
			{
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Close();
			}
			else if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.FreeResourcesPopupController.isOpen)
			{
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.FreeResourcesPopupController.CloseWithScaleAnimation();
			}
			else if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.isOpen)
			{
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.CloseWithScaleAnimation();
			}
			else if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.SingleHeroOfferController.isOpen)
			{
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.SingleHeroOfferController.CloseWithScaleAnimation();
			}
			else if (isHeroCampPopupExist && heroCampPopupController.UltimateUpgradePopupController.isOpen)
			{
				heroCampPopupController.UltimateUpgradePopupController.Close();
			}
			else if (isHeroCampPopupExist && heroCampPopupController.isOpen)
			{
				heroCampPopupController.CloseWithScaleAnimation();
			}
			else if (isMapLevelPopupExist && mapLevelSelectPopupController.isOpen)
			{
				if (mapLevelSelectPopupController.HeroesInputGroupController.AskToBuyHeroPopupController.isOpen)
				{
					mapLevelSelectPopupController.HeroesInputGroupController.AskToBuyHeroPopupController.CloseWithScaleAnimation();
				}
				else
				{
					mapLevelSelectPopupController.CloseWithScaleAnimation();
				}
			}
			else if (isUpgradePopupExist && upgradePopupController.isOpen)
			{
				upgradePopupController.CloseWithScaleAnimation();
			}
			else if (isGiftCodePopupExist && giftCodePopupController.isOpen)
			{
				giftCodePopupController.CloseWithScaleAnimation();
			}
			else if (isUserProfilePopupExist && userProfilePopupController.isOpen)
			{
				if (userProfilePopupController.ChangeNamePopupController.isOpen)
				{
					userProfilePopupController.ChangeNamePopupController.CloseWithScaleAnimation();
				}
				else if (userProfilePopupController.ChangeRegionPopupController.isOpen)
				{
					userProfilePopupController.ChangeRegionPopupController.CloseWithScaleAnimation();
				}
				else if (userProfilePopupController.ConfirmPopup.isOpen)
				{
					userProfilePopupController.ConfirmPopup.CloseWithScaleAnimation();
				}
				else
				{
					userProfilePopupController.CloseWithScaleAnimation();
				}
			}
			else if (isSettingPopupExist && settingPopupController.isOpen)
			{
				settingPopupController.CloseWithScaleAnimation();
			}
			else if (isGuidePopupExist && guidePopupController.isOpen)
			{
				if (guidePopupController.GuideEnemyController.isOpen)
				{
					guidePopupController.GuideEnemyController.Close();
				}
				else if (guidePopupController.GuideTowerController.isOpen)
				{
					guidePopupController.GuideTowerController.Close();
				}
				else if (guidePopupController.GuideTipsController.isOpen)
				{
					guidePopupController.GuideTipsController.Close();
				}
				else
				{
					guidePopupController.CloseWithScaleAnimation();
				}
			}
			else if (isUnlockThemePopupExist && unlockThemePopupController.isOpen)
			{
				unlockThemePopupController.CloseWithScaleAnimation();
			}
			else if (isDailyTrialPopupExist && dailyTrialPopupController.isOpen)
			{
				dailyTrialPopupController.CloseWithScaleAnimation();
			}
			else if (isLinkGamePopupExist && linkGamePopupController.isOpen)
			{
				linkGamePopupController.CloseWithScaleAnimation();
			}
			else if (isEndlessPopupExist && endlessPopupController.isOpen)
			{
				endlessPopupController.CloseWithScaleAnimation();
			}
			else if (isTournamentPopupExist && tournamentPopupController.isOpen)
			{
				tournamentPopupController.CloseWithScaleAnimation();
			}
			else
			{
				OpenSceneMainMenu();
			}
		}

		private void OpenSceneMainMenu()
		{
			Loading.Instance.ShowLoading();
			Invoke("DoLoadSceneMainMenu", 1f);
		}

		private void DoLoadSceneMainMenu()
		{
			GameApplication.Instance.LoadScene(GameApplication.MainMenuSceneName);
		}

		public void RefrestLinkGameButtonStatus()
		{
			if (ReadWriteDataOffers.Instance.IsOfferProcessed(ReadWriteDataOffers.KEY_INSTALL_GOE))
			{
				linkGameButton.SetActive(value: false);
			}
			else
			{
				linkGameButton.SetActive(value: true);
			}
		}
	}
}
