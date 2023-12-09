using Middle;
using MyCustom;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class FirstTimeTipAppear : CustomMonoBehaviour
	{
		[SerializeField]
		private int tipID;

		public void TryToGetTip()
		{
			if (UnlockedGameplayTips.Instance.IsTipFirstTime(tipID))
			{
				NewTipInformationUIManager.Instance.TryActivateButton(tipID);
			}
		}
	}
}
