using MyCustom;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class CharacterModel : CustomMonoBehaviour
	{
		public bool IsAlive;

		public bool IsSpecialState;

		public bool IsInvisible;

		public BoxCollider2D boxCollider;

		[SerializeField]
		private BuffsHolder buffsHolder;

		public EntityStateEnum curState;

		public BuffsHolder BuffsHolder
		{
			get
			{
				return buffsHolder;
			}
			private set
			{
				buffsHolder = value;
			}
		}

		public virtual void ProcessDamage(DamageType damageType, CommonAttackDamage commonAttackDamage)
		{
		}

		public virtual void ChangeHealth(int damagePhysics, int damageMagic, int criticalStrikeChance = 0)
		{
		}

		public virtual void RestoreHealth()
		{
		}

		public virtual void IncreaseHealth(int hpAmount)
		{
		}

		public virtual int GetCurHp()
		{
			return 0;
		}

		public virtual int GetMaxHp()
		{
			return 1;
		}

		public virtual void Dead()
		{
		}

		public virtual void ReturnPool(float delayTime)
		{
		}

		public virtual void Update()
		{
			if (IsAlive && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				GetFSMController().OnUpdate(Time.deltaTime);
				curState = GetFSMController().GetCurrentState().entityStateEnum;
			}
		}

		public void LookAtEnemy()
		{
			Vector3 position = base.transform.position;
			Vector3 position2 = GetCurrentTarget().transform.position;
			if (position2.x - position.x > 0f)
			{
				base.transform.localScale = Vector3.one;
			}
			else
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
		}

		public bool IsInMeleeRange(EnemyModel enemy)
		{
			if (enemy == null)
			{
				return false;
			}
			if (enemy.IsAir)
			{
				return false;
			}
			float num = GetMeleeRange() * GetMeleeRange();
			return SingletonMonoBehaviour<GameData>.Instance.SqrDistance(GetAssignedPosition(), enemy.transform.position) <= num;
		}

		public bool IsInRangerRange(EnemyModel enemy)
		{
			if (enemy == null)
			{
				return false;
			}
			if (!IsRanger())
			{
				return false;
			}
			if (CanAttackAirEnemy() && enemy.IsUnderground)
			{
				return false;
			}
			float num = GetRangerRange() * GetRangerRange();
			return SingletonMonoBehaviour<GameData>.Instance.SqrDistance(GetAssignedPosition(), enemy.transform.position) <= num;
		}

		public bool IsInMeleeActionRange(EnemyModel enemy)
		{
			if (enemy == null)
			{
				return false;
			}
			float attackRangeMin = GetAttackRangeMin();
			Enemy originalParameter = enemy.OriginalParameter;
			float num = attackRangeMin + (float)originalParameter.body_size / GameData.PIXEL_PER_UNIT;
			float num2 = num * num * 1.1f;
			return SingletonMonoBehaviour<GameData>.Instance.SqrDistance(base.gameObject, enemy.gameObject) <= num2;
		}

		public bool IsMeleeAttacking()
		{
			return GetFSMController().GetCurrentState() is NewHeroMeleeAtkState;
		}

		public virtual void AddTarget(EnemyModel enemy)
		{
		}

		public virtual EnemyModel GetCurrentTarget()
		{
			return null;
		}

		public virtual bool CanAttackAirEnemy()
		{
			return false;
		}

		public virtual float GetRangerRange()
		{
			return 1f;
		}

		public virtual float GetMeleeRange()
		{
			return 1f;
		}

		public virtual float GetAttackRangeMin()
		{
			return 0f;
		}

		public virtual int GetCriticalStrikeChance()
		{
			return 0;
		}

		public virtual int GetDodgeChance()
		{
			return 0;
		}

		public virtual int GetIgnoreArmorChance()
		{
			return 0;
		}

		public virtual float GetSpeed()
		{
			return 0f;
		}

		public virtual IAnimationController GetAnimationController()
		{
			return null;
		}

		public virtual void DoRangeAttack()
		{
		}

		public virtual void DoMeleeAttack()
		{
		}

		public virtual float GetAtkCooldownDuration()
		{
			return 1f;
		}

		public virtual float GetShortIdleDuration()
		{
			return 2f;
		}

		public virtual Vector3 GetAssignedPosition()
		{
			return Vector3.zero;
		}

		public virtual void SetAssignedPosition(Vector3 assignedPos)
		{
		}

		public virtual float GetDieDuration()
		{
			return 1f;
		}

		public virtual void SetSpecialStateDuration(float duration)
		{
		}

		public virtual float GetSpecialStateDuration()
		{
			return 1f;
		}

		public virtual void SetSpecialStateAnimationName(string animationName)
		{
		}

		public virtual string GetSpecialStateAnimationName()
		{
			return string.Empty;
		}

		public virtual EntityFSMController GetFSMController()
		{
			return null;
		}

		public virtual bool IsRanger()
		{
			return false;
		}

		public virtual float GetCharacterHeight()
		{
			if (boxCollider == null)
			{
				boxCollider = GetComponent<BoxCollider2D>();
			}
			if (boxCollider != null)
			{
				Vector2 size = boxCollider.size;
				return size.y;
			}
			return 1f;
		}
	}
}
