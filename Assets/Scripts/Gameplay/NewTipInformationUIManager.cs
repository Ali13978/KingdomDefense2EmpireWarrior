using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class NewTipInformationUIManager : MonoBehaviour
	{
		[Space]
		[SerializeField]
		private Transform cardsRoot;

		[Space]
		[SerializeField]
		private float buttonLifeTime = 60f;

		private static NewTipInformationUIManager instance;

		public static NewTipInformationUIManager Instance
		{
			get
			{
				return instance;
			}
			private set
			{
				instance = value;
			}
		}

		public void Awake()
		{
			Instance = this;
		}

		public void TryActivateButton(int tipID)
		{
			string path = $"NewTip/ButtonNewTip";
			NewTipInformationButton newTipInformationButton = UnityEngine.Object.Instantiate(Resources.Load<NewTipInformationButton>(path));
			newTipInformationButton.gameObject.SetActive(value: false);
			newTipInformationButton.transform.SetParent(cardsRoot);
			newTipInformationButton.transform.localScale = Vector3.one;
			newTipInformationButton.TipId = tipID;
			newTipInformationButton.ShowButton(buttonLifeTime);
			if (tipID == 5)
			{
				newTipInformationButton.GetComponent<Image>().sprite = Resources.Load<Sprite>($"NewTip/icon_newtips_lives");
			}
			if (tipID == 6)
			{
				newTipInformationButton.GetComponent<Image>().sprite = Resources.Load<Sprite>($"NewTip/icon_newtips_coin");
			}
		}
	}
}
