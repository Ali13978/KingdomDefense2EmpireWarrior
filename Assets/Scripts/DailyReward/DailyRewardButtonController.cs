namespace DailyReward
{
	public class DailyRewardButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.dailyRewardPopupPrefab, PopupPriorityEnum.Normal);
		}
	}
}
