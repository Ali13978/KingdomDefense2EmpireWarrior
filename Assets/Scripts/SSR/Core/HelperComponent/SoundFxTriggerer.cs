using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[DisallowMultipleComponent]
	public class SoundFxTriggerer : MonoBehaviour
	{
		private const int BrokenEffectInstanceId = -1;

		[Header("SoundFxTriggerer")]
		[SerializeField]
		private AudioClip audioClip;

		[SerializeField]
		[Range(0f, 1f)]
		private float volume = 1f;

		[SerializeField]
		private float interval;

		private static Dictionary<int, float> clipLastPlayedTime = new Dictionary<int, float>();

		private int effectInstanceId = -1;

		public void PlayFxSound()
		{
			effectInstanceId = PlaySound(audioClip);
		}

		public void TryStopFxSound()
		{
			SoundFxManager.Instance.TryStopEffect(effectInstanceId);
		}

		protected int PlaySound(AudioClip audioClip)
		{
			int instanceID = audioClip.GetInstanceID();
			bool flag = true;
			if (clipLastPlayedTime.TryGetValue(instanceID, out float value))
			{
				flag = (Time.realtimeSinceStartup - value >= interval);
			}
			if (!flag)
			{
				return -1;
			}
			clipLastPlayedTime[audioClip.GetInstanceID()] = Time.realtimeSinceStartup;
			return SoundFxManager.Instance.PlayEffect(audioClip, volume);
		}
	}
}
