using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(AudioSource))]
	public class RandomAudioSource : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private AudioSource audioSource;

		[SerializeField]
		private List<AudioClip> audioClips = new List<AudioClip>();

		public bool isPlaying => audioSource.isPlaying;

		public void Play()
		{
			if (audioClips.Count != 0)
			{
				int index = UnityEngine.Random.Range(0, audioClips.Count);
				audioSource.PlayOneShot(audioClips[index]);
			}
		}

		public void Reset()
		{
			audioSource = GetComponent<AudioSource>();
			if (audioSource.clip != null)
			{
				audioClips.Add(audioSource.clip);
			}
		}
	}
}
