using MyCustom;

namespace Gameplay
{
	public abstract class EnemyController : CustomMonoBehaviour
	{
		private EnemyModel enemyModel;

		public EnemyModel EnemyModel
		{
			get
			{
				return enemyModel;
			}
			set
			{
				enemyModel = value;
			}
		}

		public virtual void Initialize()
		{
		}

		public virtual void OnAppear()
		{
		}

		public virtual void OnReturnPool()
		{
		}

		public virtual void Update()
		{
			if (!SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
			}
		}

		public bool IsCurrentSpeedGreaterThanMinSpeed()
		{
			return EnemyModel.EnemyMovementController.Speed > 0.05f;
		}

		public bool IsEnemyAlive()
		{
			return EnemyModel.IsAlive;
		}
	}
}
