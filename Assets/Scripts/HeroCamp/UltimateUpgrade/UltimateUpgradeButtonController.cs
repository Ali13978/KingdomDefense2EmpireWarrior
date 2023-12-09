namespace HeroCamp.UltimateUpgrade
{
	public class UltimateUpgradeButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			InitUltimateUpgradePopup();
		}

		private void InitUltimateUpgradePopup()
		{
			HeroCampPopupController.Instance.UltimateUpgradePopupController.Init();
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
