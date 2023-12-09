using Data;
using DG.Tweening;
using Gameplay;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyTrial
{
	public class WaveRankBarController : MonoBehaviour
	{
		[SerializeField]
		private RectTransform rankBar;

		[SerializeField]
		private float minValue;

		[SerializeField]
		private float maxValue;

		private int[] waveRanks;

		[SerializeField]
		private Image[] chestBox;

		private float unit;

		private float currentValueX;

		private Tweener tween;

		private void Start()
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
			currentValueX = 0f;
			Image[] array = chestBox;
			foreach (Image image in array)
			{
				image.material.SetFloat("_EffectAmount", 1f);
			}
		}

		public void UpdateRankBar()
		{
			float valueX = currentValueX;
			tween = DOTween.To(() => currentValueX, delegate(float x)
			{
				valueX = x;
				RectTransform rectTransform = rankBar;
				float x2 = valueX;
				Vector2 sizeDelta = rankBar.sizeDelta;
				rectTransform.sizeDelta = new Vector2(x2, sizeDelta.y);
			}, unit * (float)SingletonMonoBehaviour<GameData>.Instance.CurrentWave, 0.2f).SetEase(Ease.Linear).OnComplete(OnUpdateRankBarComplete);
		}

		private void OnUpdateRankBarComplete()
		{
			currentValueX = unit * (float)SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
			for (int i = 0; i < waveRanks.Length; i++)
			{
				if (SingletonMonoBehaviour<GameData>.Instance.CurrentWave == waveRanks[i])
				{
					chestBox[i].material.SetFloat("_EffectAmount", 0f);
				}
			}
		}
	}
}
