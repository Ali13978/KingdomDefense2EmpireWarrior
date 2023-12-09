using Gameplay;
using UnityEngine;

namespace DailyTrial
{
	public class DailyTrialPopupController : GameplayPopupController
	{
		[Space]
		[Header("Controllers")]
		[SerializeField]
		private DailyTabsGroupController dailyTabsGroupController;

		public void Init()
		{
			OpenWithScaleAnimation();
			dailyTabsGroupController.InitTabsData();
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
