using Data;

namespace Notify
{
	public class NotifyDailyTrial : NotifyUnit
	{
		protected override bool ShouldShowNotify()
		{
			return isFreeResoucesAvailable();
		}

		private bool isFreeResoucesAvailable()
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			return !ReadWriteDataDailyTrial.Instance.IsDoneMaxTierMission(currentDayIndex);
		}
	}
}
