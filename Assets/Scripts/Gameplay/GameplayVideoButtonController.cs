using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class GameplayVideoButtonController : ButtonController
	{
		[SerializeField]
		private Image[] listImages;

		[SerializeField]
		private Button[] listButtons;

		[SerializeField]
		private Sprite icon_normal;

		[SerializeField]
		private Sprite icon_lock;

		[Space]
		[SerializeField]
		private FreeResourcesPopupController freeResourcesPopupController;

		[SerializeField]
		private string rewardID;

		[SerializeField]
		private Text rewardValueText;

		private bool isPlayed;

		public bool IsPlayed
		{
			get
			{
				return isPlayed;
			}
			set
			{
				isPlayed = value;
			}
		}

		private void Start()
		{
			rewardValueText.text = freeResourcesPopupController.ReadDataAdsReward.GetRewardValue(rewardID).ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			RefreshStatus();
		}

		public void RefreshStatus()
		{
			if (isPlayed)
			{
				Image[] array = listImages;
				foreach (Image image in array)
				{
					image.color = Color.gray;
				}
				Button[] array2 = listButtons;
				foreach (Button button in array2)
				{
					button.interactable = false;
				}
			}
			else
			{
				Image[] array3 = listImages;
				foreach (Image image2 in array3)
				{
					image2.color = Color.white;
				}
				Button[] array4 = listButtons;
				foreach (Button button2 in array4)
				{
					button2.interactable = true;
				}
			}
			SingletonMonoBehaviour<UIRootController>.Instance.RefreshStatusFreeResourcesButton();
		}
	}
}
