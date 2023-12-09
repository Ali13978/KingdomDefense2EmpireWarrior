using Middle;
using UnityEngine;

namespace Gameplay
{
	public class FXSoundController : MonoBehaviour
	{
		private AudioSource audioSource;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			UpdateVolume();
		}

		private void Update()
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			if (Config.Instance.Sound)
			{
				audioSource.volume = SingletonMonoBehaviour<UnitSoundManager>.Instance.ReadDataVolumeAdjust.GetVolume_GameplayEffect();
			}
			else
			{
				audioSource.volume = 0f;
			}
		}
	}
}
