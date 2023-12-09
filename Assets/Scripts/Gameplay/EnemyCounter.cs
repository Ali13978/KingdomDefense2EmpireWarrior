using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class EnemyCounter : MonoBehaviour
	{
		public Text text;

		private void Update()
		{
			text.text = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy.Count.ToString();
		}
	}
}
