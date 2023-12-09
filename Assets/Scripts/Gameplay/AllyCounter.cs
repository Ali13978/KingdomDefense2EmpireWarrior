using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class AllyCounter : MonoBehaviour
	{
		public Text text;

		private void Update()
		{
			text.text = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly.Count.ToString();
		}
	}
}
