using Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace FreeResources
{
	public class FreeResourcesPopupController : GameplayPopupController
	{
		[Space]
		[SerializeField]
		private List<FreeResourcesButtonController> listFreeResourcesButton = new List<FreeResourcesButtonController>();

		public void Init()
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			foreach (FreeResourcesButtonController item in listFreeResourcesButton)
			{
				item.InitData();
			}
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}
