using Parameter;
using SSR.Core.Architecture;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class LuckyChestIntroPopupController : MonoBehaviour
	{
		[SerializeField]
		private OrderedEventDispatcher OnShowDescriptionComplete;

		[SerializeField]
		private int[] descriptionsKey;

		[SerializeField]
		private Text description;

		[SerializeField]
		private GameObject[] turns;

		private int currentStep;

		public void Init()
		{
			Open();
			SetContent();
		}

		private void SetContent()
		{
			if (currentStep <= 2)
			{
				description.text = Singleton<NotificationDescription>.Instance.GetNotiContent(descriptionsKey[currentStep]).Replace('@', '\n').Replace('#', '-');
				GameObject[] array = turns;
				foreach (GameObject gameObject in array)
				{
					gameObject.SetActive(value: false);
				}
				turns[currentStep].SetActive(value: true);
			}
		}

		public void OnClickButtonNext()
		{
			currentStep++;
			SetContent();
			if (currentStep >= 3)
			{
				OnShowDescriptionComplete.Dispatch();
			}
		}

		private void Open()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Close()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
