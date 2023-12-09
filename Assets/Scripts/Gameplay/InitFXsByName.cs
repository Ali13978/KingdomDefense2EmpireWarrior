using UnityEngine;

namespace Gameplay
{
	public class InitFXsByName : MonoBehaviour
	{
		[SerializeField]
		private string[] effectName;

		private void Awake()
		{
			string[] array = effectName;
			foreach (string text in array)
			{
				SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(text);
			}
		}
	}
}
