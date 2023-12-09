using System;
using System.Collections.Generic;

[Serializable]
public class UserData_Map
{
	public int mapIDUnlocked;

	public int lastMapIDPlayed;

	public int lastMapModeChoose;

	public List<UserData_Map_Unique> listDataMap;

	public UserData_Map()
	{
	}

	public UserData_Map(int mapIDUnlocked, int lastMapIDPlayed, int lastMapModeChoose, List<UserData_Map_Unique> listDataMap)
	{
		this.mapIDUnlocked = mapIDUnlocked;
		this.lastMapIDPlayed = lastMapIDPlayed;
		this.lastMapModeChoose = lastMapModeChoose;
		this.listDataMap = listDataMap;
	}
}
