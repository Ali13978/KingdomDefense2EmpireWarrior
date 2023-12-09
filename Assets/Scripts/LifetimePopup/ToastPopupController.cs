using MyCustom;
using UnityEngine;
using UnityEngine.UI;

namespace LifetimePopup
{
	public class ToastPopupController : CustomMonoBehaviour
	{
		[SerializeField]
		private float popupLifeTime;

		[SerializeField]
		private Text text;

		public void Init(string value)
		{
			Open();
			text.text = value;
			CustomCancelInvoke(Close);
			CustomInvoke(Close, popupLifeTime);
		}

		private void Open()
		{
			base.gameObject.SetActive(value: true);
		}

		private void Close()
		{
			base.gameObject.SetActive(value: false);
			text.text = string.Empty;
		}
	}
}
