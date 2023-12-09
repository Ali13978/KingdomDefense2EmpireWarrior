using Data;
using Gameplay;
using HeroCamp.UltimateUpgrade;
using Parameter;
using Services.PlatformSpecific;
using SSR.Core.Architecture;
using System;
using System.Collections.Generic;
using UnityEngine;
using WorldMap;

namespace HeroCamp
{
	public class HeroCampPopupController : GameplayPopupController
	{
		[Space]
		[SerializeField]
		private OrderedEventDispatcher OnInitEvent;

		[SerializeField]
		private HeroLevelInformation heroLevelInformation;

		[SerializeField]
		private HeroSkillGroupController heroSkillGroupController;

		[Space]
		[Header("Select Hero Group")]
		[SerializeField]
		private SelectHeroButtonGroupController selectHeroButtonGroupController;

		[SerializeField]
		private SelectedImageController selectedHeroImage;

		[Space]
		[Header("Upgrade N Buy")]
		[SerializeField]
		private UpgradeNBuyGroupController upgradeNBuyGroupController;

		[Space]
		[Header("Action Avatars")]
		[SerializeField]
		private HeroActionAvatarGroupController heroActionAvatarGroupController;

		[SerializeField]
		private PetActionAvatarGroupController petActionAvatarGroupController;

		[Space]
		[Header("Ultimate Upgrade")]
		[SerializeField]
		private UltimateUpgradePopupController ultimateUpgradePopupController;

		[NonSerialized]
		public int currentHeroID = -1;

		[Space]
		[Header("Visual Effect")]
		[SerializeField]
		private GameObject unlockedEffect;

		[SerializeField]
		private Animator unlockedEffectAnimator;

		[SerializeField]
		private Animator levelUpAnimator;

		private static HeroCampPopupController _instance;

		public HeroLevelInformation HeroLevelInformation
		{
			get
			{
				return heroLevelInformation;
			}
			set
			{
				heroLevelInformation = value;
			}
		}

		public HeroSkillGroupController HeroSkillGroupController
		{
			get
			{
				return heroSkillGroupController;
			}
			set
			{
				heroSkillGroupController = value;
			}
		}

		public SelectHeroButtonGroupController SelectHeroButtonGroupController
		{
			get
			{
				return selectHeroButtonGroupController;
			}
			set
			{
				selectHeroButtonGroupController = value;
			}
		}

		public UpgradeNBuyGroupController UpgradeNBuyGroupController
		{
			get
			{
				return upgradeNBuyGroupController;
			}
			set
			{
				upgradeNBuyGroupController = value;
			}
		}

		public HeroActionAvatarGroupController HeroActionAvatarGroupController
		{
			get
			{
				return heroActionAvatarGroupController;
			}
			set
			{
				heroActionAvatarGroupController = value;
			}
		}

		public UltimateUpgradePopupController UltimateUpgradePopupController
		{
			get
			{
				return ultimateUpgradePopupController;
			}
			set
			{
				ultimateUpgradePopupController = value;
			}
		}

		public static HeroCampPopupController Instance => _instance;

		private void Awake()
		{
			_instance = this;
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			ChooseDefaultHero(2);
			SelectHeroButtonGroupController.Init();
			OnInitEvent.Dispatch();
			SendEventOpenPanel();
		}

		private void SendEventOpenPanel()
		{
			int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			int maxMapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked() + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_OpenHeroCamp(currentGem, maxMapIDUnlocked);
		}

		public void ChooseDefaultHero(int heroID)
		{
			SelectHeroButtonGroupController.AutoChoseHero(heroID);
		}

		public void RefreshHeroInformation()
		{
			int currentHeroLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(currentHeroID);
			HeroLevelInformation.Init(currentHeroID, currentHeroLevel);
			HeroSkillGroupController.Init(currentHeroID, currentHeroLevel);
			UpgradeNBuyGroupController.RefreshStatus();
			HeroActionAvatarGroupController.ShowSelectedHeroActionAvatar(currentHeroID);
			TryToShowPetInfor();
			SelectHeroButtonGroupController.Init();
		}

		private void TryToShowPetInfor()
		{
			petActionAvatarGroupController.HideAll();
			if (ReadWriteDataHero.Instance.IsPetUnlocked(currentHeroID))
			{
				int petID = HeroParameter.Instance.GetPetID(currentHeroID);
				petActionAvatarGroupController.ShowSelectedPetActionAvatar(petID);
			}
		}

		public void ShowSelectedHeroImage(Transform transform)
		{
			selectedHeroImage.gameObject.SetActive(value: true);
			selectedHeroImage.Init(transform);
		}

		private void HideSelectedHeroImage()
		{
			selectedHeroImage.gameObject.SetActive(value: false);
		}

		public void ShowUnlockEffect(int heroID)
		{
			List<SelectHeroButtonController> listSelectHeroButton = SelectHeroButtonGroupController.listSelectHeroButton;
			foreach (SelectHeroButtonController item in listSelectHeroButton)
			{
				if (item.HeroID == heroID)
				{
					unlockedEffect.transform.position = item.transform.position;
					unlockedEffectAnimator.SetTrigger("Effect");
				}
			}
		}

		public void ShowLevelUpEffect()
		{
			levelUpAnimator.SetTrigger("Effect");
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			HideSelectedHeroImage();
			if (!ReadWriteDataTutorial.Instance.GetTutorialStatus(ReadWriteDataTutorial.TUTORIAL_ID_WORLD_MAP))
			{
				SingletonMonoBehaviour<WorldMapManager>.Instance.WorldMapTutorial.NextStep();
			}
			PlatformSpecificServicesProvider.Services.DataCloudSaver.AutoBackUpData();
		}
	}
}
