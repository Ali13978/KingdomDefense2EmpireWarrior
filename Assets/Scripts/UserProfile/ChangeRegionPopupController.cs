using Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace UserProfile
{
	public class ChangeRegionPopupController : GameplayPopupController
	{
		[SerializeField]
		private List<SelectRegionButtonController> selectRegionButtonController = new List<SelectRegionButtonController>();

		[SerializeField]
		private GameObject listItemHolder;

		public void Init()
		{
			OpenWithScaleAnimation();
		}
	}
}
