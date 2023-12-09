using System;
using UnityEngine;

public class TourSeasonInfo
{
	public int seasonNumber;

	public DateTime seasonStartDate;

	public DateTime seasonEndDate;

	public string minVersion = "0.0.0";

	public bool isChoosingGroupBaseOnTier;

	public TourSeasonInfo(int seasonNumber, DateTime startData, DateTime endData)
	{
		this.seasonNumber = seasonNumber;
		seasonStartDate = startData;
		seasonEndDate = endData;
	}

	public TourSeasonInfo(int seasonNumber, DateTime startData, DateTime endData, string minVersion, bool chooseGroupBaseonTier)
		: this(seasonNumber, startData, endData)
	{
		this.minVersion = minVersion;
		isChoosingGroupBaseOnTier = chooseGroupBaseonTier;
	}

	public bool IsCurVersionUptodate()
	{
		string version = Application.version;
		string[] array = minVersion.Split('.');
		string[] array2 = version.Split('.');
		int num = Mathf.Min(array.Length, array2.Length);
		for (int i = 0; i < num; i++)
		{
			int.TryParse(array[i], out int result);
			int.TryParse(array2[i], out int result2);
			if (result > result2)
			{
				return false;
			}
			if (result2 > result)
			{
				break;
			}
		}
		return true;
	}
}
