using Middle;
using UnityEngine;

namespace Gameplay
{
	public class TowerControlSoundController : SingletonMonoBehaviour<TowerControlSoundController>
	{
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip[] buildTowers;

		[SerializeField]
		private AudioClip[] upgradeNormals;

		[SerializeField]
		private AudioClip upgradeUltimate;

		[SerializeField]
		private AudioClip sell;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		private void Update()
		{
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			audioSource.volume = SingletonMonoBehaviour<UnitSoundManager>.Instance.ReadDataVolumeAdjust.GetVolume_GameplayEffect();
		}

		public void PlayBuild(int towerID)
		{
			if (Config.Instance.Sound && buildTowers[towerID] != null)
			{
				audioSource.clip = buildTowers[towerID];
				audioSource.Play();
			}
		}

		public void PlaySell()
		{
			if (Config.Instance.Sound && (bool)sell)
			{
				audioSource.clip = sell;
				audioSource.Play();
			}
		}

		public void PlayUpgradeNormal(int towerID)
		{
			if (Config.Instance.Sound && upgradeNormals != null)
			{
				audioSource.clip = upgradeNormals[towerID];
				audioSource.Play();
			}
		}

		public void PlayUpgradeUltimate()
		{
			if (Config.Instance.Sound && upgradeUltimate != null)
			{
				audioSource.clip = upgradeUltimate;
				audioSource.Play();
			}
		}
	}
}
