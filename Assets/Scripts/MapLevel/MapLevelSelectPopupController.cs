using Data;
using Gameplay;
using Middle;
using Parameter;
using Services.PlatformSpecific;
using SSR.Core.Architecture;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class MapLevelSelectPopupController : GameplayPopupController
	{
		[Header("Events")]
		[SerializeField]
		private OrderedEventDispatcher OnInitPopupEvent;

		[SerializeField]
		private OrderedEventDispatcher OnChooseHeroEvent;

		[SerializeField]
		private OrderedEventDispatcher OnStartGameEvent;

		[Space]
		[Header("Star and level")]
		[SerializeField]
		private StarGroupController starGroupController;

		[SerializeField]
		private Text levelName;

		private int currentMapID;

		[Space]
		[Header("Controllers")]
		[SerializeField]
		private HeroesInputGroupController heroesInputGroupController;

		[SerializeField]
		private PowerUpItemGroupController powerUpItemGroupController;

		[SerializeField]
		private GameModeSelectGroupController gameModeSelectGroupController;

		public int CurrentMapID
		{
			get
			{
				return currentMapID;
			}
			set
			{
				currentMapID = value;
			}
		}

		public HeroesInputGroupController HeroesInputGroupController
		{
			get
			{
				return heroesInputGroupController;
			}
			set
			{
				heroesInputGroupController = value;
			}
		}

		public PowerUpItemGroupController PowerUpItemGroupController
		{
			get
			{
				return powerUpItemGroupController;
			}
			set
			{
				powerUpItemGroupController = value;
			}
		}

		public GameModeSelectGroupController GameModeSelectGroupController
		{
			get
			{
				return gameModeSelectGroupController;
			}
			set
			{
				gameModeSelectGroupController = value;
			}
		}

		public void Init(int mapID)
		{
			CurrentMapID = mapID;
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			SetLevelName();
			DisplayStarGroup();
			HeroesInputGroupController.HeroesSelectedController.InitHeroesSlot();
			HeroesInputGroupController.HeroesSelectController.InitListHeroToSelect();
			PowerUpItemGroupController.RefreshPowerupItems();
			GameModeSelectGroupController.InitDefault();
			OnInitPopup();
			SendEventOpenPopup();
		}

		private void SendEventOpenPopup()
		{
			int mapID = CurrentMapID + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_OpenMapLevelSelect(mapID);
		}

		private void OnInitPopup()
		{
			OnInitPopupEvent.Dispatch();
		}

		public void OnChooseHero()
		{
			OnChooseHeroEvent.Dispatch();
		}

		public void OnStartGame()
		{
			OnStartGameEvent.Dispatch();
		}

		private void SetLevelName()
		{
			levelName.text = (CurrentMapID + 1).ToString();
		}

		private void DisplayStarGroup()
		{
			starGroupController.DisplayStarGroup(ReadWriteDataMap.Instance.GetStarEarnedByMap(CurrentMapID));
		}

		public void InitListHeroesIDSelected()
		{
			int maxHeroAllowed = MapRuleParameter.Instance.GetMaxHeroAllowed(CurrentMapID);
			MiddleDelivery.Instance.ClearListHeroID();
			for (int i = 0; i < maxHeroAllowed; i++)
			{
				if (HeroesInputGroupController.HeroesSelectedController.listHeroesIDSelected[i] >= 0)
				{
					MiddleDelivery.Instance.AddHeroIDToList(HeroesInputGroupController.HeroesSelectedController.listHeroesIDSelected[i]);
				}
			}
		}

		public void SaveListHeroIDSelected()
		{
			ReadWriteDataHeroPrepare.Instance.Save(HeroesInputGroupController.HeroesSelectedController.listHeroesIDSelected);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			SaveListHeroIDSelected();
			if (HeroesInputGroupController.AskToBuyHeroPopupController.isOpen)
			{
				HeroesInputGroupController.AskToBuyHeroPopupController.CloseWithScaleAnimation();
			}
		}
	}
}
