using Data;
using Middle;
using UnityEngine;

namespace MapLevel
{
	public class ModeSelectButtonController : ButtonController
	{
		[SerializeField]
		private GameModeSelectGroupController gameModeSelectGroupController;

		[SerializeField]
		private BattleLevel battleLevel;

		public override void OnClick()
		{
			base.OnClick();
			MiddleDelivery.Instance.BattleLevel = battleLevel;
			gameModeSelectGroupController.ShowSelectedImage(battleLevel);
			ReadWriteDataMap.Instance.SaveLastMapModeChoose((int)(battleLevel + 1));
		}
	}
}
