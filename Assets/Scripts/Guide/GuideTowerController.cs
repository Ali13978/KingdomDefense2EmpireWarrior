using Data;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class GuideTowerController : GeneralPopupController
	{
		[NonSerialized]
		public int currentTowerIDSelected;

		[NonSerialized]
		public int currentTowerLvSelected;

		[SerializeField]
		private Transform[] listButtonsHolder;

		[SerializeField]
		private TowerInformationController towerInformationController;

		private List<List<SelectTowerButtonController>> listSelectTower = new List<List<SelectTowerButtonController>>();

		[SerializeField]
		private Image towerAvatar;

		public void Init()
		{
			Open();
			InitListButton();
			SetButtonsStatus();
			CustomInvoke(InitDefaultData, Time.deltaTime);
		}

		private void InitListButton()
		{
			if (listSelectTower.Count >= 1)
			{
				return;
			}
			int numberOfTower = TowerParameter.Instance.GetNumberOfTower();
			for (int i = 0; i < numberOfTower; i++)
			{
				listSelectTower.Add(new List<SelectTowerButtonController>());
				for (int j = 0; j < 5; j++)
				{
					string path = $"UI WorldMap/Guide/SelectTower/select_tower";
					SelectTowerButtonController selectTowerButtonController = UnityEngine.Object.Instantiate(Resources.Load<SelectTowerButtonController>(path));
					selectTowerButtonController.transform.SetParent(listButtonsHolder[i]);
					selectTowerButtonController.Init(i, j);
					selectTowerButtonController.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
					listSelectTower[i].Add(selectTowerButtonController);
				}
			}
		}

		private void SetButtonsStatus()
		{
			int mapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked();
			int[] maxTowerLevelByMapID = MapRuleParameter.Instance.GetMaxTowerLevelByMapID(mapIDUnlocked);
			int numberOfTower = TowerParameter.Instance.GetNumberOfTower();
			for (int i = 0; i < numberOfTower; i++)
			{
				for (int j = 0; j <= 4; j++)
				{
					if (j <= maxTowerLevelByMapID[i])
					{
						listSelectTower[i][j].SetUnLock();
					}
					else
					{
						listSelectTower[i][j].SetLock();
					}
				}
			}
		}

		public void RefreshTowerInformation()
		{
			SetAvatar();
			towerInformationController.Init(currentTowerIDSelected, currentTowerLvSelected);
		}

		private void InitDefaultData()
		{
			towerInformationController.HideAll();
			if (listSelectTower.Count > 0)
			{
				listSelectTower[0][0].OnClick();
			}
		}

		private void SetAvatar()
		{
			towerAvatar.sprite = Resources.Load<Sprite>($"Preview/Towers/p_tower_{currentTowerIDSelected}_{currentTowerLvSelected}");
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			base.Close();
			GuidePopupController.Instance.HideSelectedTowerImage();
		}
	}
}
