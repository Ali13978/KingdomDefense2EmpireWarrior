using UnityEngine;

namespace Store
{
	public class TabsGroupController : MonoBehaviour
	{
		[SerializeField]
		private Transform[] listTabs;

		[SerializeField]
		private SelectTabButtonController[] buttonsSelecTab;

		public void InitSelectedTab(int tabID)
		{
			if ((bool)listTabs[tabID])
			{
				listTabs[tabID].SetAsLastSibling();
			}
		}

		public void HighLightButton(int tabID)
		{
			SelectTabButtonController[] array = buttonsSelecTab;
			foreach (SelectTabButtonController selectTabButtonController in array)
			{
				if (selectTabButtonController.TabID == tabID)
				{
					selectTabButtonController.ViewHighlight();
				}
				else
				{
					selectTabButtonController.ViewNormal();
				}
			}
		}
	}
}
