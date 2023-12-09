using MyCustom;

namespace Gameplay
{
	public abstract class AllyController : CustomMonoBehaviour
	{
		private AllyModel allyModel;

		public AllyModel AllyModel
		{
			get
			{
				return allyModel;
			}
			set
			{
				allyModel = value;
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
	}
}
