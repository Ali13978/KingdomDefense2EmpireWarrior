using MyCustom;
using System.Collections;
using UnityEngine;

namespace MainMenu
{
	public class MainMenuButtonsGroupController : CustomMonoBehaviour
	{
		[SerializeField]
		private GameObject[] listButtons;

		[SerializeField]
		private float delayTimeToStart;

		[SerializeField]
		private float delayTimeBetweenObjects;

		private WaitForSeconds waitToStart;

		private WaitForSeconds waitDelay;

		private void Start()
		{
			waitToStart = new WaitForSeconds(delayTimeToStart);
			waitDelay = new WaitForSeconds(delayTimeBetweenObjects);
			StartCoroutine(ShowGroupButtons());
		}

		private IEnumerator ShowGroupButtons()
		{
			yield return waitToStart;
			for (int i = 0; i < listButtons.Length; i++)
			{
				listButtons[i].SetActive(value: true);
				yield return waitDelay;
			}
		}
	}
}
