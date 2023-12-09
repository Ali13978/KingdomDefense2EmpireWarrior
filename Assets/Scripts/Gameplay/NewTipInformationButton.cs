using Services.PlatformSpecific;

namespace Gameplay
{
	public class NewTipInformationButton : ButtonController
	{
		public int TipId
		{
			get;
			set;
		}

		public void ShowButton(float buttonLifeTime)
		{
			base.gameObject.SetActive(value: true);
			CustomInvoke(HideButton, buttonLifeTime);
			SendEventShowButton();
			UISoundManager.Instance.PlayNewTipButton();
		}

		private void SendEventShowButton()
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_ShowTipsButton("New Tips", TipId);
		}

		public override void OnClick()
		{
			base.OnClick();
			ShowCard();
			HideButton();
		}

		public void ShowCard()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.GameplayTipPopup.Init(TipId);
		}

		private void HideButton()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
