using UnityEngine;

namespace Gameplay
{
	public class Dragon10Skill0Projectile
	{
		public Vector3 startPos;

		public Vector3 endPos;

		public Vector3 midAnchor;

		public float duration;

		private float countdown;

		public GameObject projectile;

		private GameObject explodePrefab;

		public Dragon10Skill0Projectile(GameObject projPrefab, GameObject explodePrefab, Vector3 startPos, Vector3 endPos, float detourDis, float duration)
		{
			this.startPos = startPos;
			this.endPos = endPos;
			this.explodePrefab = explodePrefab;
			Vector3 a = endPos - startPos;
			Vector3 normalized = new Vector3(0f - a.y, a.x, 0f).normalized;
			midAnchor = startPos + a * Random.Range(0.2f, 0.55f) + normalized * Random.Range(0f - detourDis, detourDis);
			projectile = ObjectPool.Spawn(projPrefab, startPos);
			this.duration = duration;
			countdown = duration;
		}

		public bool OnUpdate(float dt)
		{
			countdown -= dt;
			float d = (duration - countdown) / duration;
			Vector3 position = startPos + (endPos - startPos) * d;
			if (countdown <= 0f)
			{
				SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
				projectile.Recycle();
				ObjectPool.Spawn(explodePrefab, position);
				return false;
			}
			projectile.transform.position = position;
			return true;
		}
	}
}
