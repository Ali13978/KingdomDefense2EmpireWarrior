using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class GameplayTipInformationPopup : GameplayPopupController
	{
		[Space]
		[SerializeField]
		private Image imageAvatar;

		[SerializeField]
		private Text tipName;

		[SerializeField]
		private Text tipDescription;

		private int tipID;

		public void Init(int tipID)
		{
			this.tipID = tipID;
			base.OpenWithScaleAnimation();
			GameplayManager.Instance.gameSpeedController.PauseGame();
			imageAvatar.sprite = Resources.Load<Sprite>($"NewTip/avatar_tip_{tipID}");
			imageAvatar.SetNativeSize();
			tipName.text = Singleton<GameplayTipsDescription>.Instance.GetName(tipID);
			tipDescription.text = Singleton<GameplayTipsDescription>.Instance.GetDescription(tipID).Replace('@', '\n').Replace('#', '-');
			SendEventOpenPopup();
		}

		private void SendEventOpenPopup()
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_OpenTipsPopup("New Tips", tipID);
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
		}
	}
}
