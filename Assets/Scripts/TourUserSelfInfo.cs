public class TourUserSelfInfo
{
	public int curgroupid;

	public int curtier;

	public int lastgroupid;

	public int recFriendReward;

	public int recSeasonReward;

	public string name;

	public int score;

	public int lastscore = -1;

	public int heroesCode;

	public string countryCode;

	public bool tierup;

	public TourUserSelfInfo(int curgroupid, int curtier, int lastgroupid, int recFriendReward, int recSeasonReward, string name, int score, int lastscore, int heroesCode, string country, bool tierup)
	{
		this.curgroupid = curgroupid;
		this.curtier = curtier;
		this.lastgroupid = lastgroupid;
		this.recFriendReward = recFriendReward;
		this.recSeasonReward = recSeasonReward;
		this.name = name;
		this.score = score;
		this.heroesCode = heroesCode;
		countryCode = country;
		this.tierup = tierup;
	}
}
