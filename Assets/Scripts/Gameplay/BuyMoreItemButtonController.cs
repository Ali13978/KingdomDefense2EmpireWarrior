using LifetimePopup;

namespace Gameplay
{
	public class BuyMoreItemButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			if (!SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				SingletonMonoBehaviour<UIRootController>.Instance.settingPopupController.Open();
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.Init();
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(1);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.TabsGroupController.HighLightButton(1);
			}
		}
	}
}
