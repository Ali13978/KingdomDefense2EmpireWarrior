using Gameplay;
using WorldMap;

namespace MapLevel
{
	public class AskToBuyHeroPopupController : GameplayPopupController
	{
		private int currentHeroIDSelected;

		public void GoToHeroCamp()
		{
			SingletonMonoBehaviour<WorldMap.UIRootController>.Instance.heroCampPopupController.Init();
			SingletonMonoBehaviour<WorldMap.UIRootController>.Instance.heroCampPopupController.ChooseDefaultHero(currentHeroIDSelected);
			CloseWithScaleAnimation();
		}

		public void Init(int heroID)
		{
			currentHeroIDSelected = heroID;
			OpenWithScaleAnimation();
		}
	}
}
