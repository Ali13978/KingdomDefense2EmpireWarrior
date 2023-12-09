using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class GameplayTutorialPanel : MonoBehaviour
	{
		public void Show()
		{
			base.gameObject.SetActive(value: true);
			SingletonMonoBehaviour<GameData>.Instance.IsAnyTutorialPopupOpen = true;
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
			SingletonMonoBehaviour<GameData>.Instance.IsAnyTutorialPopupOpen = false;
		}
	}
}
