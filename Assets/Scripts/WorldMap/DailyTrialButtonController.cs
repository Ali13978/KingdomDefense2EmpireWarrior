namespace WorldMap
{
	public class DailyTrialButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenDailyTrialPanel();
		}

		private void OpenDailyTrialPanel()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.dailyTrialPopupController.Init();
		}
	}
}
