namespace WorldMap
{
	public class EndlessButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenEndlessPopup();
		}

		private void OpenEndlessPopup()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.endlessPopupController.Init();
		}
	}
}
