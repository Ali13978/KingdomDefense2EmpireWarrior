using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TowerCounter : MonoBehaviour
	{
		public Text text;

		private void Update()
		{
			text.text = SingletonMonoBehaviour<GameData>.Instance.ListActiveTower.Count.ToString();
		}
	}
}
