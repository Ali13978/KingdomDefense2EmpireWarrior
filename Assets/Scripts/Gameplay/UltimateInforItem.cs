using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class UltimateInforItem : MonoBehaviour
	{
		[SerializeField]
		private Text ultimateName;

		[SerializeField]
		private Text ultimateDescription;

		public void Init(int _towerID, int _towerLevel, int posX, int skillID)
		{
			base.transform.localPosition = new Vector3(posX, 0f, 0f);
			ultimateName.text = Singleton<TowerDescription>.Instance.GetTowerUltimateName(_towerID, _towerLevel, skillID);
			ultimateDescription.text = Singleton<TowerDescription>.Instance.GetTowerUltimateDescription(_towerID, _towerLevel, skillID).Remove(0, 2).Replace('@', '\n')
				.Replace('#', '-');
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
