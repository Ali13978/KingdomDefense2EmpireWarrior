using Data;
using UnityEngine;

namespace MapLevel
{
	public class HeroesInputGroupController : MonoBehaviour
	{
		[SerializeField]
		private HeroesSelectedGroupController heroesSelectedController;

		[SerializeField]
		private HeroesSelectGroupController heroesSelectController;

		[SerializeField]
		private AskToBuyHeroPopupController askToBuyHeroPopupController;

		public HeroesSelectedGroupController HeroesSelectedController
		{
			get
			{
				return heroesSelectedController;
			}
			set
			{
				heroesSelectedController = value;
			}
		}

		public HeroesSelectGroupController HeroesSelectController
		{
			get
			{
				return heroesSelectController;
			}
			set
			{
				heroesSelectController = value;
			}
		}

		public AskToBuyHeroPopupController AskToBuyHeroPopupController
		{
			get
			{
				return askToBuyHeroPopupController;
			}
			set
			{
				askToBuyHeroPopupController = value;
			}
		}

		private void Start()
		{
			ReadWriteDataHero.Instance.OnBuyNewHero += Instance_OnBuyNewHero;
		}

		private void OnDestroy()
		{
			ReadWriteDataHero.Instance.OnBuyNewHero -= Instance_OnBuyNewHero;
		}

		private void Instance_OnBuyNewHero()
		{
			HeroesSelectController.UpdateButtonsStatus();
		}
	}
}
