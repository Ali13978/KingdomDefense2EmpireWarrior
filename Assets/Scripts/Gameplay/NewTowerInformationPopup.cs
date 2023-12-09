using Middle;
using Parameter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class NewTowerInformationPopup : GameplayPopupController
	{
		[Space]
		[Header("Information for normal upgrade")]
		[SerializeField]
		private TowerAvatar[] listNormalTowerAvatars;

		[SerializeField]
		private GameObject normalDescriptionHolder;

		[SerializeField]
		private Text normalDescription;

		[Space]
		[Header("Information for ultimate upgrade")]
		[SerializeField]
		private TowerAvatar ultimateTowerAvatars;

		[SerializeField]
		private GameObject ultimateDescriptionHolder;

		[SerializeField]
		private Text generalDescription;

		[SerializeField]
		private Text ultimateDescription;

		public void Init()
		{
			OpenWithScaleAnimation();
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
			{
				int tipInforID = MapRuleParameter.Instance.getTipInforID(SingletonMonoBehaviour<GameData>.Instance.MapID);
				if (tipInforID > 0)
				{
					InitNormalInformation_Campaign();
				}
				else
				{
					InitUltimateInformation_Campaign();
				}
				break;
			}
			case GameMode.DailyTrialMode:
				InitNormalInformation_DailyTrial();
				break;
			}
		}

		private void InitNormalInformation_Campaign()
		{
			normalDescriptionHolder.SetActive(value: true);
			int tipInforID = MapRuleParameter.Instance.getTipInforID(SingletonMonoBehaviour<GameData>.Instance.MapID);
			normalDescription.text = Singleton<GameplayTipsDescription>.Instance.GetDescription(tipInforID).Replace('@', '\n').Replace('#', '-');
			List<TowerTutorial> listTowerTutorialInMap = MapRuleParameter.Instance.getListTowerTutorialInMap(SingletonMonoBehaviour<GameData>.Instance.MapID);
			for (int i = 0; i < listTowerTutorialInMap.Count; i++)
			{
				listNormalTowerAvatars[i].Init(listTowerTutorialInMap[i].id, listTowerTutorialInMap[i].level);
				listNormalTowerAvatars[i].Show();
			}
		}

		private void InitUltimateInformation_Campaign()
		{
			ultimateDescriptionHolder.SetActive(value: true);
			List<TowerTutorial> listTowerTutorialInMap = MapRuleParameter.Instance.getListTowerTutorialInMap(SingletonMonoBehaviour<GameData>.Instance.MapID);
			int id = listTowerTutorialInMap[0].id;
			int level = listTowerTutorialInMap[0].level;
			generalDescription.text = Singleton<TowerDescription>.Instance.GetTowerUnlockDescription(id, level).Replace('@', '\n').Replace('#', '-');
			string text = Singleton<TowerDescription>.Instance.GetTowerUltimateDescription(id, level, 0) + "\n" + Singleton<TowerDescription>.Instance.GetTowerUltimateDescription(id, level, 1);
			ultimateDescription.text = text.Replace('@', '\n').Replace('#', '-');
			ultimateTowerAvatars.Init(id, level);
			ultimateTowerAvatars.Show();
		}

		private void InitNormalInformation_DailyTrial()
		{
			normalDescriptionHolder.SetActive(value: true);
			int notiID = 91;
			normalDescription.text = Singleton<NotificationDescription>.Instance.GetNotiContent(notiID).Replace('@', '\n').Replace('#', '-');
			TowerAvatar[] array = listNormalTowerAvatars;
			foreach (TowerAvatar towerAvatar in array)
			{
				towerAvatar.Hide();
			}
			int currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
			List<int> listTowerIDForPopup = MapRuleParameter.Instance.GetListTowerIDForPopup(currentWave);
			for (int j = 0; j < listTowerIDForPopup.Count; j++)
			{
				int maxLevelTowerCanUpgrade_Daily = MapRuleParameter.Instance.GetMaxLevelTowerCanUpgrade_Daily(currentWave, listTowerIDForPopup[j]);
				listNormalTowerAvatars[j].Init(listTowerIDForPopup[j], maxLevelTowerCanUpgrade_Daily);
				listNormalTowerAvatars[j].Show();
			}
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.DailyTrialMode:
				GameplayManager.Instance.gameSpeedController.PauseGame();
				break;
			}
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.DailyTrialMode:
				GameplayManager.Instance.gameSpeedController.UnPauseGame();
				break;
			}
		}
	}
}
