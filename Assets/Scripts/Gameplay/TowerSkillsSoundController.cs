using Middle;
using UnityEngine;

namespace Gameplay
{
	public class TowerSkillsSoundController : TowerController
	{
		[SerializeField]
		private AudioClip[] castSkill;

		private AudioSource audioSource;

		private void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		public override void Update()
		{
			base.Update();
			UpdateVolume();
		}

		private void UpdateVolume()
		{
			audioSource.volume = SingletonMonoBehaviour<UnitSoundManager>.Instance.ReadDataVolumeAdjust.GetVolume_GameplayEffect();
		}

		public void PlayCastSkillSound(int index)
		{
			if (Config.Instance.Sound && castSkill[index] != null)
			{
				audioSource.clip = castSkill[index];
				audioSource.Play();
			}
		}
	}
}
