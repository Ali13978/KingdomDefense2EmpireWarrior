using System;
using System.Collections.Generic;

public class TourPlayerInfo
{
	public int rank;

	public string name;

	public List<int> heroIds;

	public TimeSpan time;

	public bool isYou;

	public string countryCode = "gb";

	public TourPlayerInfo(string name, List<int> heroIds, TimeSpan time, bool isYou, string country)
	{
		this.name = name;
		this.heroIds = heroIds;
		this.time = time;
		this.isYou = isYou;
		countryCode = country;
	}

	public TourPlayerInfo(KeyValuePair<string, TourSeasonGroup_Member> entry, string uid)
	{
		name = entry.Value.name;
		heroIds = GameTools.DecodeHeroList(entry.Value.heroes);
		time = new TimeSpan(0, 0, 0, 0, entry.Value.score);
		isYou = (entry.Key == uid);
		countryCode = entry.Value.country;
	}
}
