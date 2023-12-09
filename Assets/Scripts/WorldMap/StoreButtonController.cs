using LifetimePopup;
using UnityEngine;

namespace WorldMap
{
	public class StoreButtonController : ButtonController
	{
		[SerializeField]
		private float timeToOpen;

		public override void OnClick()
		{
			base.OnClick();
			CustomInvoke(DoOpen, timeToOpen);
		}

		private void DoOpen()
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.Init();
		}
	}
}
