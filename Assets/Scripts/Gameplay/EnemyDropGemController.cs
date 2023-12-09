using Middle;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class EnemyDropGemController : EnemyController
	{
		private float dropGemPercent;

		private int dropGemAmountMin;

		private int dropGemAmountMax;

		public override void OnAppear()
		{
			base.OnAppear();
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			dropGemAmountMin = originalParameter.dropGemAmountMin;
			Enemy originalParameter2 = base.EnemyModel.OriginalParameter;
			dropGemAmountMax = originalParameter2.dropGemAmountMax;
			int playCountMapInCampaign = SingletonMonoBehaviour<GameData>.Instance.PlayCountMapInCampaign;
			if (playCountMapInCampaign == 0)
			{
				Enemy originalParameter3 = base.EnemyModel.OriginalParameter;
				dropGemPercent = (float)originalParameter3.dropGemPercent * ((float)Config.Instance.FirstTimeGemTakenPercentage / 100f);
			}
			if (playCountMapInCampaign == 1)
			{
				Enemy originalParameter4 = base.EnemyModel.OriginalParameter;
				dropGemPercent = (float)originalParameter4.dropGemPercent * ((float)Config.Instance.SecondTimeGemTakenPercentage / 100f);
			}
			if (playCountMapInCampaign >= 2)
			{
				Enemy originalParameter5 = base.EnemyModel.OriginalParameter;
				dropGemPercent = (float)originalParameter5.dropGemPercent * ((float)Config.Instance.ThirdTimeGemTakenPercentage / 100f);
			}
		}

		public void TryDropDiamond()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				if ((float)Random.Range(0, 100) < dropGemPercent && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
				{
					int num = Random.Range(dropGemAmountMin, dropGemAmountMax);
					GemController droppedGem = SingletonMonoBehaviour<SpawnFX>.Instance.GetDroppedGem();
					droppedGem.gameObject.SetActive(value: true);
					droppedGem.transform.position = base.EnemyModel.transform.position;
					droppedGem.Init(num);
					SingletonMonoBehaviour<GameData>.Instance.GameplayGem += num;
				}
				break;
			}
		}
	}
}
