using Data;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace UserProfile
{
	public class ChangeRegionButtonController : ButtonController
	{
		[SerializeField]
		private Image image;

		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private int imageMaxHeight;

		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.userProfilePopupController.ChangeRegionPopupController.Init();
		}

		private void Start()
		{
			UpdateImage();
		}

		public void UpdateImage()
		{
			string userRegionCode = ReadWriteDataUserProfile.Instance.GetUserRegionCode();
			image.sprite = Resources.Load<Sprite>($"CountryFlags2/{userRegionCode}");
			image.SetNativeSize();
			Vector2 sizeDelta = rectTransform.sizeDelta;
			float x = sizeDelta.x;
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			float num = x / sizeDelta2.y;
			Vector2 sizeDelta3 = rectTransform.sizeDelta;
			if (sizeDelta3.y > (float)imageMaxHeight)
			{
				int num2 = imageMaxHeight;
				float x2 = (float)imageMaxHeight * num;
				rectTransform.sizeDelta = new Vector2(x2, num2);
			}
		}
	}
}
