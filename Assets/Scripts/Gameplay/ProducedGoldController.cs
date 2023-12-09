using UnityEngine;

namespace Gameplay
{
	public class ProducedGoldController : MonoBehaviour
	{
		[SerializeField]
		private TextMesh totalGoldText;

		private int totalGold;

		public void ResetParameter()
		{
			totalGold = 0;
			Hide();
		}

		public void Init(int goldAmount)
		{
			Show();
			totalGold += goldAmount;
			totalGoldText.text = totalGold.ToString();
		}

		public void TapOnGold()
		{
			SingletonMonoBehaviour<GameData>.Instance.IncreaseMoney(totalGold);
			ResetParameter();
		}

		private void Show()
		{
			base.gameObject.SetActive(value: true);
			totalGoldText.gameObject.SetActive(value: true);
		}

		private void Hide()
		{
			base.gameObject.SetActive(value: false);
			totalGoldText.gameObject.SetActive(value: false);
		}
	}
}
