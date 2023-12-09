using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class SelectTipButtonController : ButtonController
	{
		[SerializeField]
		private int tipID;

		[SerializeField]
		private Text tipName;

		private void Start()
		{
			SetTipName();
		}

		private void SetTipName()
		{
			tipName.text = Singleton<GameplayTipsDescription>.Instance.GetName(tipID);
		}

		public override void OnClick()
		{
			base.OnClick();
			GuidePopupController.Instance.GuideTipsController.currentTipIDSelected = tipID;
			GuidePopupController.Instance.GuideTipsController.RefreshTipInformation();
		}
	}
}
