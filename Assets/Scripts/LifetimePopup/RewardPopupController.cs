using Gameplay;
using RewardPopup;
using UnityEngine;

namespace LifetimePopup
{
	public class RewardPopupController : GameplayPopupController
	{
		[Header("Controllers")]
		[SerializeField]
		private GeneralItemGroupController generalItemGroupController;

		public void Init(RewardItem[] listData)
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			generalItemGroupController.InitListItems(listData);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
