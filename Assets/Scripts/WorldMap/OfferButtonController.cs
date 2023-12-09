using LifetimePopup;
using UnityEngine;

namespace WorldMap
{
	public class OfferButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.Init();
			CustomInvoke(DoOpen, Time.deltaTime);
		}

		private void DoOpen()
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(2);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.TabsGroupController.HighLightButton(2);
		}
	}
}
