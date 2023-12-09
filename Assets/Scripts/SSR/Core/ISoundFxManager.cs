using UnityEngine;

namespace SSR.Core
{
	public interface ISoundFxManager
	{
		int PlayEffect(AudioClip audioClip, float volumeScale);

		void TryStopEffect(int effectInstanceId);
	}
}
