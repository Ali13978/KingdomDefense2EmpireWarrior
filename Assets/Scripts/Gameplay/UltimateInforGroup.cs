using MyCustom;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class UltimateInforGroup : CustomMonoBehaviour
	{
		[SerializeField]
		private List<UltimateInforItem> listUltiInforItem = new List<UltimateInforItem>();

		[Space]
		[Header("Offset position for items")]
		[SerializeField]
		private List<int> allignCenter = new List<int>();

		[SerializeField]
		private List<int> allignLeft = new List<int>();

		[SerializeField]
		private List<int> allignRight = new List<int>();

		private TowerModel towerModel;

		private bool toggle;

		private float screenLenght = 6.4f;

		public void InitList()
		{
			towerModel = SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.towerModel;
			Vector3 position = towerModel.transform.position;
			if (position.x > screenLenght / 2f)
			{
				UnityEngine.Debug.Log("tower phai!");
				for (int i = 0; i < 2; i++)
				{
					listUltiInforItem[i].Init(towerModel.Id, towerModel.Level, allignLeft[i], i);
					listUltiInforItem[i].Show();
				}
				return;
			}
			Vector3 position2 = towerModel.transform.position;
			if (position2.x < (0f - screenLenght) / 2f)
			{
				UnityEngine.Debug.Log("tower trai");
				for (int j = 0; j < 2; j++)
				{
					listUltiInforItem[j].Init(towerModel.Id, towerModel.Level, allignRight[j], j);
					listUltiInforItem[j].Show();
				}
			}
			else
			{
				UnityEngine.Debug.Log("tower giua!");
				for (int k = 0; k < 2; k++)
				{
					listUltiInforItem[k].Init(towerModel.Id, towerModel.Level, allignCenter[k], k);
					listUltiInforItem[k].Show();
				}
			}
		}

		public void HideList()
		{
			foreach (UltimateInforItem item in listUltiInforItem)
			{
				item.Hide();
				toggle = false;
			}
		}

		public void TogglePopup()
		{
			toggle = !toggle;
			if (toggle)
			{
				InitList();
			}
			else
			{
				HideList();
			}
		}
	}
}
