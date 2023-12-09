using MyCustom;

namespace Gameplay
{
	public class HeroController : CustomMonoBehaviour
	{
		private HeroModel heroModel;

		public HeroModel HeroModel
		{
			get
			{
				return heroModel;
			}
			set
			{
				heroModel = value;
			}
		}

		public virtual void Initialize()
		{
		}

		public virtual void OnAppear()
		{
		}

		public virtual void OnDead()
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
