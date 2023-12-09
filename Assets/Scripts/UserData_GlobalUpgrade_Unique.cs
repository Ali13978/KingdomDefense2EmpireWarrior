using System;

[Serializable]
public class UserData_GlobalUpgrade_Unique
{
	public int towerID;

	public int towerUpgradedLevel;

	public UserData_GlobalUpgrade_Unique()
	{
	}

	public UserData_GlobalUpgrade_Unique(int towerID, int towerUpgradedLevel)
	{
		this.towerID = towerID;
		this.towerUpgradedLevel = towerUpgradedLevel;
	}
}
