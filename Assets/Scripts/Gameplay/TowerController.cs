using MyCustom;

namespace Gameplay
{
	public abstract class TowerController : CustomMonoBehaviour
	{
		private TowerModel towerModel;

		public TowerModel TowerModel
		{
			get
			{
				return towerModel;
			}
			set
			{
				towerModel = value;
			}
		}

		public virtual void Initialize()
		{
		}

		public virtual void OnAppear()
		{
		}

		public virtual void OnBuildFinished()
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
	}
}
