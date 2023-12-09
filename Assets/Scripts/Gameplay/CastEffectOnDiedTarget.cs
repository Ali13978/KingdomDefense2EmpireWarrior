using UnityEngine;

namespace Gameplay
{
	public class CastEffectOnDiedTarget : MonoBehaviour
	{
		[Header("Effect on died target")]
		[SerializeField]
		private string effectName;

		[SerializeField]
		private float effectDuration;

		[SerializeField]
		private bool isLayerOverTarget;

		public void CastEffect(Transform targetTransform)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(effectName);
			effect.transform.position = targetTransform.position;
			effect.Init(effectDuration);
			if (isLayerOverTarget)
			{
				effect.SetLayerOverTarget(targetTransform);
			}
		}
	}
}
