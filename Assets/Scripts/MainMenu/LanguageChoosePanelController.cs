using UnityEngine;

namespace MainMenu
{
	public class LanguageChoosePanelController : MonoBehaviour
	{
		[HideInInspector]
		public bool isOpen;

		public void Init()
		{
			Open();
		}

		public void Open()
		{
			base.gameObject.SetActive(value: true);
			isOpen = true;
		}

		public void Close()
		{
			base.gameObject.SetActive(value: false);
			isOpen = false;
		}
	}
}
