public class TourUsers_UserData
{
	public int curgroupid = -1;

	public int curtier;

	public int heroes = 3;

	public int lastgroupid = -1;

	public string name;

	public int recFriendReward;

	public int recSeasonReward;

	public int score;

	public int lastscore = -1;

	public string country = "ch";

	public bool tierup;

	public TourUsers_UserData()
	{
	}

	public TourUsers_UserData(TourUserSelfInfo userInfo)
	{
		name = userInfo.name;
		heroes = userInfo.heroesCode;
		score = userInfo.score;
		lastscore = userInfo.lastscore;
		country = userInfo.countryCode;
	}
}
