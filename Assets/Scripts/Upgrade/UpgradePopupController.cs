using Data;
using Gameplay;
using Services.PlatformSpecific;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace Upgrade
{
	public class UpgradePopupController : GameplayPopupController
	{
		public UpgradeInformationController upgradeInformationController;

		[Header("Upgrade group")]
		public List<UpgradeGroupController> upgradeGroupControllers = new List<UpgradeGroupController>();

		[Space]
		[Header("o'3'o")]
		[SerializeField]
		private GameObject upgradeSelectedImage;

		[SerializeField]
		private GameObject upgradedEffect;

		[SerializeField]
		private Animator upgradedEffectAnimator;

		[SerializeField]
		private UpgradeButtonController upgradeButton;

		[SerializeField]
		private ResetButtonController resetButton;

		[HideInInspector]
		public int currentTowerIDSelected;

		[HideInInspector]
		public int currentTierSelected;

		[Space]
		[Header("Player currency")]
		[SerializeField]
		private Text playerStar;

		[NonSerialized]
		public int currentStar;

		private static UpgradePopupController _instance;

		public static UpgradePopupController Instance => _instance;

		private void Awake()
		{
			_instance = this;
		}

		private void Start()
		{
			InitUpgradeStatus();
			HideSelectedImage();
			SetDefaultData();
			CalculateCurrentStar();
		}

		private void OnEnable()
		{
			InitUpgradeStatus();
			HideSelectedImage();
			SetDefaultData();
		}

		public void Init()
		{
			CalculateCurrentStar();
			OpenWithScaleAnimation();
			SendEventOpenPanel();
		}

		private void SendEventOpenPanel()
		{
			int totalStar = ReadWriteDataPlayerCurrency.Instance.GetCurrentStar();
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_OpenGlobalUpgrade(currentStar, totalStar);
		}

		private void SetDefaultData()
		{
			currentTowerIDSelected = -1;
			currentTierSelected = -1;
			upgradeButton.gameObject.SetActive(value: false);
			upgradeInformationController.InitDefaultData();
		}

		private void InitUpgradeStatus()
		{
			for (int i = 0; i < upgradeGroupControllers.Count; i++)
			{
				upgradeGroupControllers[i].currentUpgradeLevel = ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(i);
				upgradeGroupControllers[i].RefreshListTier();
			}
		}

		private void HideSelectedImage()
		{
			upgradeSelectedImage.SetActive(value: false);
		}

		public void ShowUpgradedEffect()
		{
			upgradedEffect.transform.position = upgradeGroupControllers[currentTowerIDSelected].listTierUpgrade[currentTierSelected].transform.position;
			upgradedEffectAnimator.SetTrigger("Effect");
		}

		public void ShowSelectedUpgradeImage(Vector3 pos)
		{
			upgradeSelectedImage.SetActive(value: true);
			upgradeSelectedImage.transform.position = pos;
		}

		public void TryShowButtonUpgrade(bool canUpgrade)
		{
			upgradeButton.gameObject.SetActive(canUpgrade);
			upgradeButton.RefreshButtonStatus();
		}

		private void TryShowButtonReset()
		{
		}

		public void Reset()
		{
			for (int i = 0; i < upgradeGroupControllers.Count; i++)
			{
				upgradeGroupControllers[i].currentUpgradeLevel = -1;
				upgradeGroupControllers[i].RefreshListTier();
				ReadWriteDataGlobalUpgrade.Instance.Save(i, -1);
			}
			TryShowButtonUpgrade(canUpgrade: false);
			HideSelectedImage();
			CalculateCurrentStar();
		}

		public void CalculateCurrentStar()
		{
			int num = ReadWriteDataPlayerCurrency.Instance.GetCurrentStar();
			int num2 = 0;
			for (int i = 0; i < upgradeGroupControllers.Count; i++)
			{
				for (int j = 0; j <= ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(i); j++)
				{
					int starRequireForUpgrade = ReadWriteDataGlobalUpgrade.Instance.GetStarRequireForUpgrade(i, j);
					num2 += starRequireForUpgrade;
				}
			}
			currentStar = num - num2;
			playerStar.text = currentStar + "/" + num;
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			SingletonMonoBehaviour<WorldMapManager>.Instance.WorldMapTutorial.SetTutorialPassed();
		}
	}
}
