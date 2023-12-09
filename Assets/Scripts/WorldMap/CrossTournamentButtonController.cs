using UnityEngine;

namespace WorldMap
{
	public class CrossTournamentButtonController : ButtonController
	{
		private int rewardValue;

		private bool isGameInstalled;

		[SerializeField]
		private float timeToOpen;

		public override void OnClick()
		{
			base.OnClick();
			DoOpen();
		}

		private void DoOpen()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.linkGamePopupController.Init();
			SingletonMonoBehaviour<UIRootController>.Instance.linkGamePopupController.linkWithoutGems.SetActive(value: true);
			SingletonMonoBehaviour<UIRootController>.Instance.linkGamePopupController.linkWithGems.SetActive(value: false);
		}
	}
}
