using Data;
using Parameter;
using System.Collections.Generic;

namespace DailyTrial
{
	public class WatchOfferButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			InitOffer();
		}

		private void InitOffer()
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			List<int> listInputHeroesID = DailyTrialParameter.Instance.getListInputHeroesID(currentDayIndex);
		}

		public void TurnOffButton()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
