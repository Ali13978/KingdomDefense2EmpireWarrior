using UnityEngine;
using UnityEngine.UI;

namespace RewardPopup
{
	public class GeneralItem : MonoBehaviour
	{
		[SerializeField]
		private Image itemAvatar;

		[SerializeField]
		private Text itemQuantity;

		public void Init(RewardItem data)
		{
			Show();
			if (data != null && data.isDisplayQuantity && data.value == 0)
			{
				Hide();
			}
			GameTools.SetRewardSprite(data, itemAvatar);
			itemQuantity.text = data.value.ToString();
			itemQuantity.gameObject.SetActive(data.isDisplayQuantity);
		}

		private void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
