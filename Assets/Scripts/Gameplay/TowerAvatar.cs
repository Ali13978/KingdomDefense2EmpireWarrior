using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TowerAvatar : MonoBehaviour
	{
		[SerializeField]
		private Image image;

		private int towerID;

		private int towerLevel;

		public void Init(int towerID, int towerLevel)
		{
			this.towerID = towerID;
			this.towerLevel = towerLevel;
			image.sprite = Resources.Load<Sprite>($"Preview/Towers/p_tower_{towerID}_{towerLevel}");
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}
	}
}
