using Middle;
using MyCustom;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Tournament
{
	public class BattleTimeCounter : MonoBehaviour
	{
		[SerializeField]
		private Text timeValue;

		private void Update()
		{
			if (ModeManager.Instance.gameMode == GameMode.TournamentMode && SingletonMonoBehaviour<GameData>.Instance.IsGameStart && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver && !SingletonMonoBehaviour<GameData>.Instance.IsPause)
			{
				UpdateTime();
			}
		}

		public void UpdateTime()
		{
			timeValue.text = StaticMethod.GetFormatedTimeSpan(StaticMethod.getTimeSpanFromSecond(SingletonMonoBehaviour<GameData>.Instance.tournamentBattleTime));
		}
	}
}
