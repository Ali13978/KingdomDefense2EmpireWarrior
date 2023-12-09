using Middle;
using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundTriggerer : MonoBehaviour
	{
		[Header("SoundTriggerer")]
		[SerializeField]
		[HideInInspector]
		private AudioSource audioSource;

		[SerializeField]
		private bool stopOnDisabled = true;

		[SerializeField]
		private OrderedEventDispatcher onStop = new OrderedEventDispatcher();

		private bool playing;

		public void TriggerSound()
		{
			if (Config.Instance.Sound)
			{
				audioSource.Play();
			}
		}

		public void OnEnable()
		{
			playing = audioSource.isPlaying;
		}

		public void OnDisable()
		{
			if (stopOnDisabled)
			{
				audioSource.Stop();
			}
		}

		public void Update()
		{
			if (playing && !audioSource.isPlaying)
			{
				onStop.Dispatch();
			}
			playing = audioSource.isPlaying;
		}

		public void Reset()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
		}
	}
}
