using Parameter;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class EndGameVideoController : GameplayPopupController
	{
		[Space]
		[SerializeField]
		private Text title;

		[SerializeField]
		private Text lifeAmount;

		[SerializeField]
		private Text countdownText;

		private int originTimeCooldown;

		private int currentTimeCooldown;

		public void Init()
		{
			OpenWithScaleAnimation();
			ShowTitle();
			GameplayManager.Instance.gameSpeedController.PauseGame();
			originTimeCooldown = SingletonMonoBehaviour<GameplayDataReader>.Instance.ReadDataEndGameVideo.GetCountdownTime();
			currentTimeCooldown = originTimeCooldown;
			StartCountDown();
		}

		private void ShowTitle()
		{
			int actuallyGemAmount = SingletonMonoBehaviour<GameData>.Instance.GetActuallyGemAmount();
			string text = string.Format(Singleton<NotificationDescription>.Instance.GetNotiContent(21), actuallyGemAmount);
			title.text = text.Replace('@', '\n').Replace('#', '-');
			lifeAmount.text = "+" + SingletonMonoBehaviour<GameplayDataReader>.Instance.ReadDataEndGameVideo.GetEndGameVideoReward();
		}

		private IEnumerator ICountDown()
		{
			for (int i = 0; i <= originTimeCooldown; i++)
			{
				countdownText.text = currentTimeCooldown.ToString();
				currentTimeCooldown--;
				yield return new WaitForSecondsRealtime(1f);
			}
			OutOfTime();
		}

		public void StartCountDown()
		{
			StartCoroutine(ICountDown());
		}

		public void StopCountDown()
		{
			StopCoroutine(ICountDown());
		}

		private void OutOfTime()
		{
			UnityEngine.Debug.Log("out of time!");
			Close();
			GameplayManager.Instance.gameLogicController.Defeated();
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(value: true);
		}

		public override void Close()
		{
			base.Close();
			base.gameObject.SetActive(value: false);
		}
	}
}
