using UnityEngine;

namespace WorldMap
{
	public class UpgradeButtonController : ButtonController
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
			SingletonMonoBehaviour<UIRootController>.Instance.upgradePopupController.Init();
		}
	}
}
