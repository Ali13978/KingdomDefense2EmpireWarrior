using Data;
using Middle;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace MapLevel
{
	public class StartButtonController : MonoBehaviour
	{
		[SerializeField]
		private MapLevelSelectPopupController mapLevelSelectPopupController;

		[SerializeField]
		private HeroesInputGroupController heroesInputGroupController;

		[Space]
		[Header("Image material")]
		[SerializeField]
		private Material material;

		private Button startButton;

		[Space]
		[Header("Text notification")]
		[SerializeField]
		private Text textNotification;

		[SerializeField]
		private int readyContentID;

		[SerializeField]
		private int notReadyContentID;

		[SerializeField]
		private Color readyColor;

		[SerializeField]
		private Color notReadyColor;

		private void Awake()
		{
			startButton = GetComponent<Button>();
		}

		private void Update()
		{
			if (heroesInputGroupController.HeroesSelectedController.IsChooseAtLeastOneHero())
			{
				SetStartButtonEnable();
			}
			else
			{
				SetStartButtonDisable();
			}
		}

		private void SetStartButtonEnable()
		{
			startButton.enabled = true;
			material.SetFloat("_EffectAmount", 0f);
			textNotification.text = Singleton<NotificationDescription>.Instance.GetNotiContent(readyContentID);
			textNotification.color = readyColor;
		}

		private void SetStartButtonDisable()
		{
			startButton.enabled = false;
			material.SetFloat("_EffectAmount", 1f);
			textNotification.text = Singleton<NotificationDescription>.Instance.GetNotiContent(notReadyContentID);
			textNotification.color = notReadyColor;
		}

		private void DoLoadSceneGameplay()
		{
			ReadWriteDataMap.Instance.SaveLastMapPlayed(mapLevelSelectPopupController.CurrentMapID);
			GameApplication.Instance.LoadScene(GameApplication.GameplaySceneName);
		}

		public void OnClick()
		{
			Invoke("OnPrepareToLoad", 0.1f);
			UISoundManager.Instance.PlayStartGameAtMapLevel();
		}

		private void OnPrepareToLoad()
		{
			Loading.Instance.ShowLoading();
			Invoke("DoLoadSceneGameplay", 0.3f);
			ModeManager.Instance.gameMode = GameMode.CampaignMode;
			mapLevelSelectPopupController.OnStartGame();
			mapLevelSelectPopupController.InitListHeroesIDSelected();
			mapLevelSelectPopupController.SaveListHeroIDSelected();
			SendEvent_StartGame();
		}

		private void SendEvent_StartGame()
		{
			int mapID = mapLevelSelectPopupController.CurrentMapID + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_StartGame_MapLevel(mapID);
		}
	}
}
