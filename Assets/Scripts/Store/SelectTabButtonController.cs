using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
	public class SelectTabButtonController : ButtonController
	{
		[SerializeField]
		private int tabID;

		private Image image;

		private Text text;

		[SerializeField]
		private Color textColorHighlight;

		[SerializeField]
		private Color textColorNormal;

		public int TabID
		{
			get
			{
				return tabID;
			}
			set
			{
				tabID = value;
			}
		}

		private void Awake()
		{
			image = GetComponent<Image>();
			text = GetComponentInChildren<Text>();
		}

		public override void OnClick()
		{
			base.OnClick();
			SelectTab();
		}

		private void SelectTab()
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.TabsGroupController.InitSelectedTab(TabID);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.TabsGroupController.HighLightButton(TabID);
		}

		public void ViewHighlight()
		{
			image.color = Color.white;
			text.color = textColorHighlight;
		}

		public void ViewNormal()
		{
			image.color = Color.gray;
			text.color = textColorNormal;
		}
	}
}
