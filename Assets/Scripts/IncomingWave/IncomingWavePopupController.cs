using Gameplay;
using Middle;
using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace IncomingWave
{
	public class IncomingWavePopupController : GameplayPopupController
	{
		[Space]
		[SerializeField]
		private List<EnemyPreview> listEnemyPreview = new List<EnemyPreview>();

		[SerializeField]
		private EnemyPreview bossPreview;

		[Space]
		[Header("Content size setter")]
		[SerializeField]
		private RectTransform content;

		private Vector2 popupPosition = Vector2.zero;

		private Vector2 contentSize = Vector2.zero;

		[SerializeField]
		private int baseHeightValue;

		[SerializeField]
		private int normalEnemyHeightSize;

		[SerializeField]
		private int bossHeightSize;

		private List<int> listEnemyIDInWave = new List<int>();

		private int currentWave;

		public void Init(Vector2 buttonPosition, Vector2 buttonSizeDelta)
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
				listEnemyIDInWave = SingletonMonoBehaviour<GameData>.Instance.GetListEnemyIDWithWave(SingletonMonoBehaviour<GameData>.Instance.CurrentWave);
				break;
			case GameMode.DailyTrialMode:
				currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
				listEnemyIDInWave = SingletonMonoBehaviour<GameData>.Instance.GetListEnemyIDWithWave(SingletonMonoBehaviour<GameData>.Instance.CurrentWave);
				break;
			case GameMode.TournamentMode:
				if (!GameplayManager.Instance.endlessModeManager.IsLastEnemyInNormalWave)
				{
					currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
					if (SingletonMonoBehaviour<GameData>.Instance.CurrentWave >= SingletonMonoBehaviour<GameData>.Instance.TotalWave)
					{
						currentWave = SingletonMonoBehaviour<GameData>.Instance.TotalWave - 1;
					}
					listEnemyIDInWave = SingletonMonoBehaviour<GameData>.Instance.GetListEnemyIDWithWave(currentWave);
				}
				else
				{
					currentWave = GameplayManager.Instance.endlessModeManager.CurrentWaveEndless;
					listEnemyIDInWave = SingletonMonoBehaviour<GameData>.Instance.GetListEnemyIDWithWave(currentWave);
				}
				break;
			}
			HideAllListEnemies();
			for (int i = 0; i < listEnemyIDInWave.Count; i++)
			{
				int num = 0;
				if (EnemyParameterManager.Instance.IsBoss(listEnemyIDInWave[i]))
				{
					num = SingletonMonoBehaviour<GameData>.Instance.GetTotalEnemyInWave(listEnemyIDInWave[i], currentWave);
					bossPreview.Init(listEnemyIDInWave[i], num);
				}
				else
				{
					num = SingletonMonoBehaviour<GameData>.Instance.GetTotalEnemyInWave(listEnemyIDInWave[i], currentWave);
					listEnemyPreview[i].Init(listEnemyIDInWave[i], num);
				}
			}
			SetContentSize();
			Open();
			InitPopupPosition(buttonPosition, buttonSizeDelta);
		}

		private void InitPopupPosition(Vector2 buttonPosition, Vector2 buttonSizeDelta)
		{
			float num = 1.77777779f;
			float num2 = (float)Screen.width / (float)Screen.height;
			float d = num / num2;
			if (buttonPosition.x <= 0f && buttonPosition.y <= 0f)
			{
				Vector2 a = buttonPosition;
				Vector2 sizeDelta = content.sizeDelta;
				float x = sizeDelta.x / 2f + buttonSizeDelta.x / 2f;
				Vector2 sizeDelta2 = content.sizeDelta;
				popupPosition = a + new Vector2(x, sizeDelta2.y / 2f + buttonSizeDelta.y / 2f) * d;
			}
			else if (buttonPosition.x <= 0f && buttonPosition.y > 0f)
			{
				Vector2 a2 = buttonPosition;
				Vector2 sizeDelta3 = content.sizeDelta;
				float x2 = sizeDelta3.x / 2f + buttonSizeDelta.x / 2f;
				Vector2 sizeDelta4 = content.sizeDelta;
				popupPosition = a2 + new Vector2(x2, (0f - sizeDelta4.y) / 2f - buttonSizeDelta.y / 2f) * d;
			}
			else if (buttonPosition.x > 0f && buttonPosition.y > 0f)
			{
				Vector2 a3 = buttonPosition;
				Vector2 sizeDelta5 = content.sizeDelta;
				float x3 = (0f - sizeDelta5.x) / 2f - buttonSizeDelta.x / 2f;
				Vector2 sizeDelta6 = content.sizeDelta;
				popupPosition = a3 + new Vector2(x3, (0f - sizeDelta6.y) / 2f - buttonSizeDelta.y / 2f) * d;
			}
			else if (buttonPosition.x > 0f && buttonPosition.y < 0f)
			{
				Vector2 a4 = buttonPosition;
				Vector2 sizeDelta7 = content.sizeDelta;
				float x4 = (0f - sizeDelta7.x) / 2f - buttonSizeDelta.x / 2f;
				Vector2 sizeDelta8 = content.sizeDelta;
				popupPosition = a4 + new Vector2(x4, sizeDelta8.y / 2f + buttonSizeDelta.y / 2f) * d;
			}
			content.transform.localPosition = popupPosition;
		}

		private void SetContentSize()
		{
			contentSize = content.sizeDelta;
			if (EnemyParameterManager.Instance.IsWaveHaveBoss(listEnemyIDInWave))
			{
				float num = Mathf.Ceil((float)(listEnemyIDInWave.Count - 1) / 2f);
				float newY = (float)baseHeightValue + num * (float)normalEnemyHeightSize + (float)bossHeightSize;
				contentSize.Set(contentSize.x, newY);
			}
			else
			{
				float num2 = Mathf.Ceil((float)listEnemyIDInWave.Count / 2f);
				float newY2 = (float)baseHeightValue + num2 * (float)normalEnemyHeightSize;
				contentSize.Set(contentSize.x, newY2);
			}
			content.sizeDelta = contentSize;
		}

		private void HideAllListEnemies()
		{
			foreach (EnemyPreview item in listEnemyPreview)
			{
				item.Hide();
			}
			bossPreview.Hide();
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
