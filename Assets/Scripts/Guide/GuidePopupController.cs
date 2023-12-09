using Data;
using Gameplay;
using Services.PlatformSpecific;
using UnityEngine;

namespace Guide
{
	public class GuidePopupController : GameplayPopupController
	{
		[SerializeField]
		private GuideTowerController guideTowerController;

		[SerializeField]
		private GuideEnemyController guideEnemyController;

		[SerializeField]
		private GuideTipsController guideTipsController;

		[SerializeField]
		private SelectedImageController selectedEnemyImage;

		[SerializeField]
		private SelectedImageController selectedTowerImage;

		private static GuidePopupController _instance;

		public GuideTowerController GuideTowerController
		{
			get
			{
				return guideTowerController;
			}
			set
			{
				guideTowerController = value;
			}
		}

		public GuideEnemyController GuideEnemyController
		{
			get
			{
				return guideEnemyController;
			}
			set
			{
				guideEnemyController = value;
			}
		}

		public GuideTipsController GuideTipsController
		{
			get
			{
				return guideTipsController;
			}
			set
			{
				guideTipsController = value;
			}
		}

		public static GuidePopupController Instance => _instance;

		private void Awake()
		{
			_instance = this;
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			HideSelectedEnemyImage();
			SendEventOpenPanel();
		}

		private void SendEventOpenPanel()
		{
			int maxMapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked() + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_OpenGuide(maxMapIDUnlocked);
		}

		public void ShowSelectedEnemyImage(Transform transform)
		{
			if (!selectedEnemyImage.gameObject.activeSelf)
			{
				selectedEnemyImage.gameObject.SetActive(value: true);
			}
			selectedEnemyImage.Init(transform);
		}

		public void ShowSelectedTowerImage(Transform transform)
		{
			if (!selectedTowerImage.gameObject.activeSelf)
			{
				selectedTowerImage.gameObject.SetActive(value: true);
			}
			selectedTowerImage.Init(transform);
		}

		public void HideSelectedEnemyImage()
		{
			selectedEnemyImage.gameObject.SetActive(value: false);
		}

		public void HideSelectedTowerImage()
		{
			selectedTowerImage.gameObject.SetActive(value: false);
		}

		public void OpenGuideTower(float delayTime)
		{
			CustomInvoke(DoOpenGuideTower, delayTime);
		}

		private void DoOpenGuideTower()
		{
			GuideTowerController.Init();
		}

		public void OpenGuideEnemy(float delayTime)
		{
			CustomInvoke(DoOpenGuideEnemy, delayTime);
		}

		private void DoOpenGuideEnemy()
		{
			GuideEnemyController.Init();
		}

		public void OpenGuideTip(float delayTime)
		{
			CustomInvoke(DoOpenGuideTip, delayTime);
		}

		private void DoOpenGuideTip()
		{
			GuideTipsController.Init();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
