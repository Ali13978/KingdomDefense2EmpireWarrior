public class TestConfig : Singleton<TestConfig>
{
	public int gemNumber
	{
		get;
		set;
	}

	public int starsNumber
	{
		get;
		set;
	}

	public bool unlockAllMaps
	{
		get;
		set;
	}

	public bool unlockAllThemes
	{
		get;
		set;
	}

	public bool useCustomCSV
	{
		get;
		set;
	}

	public int tourDefaultMapId
	{
		get;
		set;
	}
}
