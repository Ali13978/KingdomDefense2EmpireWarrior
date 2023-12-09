using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class RandomSoundFxTriggerer : SoundFxTriggerer
	{
		[Header("RandomSoundFxTriggerer")]
		[SerializeField]
		private AudioClip[] additionalAudioClips = new AudioClip[0];

		public void TriggerRandom()
		{
			int num = Random.Range(-1, additionalAudioClips.Length);
			if (num == -1)
			{
				PlayFxSound();
			}
			else
			{
				PlaySound(additionalAudioClips[num]);
			}
		}
	}
}
