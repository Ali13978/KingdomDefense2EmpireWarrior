using MapLevel;
using Middle;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Tournament
{
	public class StartGameButtonController : MonoBehaviour
	{
		[SerializeField]
		private StartGamePopupController startGamePopupController;

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
			ModeManager.Instance.gameMode = GameMode.TournamentMode;
			startGamePopupController.InitListHeroesIDSelected();
			startGamePopupController.SaveListHeroIDSelected();
		}
	}
}
