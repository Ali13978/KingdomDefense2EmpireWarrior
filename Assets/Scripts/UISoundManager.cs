using Middle;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
	[Space]
	[Header("Audio Sources")]
	[SerializeField]
	private AudioSource audioSource_UI;

	[SerializeField]
	private AudioSource audioSource_Result;

	[SerializeField]
	private AudioSource audioSource_UIEffect;

	[Space]
	[SerializeField]
	private AudioSource audioSourcesGameplay_Layer0;

	[SerializeField]
	private AudioSource audioSourcesGameplay_Layer1;

	[Space]
	[SerializeField]
	private ReadDataVolumeAdjust readDataVolumeAdjust;

	[Space]
	[Header("Audio Clips")]
	[SerializeField]
	private UISound uiSound;

	[Space]
	[SerializeField]
	private UIResult uiResult;

	[Space]
	[SerializeField]
	private SfxGameplayNoti sfxGameplayNoti;

	[Space]
	[SerializeField]
	private UIEffect uiEffect;

	public static UISoundManager Instance
	{
		get;
		set;
	}

	private void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		audioSource_UI.ignoreListenerPause = true;
		audioSource_Result.ignoreListenerPause = true;
	}

	private void Start()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		UpdateVolume();
	}

	private void UpdateVolume()
	{
		audioSource_UI.volume = readDataVolumeAdjust.GetVolume_UI();
		audioSource_UIEffect.volume = readDataVolumeAdjust.GetVolume_UIEffect();
		audioSourcesGameplay_Layer0.volume = readDataVolumeAdjust.GetVolume_GameplayEffect();
		audioSourcesGameplay_Layer1.volume = readDataVolumeAdjust.GetVolume_GameplayEffect();
	}

	public void PlayCallEnemy()
	{
		if (Config.Instance.Sound && (bool)sfxGameplayNoti.callEnemy)
		{
			audioSourcesGameplay_Layer0.clip = sfxGameplayNoti.callEnemy;
			audioSourcesGameplay_Layer0.Play();
		}
	}

	public void PlayBeforeCallEnemy()
	{
		if (Config.Instance.Sound && (bool)sfxGameplayNoti.beforeCallEnemy)
		{
			audioSourcesGameplay_Layer1.clip = sfxGameplayNoti.beforeCallEnemy;
			audioSourcesGameplay_Layer1.Play();
		}
	}

	public void PlayNewEnemyButton()
	{
		if (Config.Instance.Sound && (bool)sfxGameplayNoti.newEnemyButton)
		{
			audioSourcesGameplay_Layer0.clip = sfxGameplayNoti.newEnemyButton;
			audioSourcesGameplay_Layer0.Play();
		}
	}

	public void PlayNewTipButton()
	{
		if (Config.Instance.Sound && (bool)sfxGameplayNoti.newTipsButton)
		{
			audioSourcesGameplay_Layer1.clip = sfxGameplayNoti.newTipsButton;
			audioSourcesGameplay_Layer1.Play();
		}
	}

	public void PlayClick()
	{
		if (Config.Instance.Sound && (bool)uiSound.click)
		{
			audioSource_UI.clip = uiSound.click;
			audioSource_UI.Play();
		}
	}

	public void PlayClosePopup()
	{
		if (Config.Instance.Sound && (bool)uiSound.close)
		{
			audioSource_UI.clip = uiSound.close;
			audioSource_UI.Play();
		}
	}

	public void PlayOpenPopup()
	{
		if (Config.Instance.Sound && (bool)uiSound.open)
		{
			audioSource_UI.clip = uiSound.open;
			audioSource_UI.Play();
		}
	}

	public void PlayCloseLoading()
	{
		if (Config.Instance.Sound && (bool)uiSound.loadingClose)
		{
			audioSource_UI.clip = uiSound.loadingClose;
			audioSource_UI.Play();
		}
	}

	public void PlayOpenLoading()
	{
		if (Config.Instance.Sound && (bool)uiSound.loadingOpen)
		{
			audioSource_UI.clip = uiSound.loadingOpen;
			audioSource_UI.Play();
		}
	}

	public void PlayVictory()
	{
		if (Config.Instance.Sound && (bool)uiResult.victory)
		{
			audioSource_Result.clip = uiResult.victory;
			audioSource_Result.Play();
		}
	}

	public void PlayDefeat()
	{
		if (Config.Instance.Sound && (bool)uiResult.defeat)
		{
			audioSource_Result.clip = uiResult.defeat;
			audioSource_Result.Play();
		}
	}

	public void PlayluckyChestSound()
	{
		if (Config.Instance.Sound && (bool)uiResult.openLuckyChest)
		{
			audioSource_Result.clip = uiResult.openLuckyChest;
			audioSource_Result.Play();
		}
	}

	public void PlayStartGameAtMainMenu()
	{
		if (Config.Instance.Sound && (bool)uiEffect.startGameAtMainMenu)
		{
			audioSource_UIEffect.clip = uiEffect.startGameAtMainMenu;
			audioSource_UIEffect.Play();
		}
	}

	public void PlayStartGameAtMapLevel()
	{
		if (Config.Instance.Sound && (bool)uiEffect.startGameAtMapLevel)
		{
			audioSource_UIEffect.clip = uiEffect.startGameAtMapLevel;
			audioSource_UIEffect.Play();
		}
	}

	public void PlayUpgradeSuccess()
	{
		if (Config.Instance.Sound && (bool)uiEffect.upgradeSuccess)
		{
			audioSource_UIEffect.clip = uiEffect.upgradeSuccess;
			audioSource_UIEffect.Play();
		}
	}

	public void PlayBuySuccess()
	{
		if (Config.Instance.Sound && (bool)uiEffect.buySuccess)
		{
			audioSource_UIEffect.clip = uiEffect.buySuccess;
			audioSource_UIEffect.Play();
		}
	}

	public void PlayUnlockSuccess()
	{
		if (Config.Instance.Sound && (bool)uiEffect.unlockSuccess)
		{
			audioSource_UIEffect.clip = uiEffect.unlockSuccess;
			audioSource_UIEffect.Play();
		}
	}
}
