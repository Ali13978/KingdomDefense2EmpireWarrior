using System;
using System.Collections.Generic;

[Serializable]
public class UserData_Hero
{
	public List<UserData_Hero_Unique> listHeroData;

	public UserData_Hero()
	{
	}

	public UserData_Hero(List<UserData_Hero_Unique> listHeroData)
	{
		this.listHeroData = listHeroData;
	}
}
