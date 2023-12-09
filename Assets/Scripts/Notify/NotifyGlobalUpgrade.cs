using Data;

namespace Notify
{
	public class NotifyGlobalUpgrade : NotifyUnit
	{
		protected override bool ShouldShowNotify()
		{
			return isEnoughStarForNextUpgrade();
		}

		private bool isEnoughStarForNextUpgrade()
		{
			bool result = false;
			int[] array = new int[4];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(i);
			}
			int currentStar = UpgradeStarCalculator.GetCurrentStar();
			for (int j = 0; j < 4; j++)
			{
				int starRequireForUpgrade = ReadWriteDataGlobalUpgrade.Instance.GetStarRequireForUpgrade(j, array[j]);
				if (currentStar >= starRequireForUpgrade)
				{
					result = true;
				}
			}
			return result;
		}
	}
}
