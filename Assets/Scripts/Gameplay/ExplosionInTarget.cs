using UnityEngine;

namespace Gameplay
{
	public class ExplosionInTarget : MonoBehaviour
	{
		[SerializeField]
		private float explosionDuration;

		[SerializeField]
		private string explosionFXName;

		public void CastExplosion(Transform targetTransform)
		{
			if (targetTransform != null && targetTransform.gameObject.activeSelf)
			{
				EffectController explosion = SingletonMonoBehaviour<SpawnFX>.Instance.GetExplosion(explosionFXName);
				explosion.Init(explosionDuration, targetTransform);
			}
		}
	}
}
