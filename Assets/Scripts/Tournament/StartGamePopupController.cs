using Data;
using Gameplay;
using MapLevel;
using Middle;
using Parameter;
using UnityEngine;

namespace Tournament
{
	public class StartGamePopupController : GameplayPopupController
	{
		[Space]
		[Header("Controllers")]
		[SerializeField]
		private HeroesInputGroupController heroesInputGroupController;

		[SerializeField]
		private PowerUpItemGroupController powerUpItemGroupController;

		public void Init()
		{
			MiddleDelivery.Instance.MapIDSelected = MapRuleParameter.Instance.GetCurrentSeasonMapID();
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			heroesInputGroupController.HeroesSelectedController.InitHeroesSlot();
			heroesInputGroupController.HeroesSelectController.InitListHeroToSelect();
			powerUpItemGroupController.RefreshPowerupItems();
		}

		public void StartGame()
		{
		}

		public void InitListHeroesIDSelected()
		{
			int num = 3;
			MiddleDelivery.Instance.ClearListHeroID();
			for (int i = 0; i < num; i++)
			{
				if (heroesInputGroupController.HeroesSelectedController.listHeroesIDSelected[i] >= 0)
				{
					MiddleDelivery.Instance.AddHeroIDToList(heroesInputGroupController.HeroesSelectedController.listHeroesIDSelected[i]);
				}
			}
		}

		public void SaveListHeroIDSelected()
		{
			ReadWriteDataHeroPrepare.Instance.Save(heroesInputGroupController.HeroesSelectedController.listHeroesIDSelected);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			SaveListHeroIDSelected();
			if (heroesInputGroupController.AskToBuyHeroPopupController.isOpen)
			{
				heroesInputGroupController.AskToBuyHeroPopupController.CloseWithScaleAnimation();
			}
		}
	}
}
