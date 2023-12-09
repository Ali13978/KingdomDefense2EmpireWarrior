using System;

[Serializable]
public class UserData_Map_Unique
{
	public int mapID;

	public int starEarned;

	public int playCount;

	public int playCount_victory;

	public int playCount_defeat;

	public UserData_Map_Unique()
	{
	}

	public UserData_Map_Unique(int mapID, int starEarned, int playCount, int playCount_victory, int playCount_defeat)
	{
		this.mapID = mapID;
		this.starEarned = starEarned;
		this.playCount = playCount;
		this.playCount_victory = playCount_victory;
		this.playCount_defeat = playCount_defeat;
	}
}
