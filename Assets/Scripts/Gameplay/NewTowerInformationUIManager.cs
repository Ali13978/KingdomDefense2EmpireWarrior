using Middle;
using MyCustom;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class NewTowerInformationUIManager : CustomMonoBehaviour
	{
		[SerializeField]
		private Transform panelParent;

		[Space]
		[SerializeField]
		private float delayTimeToCheck;

		private string newTowerInforPanelPath;

		private NewTowerInformationPopup newTowerInforPanel;

		public NewTowerInformationPopup NewTowerInforPanel
		{
			get
			{
				return newTowerInforPanel;
			}
			set
			{
				newTowerInforPanel = value;
			}
		}

		private void Start()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				if (MapRuleParameter.Instance.HaveTutorialTowerInMap(SingletonMonoBehaviour<GameData>.Instance.MapID))
				{
					CustomInvoke(InitNewTowerInformationPanel, delayTimeToCheck);
				}
				break;
			}
		}

		public void InitNewTowerInformationPanel()
		{
			newTowerInforPanelPath = "NewTower/NewTowerPanel";
			NewTowerInforPanel = Object.Instantiate(Resources.Load<NewTowerInformationPopup>(newTowerInforPanelPath));
			NewTowerInforPanel.transform.SetParent(panelParent);
			NewTowerInforPanel.transform.localScale = Vector3.one;
			NewTowerInforPanel.Init();
		}
	}
}
