using Data;
using DG.Tweening;
using Gameplay;
using Parameter;
using System.Collections;
using UnityEngine;

namespace DailyTrial
{
	public class RewardGroupController : MonoBehaviour
	{
		[SerializeField]
		private GameObject continueButton;

		[SerializeField]
		private RectTransform rankBar;

		[SerializeField]
		private float minValue;

		[SerializeField]
		private float maxValue;

		private int[] waveRanks;

		private int[] remants;

		private bool[] isAbleToTakeReward;

		[SerializeField]
		private RewardController[] listRewardController;

		private int currentWavePassed;

		private float unit;

		private Tweener tween;

		public void Init(BattleStatus battleStatus)
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			int[] listWaveRank = DailyTrialParameter.Instance.GetListWaveRank(currentDayIndex);
			waveRanks = new int[3]
			{
				listWaveRank[0],
				listWaveRank[1],
				listWaveRank[2]
			};
			unit = maxValue / (float)SingletonMonoBehaviour<GameData>.Instance.TotalWave;
			RectTransform rectTransform = rankBar;
			Vector2 sizeDelta = rankBar.sizeDelta;
			rectTransform.sizeDelta = new Vector2(0f, sizeDelta.y);
			switch (battleStatus)
			{
			case BattleStatus.Victory:
				UpdateRankBarResult(SingletonMonoBehaviour<GameData>.Instance.CurrentWave);
				break;
			case BattleStatus.Defeat:
				UpdateRankBarResult(SingletonMonoBehaviour<GameData>.Instance.CurrentWave - 1);
				break;
			}
		}

		public void UpdateRankBar_Victory()
		{
			float valueX = 0f;
			tween = DOTween.To(() => 0f, delegate(float x)
			{
				valueX = x;
				RectTransform rectTransform = rankBar;
				float x2 = valueX;
				Vector2 sizeDelta = rankBar.sizeDelta;
				rectTransform.sizeDelta = new Vector2(x2, sizeDelta.y);
			}, unit * (float)SingletonMonoBehaviour<GameData>.Instance.CurrentWave, 2f).SetEase(Ease.Linear).OnComplete(OnUpdateRankBarComplete_Victory);
		}

		private void OnUpdateRankBarComplete_Victory()
		{
			continueButton.SetActive(value: true);
			for (int i = 0; i < waveRanks.Length; i++)
			{
				if (SingletonMonoBehaviour<GameData>.Instance.CurrentWave >= waveRanks[i])
				{
					listRewardController[i].InitRewardInfor();
				}
			}
		}

		public void UpdateRankBar_Defeat()
		{
			float valueX = 0f;
			tween = DOTween.To(() => 0f, delegate(float x)
			{
				valueX = x;
				RectTransform rectTransform = rankBar;
				float x2 = valueX;
				Vector2 sizeDelta = rankBar.sizeDelta;
				rectTransform.sizeDelta = new Vector2(x2, sizeDelta.y);
			}, unit * (float)(SingletonMonoBehaviour<GameData>.Instance.CurrentWave - 1), 2f).SetEase(Ease.Linear).OnComplete(OnUpdateRankBarComplete_Defeat);
		}

		private void OnUpdateRankBarComplete_Defeat()
		{
			continueButton.SetActive(value: true);
			for (int i = 0; i < waveRanks.Length; i++)
			{
				if (SingletonMonoBehaviour<GameData>.Instance.CurrentWave >= waveRanks[i] + 1)
				{
					listRewardController[i].InitRewardInfor();
				}
			}
		}

		private void UpdateRankBarResult(int wavePassed)
		{
			currentWavePassed = wavePassed;
			isAbleToTakeReward = new bool[3];
			remants = new int[3];
			for (int i = 0; i < waveRanks.Length; i++)
			{
				if (wavePassed >= waveRanks[i])
				{
					isAbleToTakeReward[i] = true;
				}
			}
			if (wavePassed >= 0 && wavePassed < 3)
			{
				remants[0] = wavePassed;
				remants[1] = -1;
				remants[2] = -1;
			}
			if (wavePassed >= 3 && wavePassed < 6)
			{
				remants[0] = 3;
				remants[1] = wavePassed - 3;
				remants[2] = -1;
			}
			if (wavePassed >= 6 && wavePassed <= 9)
			{
				remants[0] = 3;
				remants[1] = 3;
				remants[2] = wavePassed - 6;
			}
			StartCoroutine(ProcessBarRank0());
		}

		private IEnumerator ProcessBarRank0()
		{
			yield return null;
			if (remants[0] > 0)
			{
				float valueX = 0f;
				tween = DOTween.To(() => 0f, delegate(float x)
				{
					valueX = x;
					RectTransform rectTransform = rankBar;
					float x2 = valueX;
					Vector2 sizeDelta = rankBar.sizeDelta;
					rectTransform.sizeDelta = new Vector2(x2, sizeDelta.y);
				}, unit * (float)remants[0], 1f).SetEase(Ease.Linear).OnComplete(ProcessRewardRank0);
			}
		}

		private void ProcessRewardRank0()
		{
			if (isAbleToTakeReward[0])
			{
				listRewardController[0].InitRewardInfor();
			}
			if (remants[1] <= 0)
			{
				continueButton.SetActive(value: true);
			}
			StartCoroutine(ProcessBarRank1());
		}

		private IEnumerator ProcessBarRank1()
		{
			yield return new WaitForSeconds(0.3f);
			if (remants[1] > 0)
			{
				Vector2 sizeDelta = rankBar.sizeDelta;
				float valueX = sizeDelta.x;
				tween = DOTween.To(delegate
				{
					Vector2 sizeDelta3 = rankBar.sizeDelta;
					return sizeDelta3.x;
				}, delegate(float x)
				{
					valueX = x;
					RectTransform rectTransform = rankBar;
					float x2 = valueX;
					Vector2 sizeDelta2 = rankBar.sizeDelta;
					rectTransform.sizeDelta = new Vector2(x2, sizeDelta2.y);
				}, unit * (float)(remants[0] + remants[1]), 0.75f).SetEase(Ease.Linear).OnComplete(ProcessRewardRank1);
			}
		}

		private void ProcessRewardRank1()
		{
			if (isAbleToTakeReward[1])
			{
				listRewardController[1].InitRewardInfor();
			}
			if (remants[2] <= 0)
			{
				continueButton.SetActive(value: true);
			}
			StartCoroutine(ProcessBarRank2());
		}

		private IEnumerator ProcessBarRank2()
		{
			yield return new WaitForSeconds(0.3f);
			if (remants[2] > 0)
			{
				Vector2 sizeDelta = rankBar.sizeDelta;
				float valueX = sizeDelta.x;
				tween = DOTween.To(delegate
				{
					Vector2 sizeDelta3 = rankBar.sizeDelta;
					return sizeDelta3.x;
				}, delegate(float x)
				{
					valueX = x;
					RectTransform rectTransform = rankBar;
					float x2 = valueX;
					Vector2 sizeDelta2 = rankBar.sizeDelta;
					rectTransform.sizeDelta = new Vector2(x2, sizeDelta2.y);
				}, unit * (float)(remants[0] + remants[1] + remants[2]), 0.5f).SetEase(Ease.Linear).OnComplete(ProcessRewardRank2);
			}
		}

		private void ProcessRewardRank2()
		{
			if (isAbleToTakeReward[2])
			{
				listRewardController[2].InitRewardInfor();
			}
			continueButton.SetActive(value: true);
		}
	}
}
