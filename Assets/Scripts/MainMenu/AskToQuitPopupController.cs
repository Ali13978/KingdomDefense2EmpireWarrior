using UnityEngine;

namespace MainMenu
{
	public class AskToQuitPopupController : MonoBehaviour
	{
		[HideInInspector]
		public bool isOpen;

		public void Init()
		{
			Open();
		}

		public void QuitGame()
		{
			Application.Quit();
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
