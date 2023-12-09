using UnityEngine;

namespace WorldMap
{
	public class GuideButtonController : ButtonController
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
			SingletonMonoBehaviour<UIRootController>.Instance.guidePopupController.Init();
		}
	}
}
