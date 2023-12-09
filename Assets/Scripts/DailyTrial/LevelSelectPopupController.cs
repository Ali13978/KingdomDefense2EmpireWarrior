using Data;
using Gameplay;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class LevelSelectPopupController : GameplayPopupController
	{
		[Space]
		[Header("Controller")]
		[SerializeField]
		private DailyTab inputHero;

		[SerializeField]
		private MissionGroupController missionGroupController;

		[Space]
		[SerializeField]
		private Text title;

		[SerializeField]
		private GameObject offerButton;

		private int currentDay;

		public void Init()
		{
			currentDay = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			title.text = DailyTrialParameter.Instance.GetTitle(currentDay);
			OpenWithScaleAnimation();
			InitDailyHeroInfor();
			missionGroupController.InitAllMissionsState();
			InitOfferButton();
		}

		private void InitOfferButton()
		{
			offerButton.SetActive(value: false);
		}

		private void InitDailyHeroInfor()
		{
			inputHero.Day = currentDay;
			inputHero.Init();
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
