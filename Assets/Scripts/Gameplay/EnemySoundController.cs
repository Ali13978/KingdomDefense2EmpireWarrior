using Middle;
using UnityEngine;

namespace Gameplay
{
	public class EnemySoundController : MonoBehaviour
	{
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip attack;

		[SerializeField]
		private AudioClip dead;

		[SerializeField]
		private int playDeadSoundRate = 30;

		[SerializeField]
		private bool haveVibrateScreenOnAttack;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.ignoreListenerPause = true;
		}

		private void Update()
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			audioSource.volume = SingletonMonoBehaviour<UnitSoundManager>.Instance.ReadDataVolumeAdjust.GetVolume_GameplayEffect();
		}

		public void PlayAttack()
		{
			if (Config.Instance.Sound && (bool)attack)
			{
				audioSource.clip = attack;
				audioSource.Play();
			}
			if (haveVibrateScreenOnAttack)
			{
				SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
			}
		}

		public void PlayDead()
		{
			if (Config.Instance.Sound && (bool)dead && UnityEngine.Random.Range(0, 100) < playDeadSoundRate)
			{
				audioSource.clip = dead;
				audioSource.Play();
			}
		}
	}
}
