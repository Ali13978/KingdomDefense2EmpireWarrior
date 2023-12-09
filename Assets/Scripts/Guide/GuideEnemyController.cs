using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class GuideEnemyController : GeneralPopupController
	{
		[NonSerialized]
		public int currentEnemyIDSelected;

		[SerializeField]
		private Transform listButtonsHolder;

		[SerializeField]
		private Transform listAvatarHolder;

		[SerializeField]
		private EnemyInformationController enemyInformationController;

		[SerializeField]
		private Image enemyAvatar;

		private List<SelectEnemyButtonController> listSelectEnemy = new List<SelectEnemyButtonController>();

		private List<int> listEnemyID;

		private List<bool> listUnlockedEnemy;

		public void Init()
		{
			Open();
			listEnemyID = EnemyParameterManager.Instance.GetAllEnemyIds();
			InitListButton();
			SetButtonStatus();
			CustomInvoke(InitDefaultData, Time.deltaTime);
		}

		private void InitListButton()
		{
			if (listSelectEnemy.Count < 1)
			{
				for (int i = 0; i < listEnemyID.Count; i++)
				{
					string path = $"UI WorldMap/Guide/SelectEnemy/select_enemy";
					SelectEnemyButtonController selectEnemyButtonController = UnityEngine.Object.Instantiate(Resources.Load<SelectEnemyButtonController>(path));
					selectEnemyButtonController.transform.SetParent(listButtonsHolder);
					selectEnemyButtonController.Init(listEnemyID[i]);
					selectEnemyButtonController.transform.localScale = Vector3.one;
					listSelectEnemy.Add(selectEnemyButtonController);
				}
			}
		}

		private void SetButtonStatus()
		{
			listUnlockedEnemy = UnlockedEnemies.Instance.GetListEnemyUnlockStatus();
			for (int i = 0; i < listEnemyID.Count; i++)
			{
				if (listUnlockedEnemy[i])
				{
					listSelectEnemy[i].SetUnLock();
				}
				else
				{
					listSelectEnemy[i].SetLock();
				}
			}
		}

		public void RefreshEnemyInformation()
		{
			SetAvatar();
			enemyInformationController.Init(currentEnemyIDSelected);
		}

		private void SetAvatar()
		{
			enemyAvatar.sprite = Resources.Load<Sprite>($"Preview/Enemies/FullAvatars/fa_enemy_{currentEnemyIDSelected}");
		}

		private void InitDefaultData()
		{
			if (listUnlockedEnemy.Count > 0 && listUnlockedEnemy[0])
			{
				listSelectEnemy[0].OnClick();
			}
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			base.Close();
			GuidePopupController.Instance.HideSelectedEnemyImage();
		}
	}
}
