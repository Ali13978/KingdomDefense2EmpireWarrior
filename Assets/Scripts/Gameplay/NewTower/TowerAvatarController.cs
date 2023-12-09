using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.NewTower
{
	public class TowerAvatarController : MonoBehaviour
	{
		[SerializeField]
		private Text towerNameText;

		[SerializeField]
		private Text towerLevelText;

		[SerializeField]
		private int towerID;

		[SerializeField]
		private int towerLevel;

		private void Start()
		{
			towerNameText.text = Singleton<TowerDescription>.Instance.GetTowerName(towerID);
			towerLevelText.text = "Level " + (towerLevel + 1).ToString();
		}
	}
}
