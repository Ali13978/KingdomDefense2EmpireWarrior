using Middle;
using UnityEngine;

namespace Gameplay
{
	public class UnitSoundController : MonoBehaviour
	{
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip[] startMove;

		[SerializeField]
		private AudioClip[] select;

		[SerializeField]
		private AudioClip die;

		[SerializeField]
		private AudioClip respawn;

		[SerializeField]
		private AudioClip[] attack;

		[SerializeField]
		private AudioClip specialAttack;

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

		public void PlaySelect()
		{
			if (Config.Instance.Sound && select != null)
			{
				int num = UnityEngine.Random.Range(0, select.Length);
				audioSource.clip = select[num];
				audioSource.Play();
			}
		}

		public void PlayStartMove()
		{
			if (Config.Instance.Sound && startMove != null)
			{
				int num = UnityEngine.Random.Range(0, startMove.Length);
				audioSource.clip = startMove[num];
				audioSource.Play();
			}
		}

		public void PlayDie()
		{
			if (Config.Instance.Sound && (bool)die)
			{
				audioSource.clip = die;
				audioSource.Play();
			}
		}

		public void PlayRespawn()
		{
			if (Config.Instance.Sound && (bool)respawn)
			{
				audioSource.clip = respawn;
				audioSource.Play();
			}
		}

		public void PlayAttack(int index)
		{
			if (Config.Instance.Sound && (bool)attack[index])
			{
				audioSource.clip = attack[index];
				audioSource.Play();
			}
		}

		public void PlayRandomAttack()
		{
			if (Config.Instance.Sound && attack != null)
			{
				int num = UnityEngine.Random.Range(0, attack.Length - 1);
				audioSource.clip = attack[num];
				audioSource.Play();
			}
		}

		public void PlaySpecialAttack()
		{
			if (Config.Instance.Sound && (bool)specialAttack)
			{
				audioSource.clip = specialAttack;
				audioSource.Play();
			}
		}
	}
}
