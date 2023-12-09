using DG.Tweening;
using GeneralVariable;
using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class Tower0Ultimate0Bullet : CustomMonoBehaviour
	{
		[Header("Effect hit enemy")]
		[SerializeField]
		private string explosionFXName;

		[SerializeField]
		private float explosionFXDuration;

		[Space]
		[Header("Effect on died enemy")]
		[SerializeField]
		private string effectName;

		[SerializeField]
		private float effectDuration;

		private int damage;

		private Vector3 target;

		private SpriteRenderer spriteRenderer;

		private bool isReady;

		private void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Update()
		{
			if (isReady)
			{
				spriteRenderer.enabled = true;
				base.transform.right = target - base.transform.position;
			}
		}

		public void Init(int _damage, float _lifeTime, Vector2 targerPosition)
		{
			damage = _damage;
			target = targerPosition;
			spriteRenderer.enabled = false;
			isReady = true;
			base.transform.DOKill();
			base.transform.DOMove(targerPosition, _lifeTime).SetEase(Ease.Linear).OnComplete(OnMoveComplete);
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.tag == GeneralVariable.GeneralVariable.ENEMY_TAG)
			{
				EnemyModel component = coll.gameObject.GetComponent<EnemyModel>();
				if (!component.IsUnderground)
				{
					CommonAttackDamage commonAttackDamage = new CommonAttackDamage(damage, 0);
					component.ProcessDamage(DamageType.Range, commonAttackDamage);
					EffectController effectController = SpawnExplosion();
					effectController.Init(explosionFXDuration, target);
				}
			}
		}

		private void OnMoveComplete()
		{
			CastEffectOnDiedEnemy();
			ReturnPool();
		}

		public void CastEffectOnDiedEnemy()
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(effectName);
			effect.transform.position = base.gameObject.transform.position;
			effect.Init(effectDuration);
		}

		private EffectController SpawnExplosion()
		{
			EffectController effectController = null;
			return SingletonMonoBehaviour<SpawnFX>.Instance.GetExplosion(explosionFXName);
		}

		private void ReturnPool()
		{
			isReady = true;
			base.transform.DOKill();
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}
