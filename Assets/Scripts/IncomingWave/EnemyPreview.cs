using MyCustom;
using UnityEngine;
using UnityEngine.UI;

namespace IncomingWave
{
	public class EnemyPreview : CustomMonoBehaviour
	{
		[SerializeField]
		private Image avatar;

		[SerializeField]
		private Text amount;

		private int enemyID;

		private int enemyAmount;

		public void Init(int enemyID, int amount)
		{
			this.enemyID = enemyID;
			avatar.sprite = Resources.Load<Sprite>($"Preview/Enemies/p_enemy_{enemyID}");
			avatar.SetNativeSize();
			enemyAmount = amount;
			this.amount.text = "X " + enemyAmount.ToString();
			Show();
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
