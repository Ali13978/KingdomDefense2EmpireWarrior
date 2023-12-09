using Data;

public static class UpgradeStarCalculator
{
	public static int GetCurrentStar()
	{
		int num = 0;
		int currentStar = ReadWriteDataPlayerCurrency.Instance.GetCurrentStar();
		int num2 = 0;
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j <= ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(i); j++)
			{
				int starRequireForUpgrade = ReadWriteDataGlobalUpgrade.Instance.GetStarRequireForUpgrade(i, j);
				num2 += starRequireForUpgrade;
			}
		}
		return currentStar - num2;
	}
}
