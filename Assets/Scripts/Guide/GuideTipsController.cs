using System;
using System.Collections.Generic;
using UnityEngine;

namespace Guide
{
	public class GuideTipsController : GeneralPopupController
	{
		[NonSerialized]
		public int currentTipIDSelected;

		[SerializeField]
		private List<SelectTipButtonController> listSelectTips = new List<SelectTipButtonController>();

		[SerializeField]
		private TipInformationController tipInformationController;

		public void Init()
		{
			Open();
			CustomInvoke(InitDefaultData, Time.deltaTime);
		}

		public void RefreshTipInformation()
		{
			tipInformationController.InitInformation(currentTipIDSelected);
		}

		private void InitDefaultData()
		{
			if (listSelectTips.Count > 0)
			{
				listSelectTips[0].OnClick();
			}
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			base.Close();
		}
	}
}
