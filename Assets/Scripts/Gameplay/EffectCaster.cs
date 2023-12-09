using UnityEngine;

namespace Gameplay
{
	public class EffectCaster : MonoBehaviour
	{
		public void CastEffect(string effectName, float duration, Vector2 targetPosition)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(effectName);
			effect.transform.position = targetPosition;
			effect.Init(duration);
		}
	}
}
