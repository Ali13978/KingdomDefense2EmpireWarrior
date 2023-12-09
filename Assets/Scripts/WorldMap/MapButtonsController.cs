using Data;
using System.Collections.Generic;
using UnityEngine;

namespace WorldMap
{
	public class MapButtonsController : MonoBehaviour
	{
		public List<MapButtonController> listButtons = new List<MapButtonController>();

		[Space]
		[SerializeField]
		private GameObject focusOnMap;

		public void RefreshListMapButtons(int themeID)
		{
			int mapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked();
			switch (themeID)
			{
			case 0:
				UnlockToMap(mapIDUnlocked);
				break;
			case 1:
				UnlockToMap(mapIDUnlocked - 6);
				break;
			case 2:
				UnlockToMap(mapIDUnlocked - 12);
				break;
			}
		}

		private void HideAll()
		{
			foreach (MapButtonController listButton in listButtons)
			{
				listButton.gameObject.SetActive(value: false);
			}
		}

		private void UnlockToMap(int currentMapIDUnlocked)
		{
			for (int i = 0; i < listButtons.Count; i++)
			{
				if (i <= currentMapIDUnlocked)
				{
					listButtons[i].ViewUnLock();
				}
				else
				{
					listButtons[i].ViewLock();
				}
			}
		}
	}
}
