using System;
using System.Collections.Generic;

[Serializable]
public class UserData_GlobalUpgrade
{
	public List<UserData_GlobalUpgrade_Unique> listUpgradedTower;

	public UserData_GlobalUpgrade()
	{
	}

	public UserData_GlobalUpgrade(List<UserData_GlobalUpgrade_Unique> listUpgradedTower)
	{
		this.listUpgradedTower = listUpgradedTower;
	}
}
