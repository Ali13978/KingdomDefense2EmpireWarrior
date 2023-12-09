using System.Collections.Generic;
using UnityEngine;

namespace DailyTrial
{
	public class DailyTabsGroupController : MonoBehaviour
	{
		[SerializeField]
		private List<DailyTab> listDailyTabs = new List<DailyTab>();

		public void InitTabsData()
		{
			foreach (DailyTab listDailyTab in listDailyTabs)
			{
				listDailyTab.Init();
			}
		}
	}
}
