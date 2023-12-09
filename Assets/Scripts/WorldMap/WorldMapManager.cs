using Data;
using GiftcodeSystem;
using Services.PlatformSpecific;
using Tutorial;
using UnityEngine;

namespace WorldMap
{
	public class WorldMapManager : SingletonMonoBehaviour<WorldMapManager>
	{
		[SerializeField]
		private WorldMapTutorial worldMapTutorial;

		[SerializeField]
		private GiftCodeManager giftCodeManager;

		public WorldMapTutorial WorldMapTutorial
		{
			get
			{
				return worldMapTutorial;
			}
			set
			{
				worldMapTutorial = value;
			}
		}

		public GiftCodeManager GiftCodeManager
		{
			get
			{
				return giftCodeManager;
			}
			set
			{
				giftCodeManager = value;
			}
		}

		private void Start()
		{
			SendEventOpenScene();
			DailyCheckinManager.Instance.WorldMapCheckIn(worldMapTutorial);
		}

		private void SendEventOpenScene()
		{
			int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			int currentStar = ReadWriteDataPlayerCurrency.Instance.GetCurrentStar();
			int maxMapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked() + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_OpenSceneWorldMap(currentGem, currentStar, maxMapIDUnlocked);
		}
	}
}
