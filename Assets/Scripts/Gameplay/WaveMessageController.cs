using Middle;
using MyCustom;
using SSR.Core.Architecture;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class WaveMessageController : CustomMonoBehaviour
	{
		[SerializeField]
		private OrderedEventDispatcher onUpdateWaveMessage;

		[SerializeField]
		private Text waveMessage;

		private int currentWave;

		private int totalWave;

		private void Start()
		{
			SetWaveMessage();
		}

		public void SetWaveMessage()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
				totalWave = SingletonMonoBehaviour<GameData>.Instance.TotalWave;
				waveMessage.text = $"{currentWave}/{totalWave}";
				break;
			case GameMode.DailyTrialMode:
				currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
				totalWave = SingletonMonoBehaviour<GameData>.Instance.TotalWave;
				waveMessage.text = $"{currentWave}/{totalWave}";
				break;
			case GameMode.TournamentMode:
				currentWave = GameplayManager.Instance.endlessModeManager.TotalWavesPassed;
				waveMessage.text = currentWave.ToString();
				break;
			}
			onUpdateWaveMessage.Dispatch();
		}
	}
}
