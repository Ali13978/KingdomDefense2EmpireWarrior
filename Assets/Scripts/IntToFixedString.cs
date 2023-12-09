public static class IntToFixedString
{
	private const int MaxValue = 200;

	private static string[] fixedStrings;

	private static string[] fixedStringsPercentage;

	static IntToFixedString()
	{
		fixedStrings = new string[201];
		fixedStringsPercentage = new string[201];
		for (int i = 0; i < 200; i++)
		{
			fixedStrings[i] = i.ToString();
		}
		for (int j = 0; j < 200; j++)
		{
			fixedStringsPercentage[j] = j.ToString() + "%";
		}
	}

	public static string ToFixedString(this int numberUnder100)
	{
		return fixedStrings[numberUnder100];
	}

	public static string ToFixedStringPercentage(this int numberUnder100)
	{
		return fixedStringsPercentage[numberUnder100];
	}
}
