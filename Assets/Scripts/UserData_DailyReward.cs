using System;
using System.Collections.Generic;

[Serializable]
public class UserData_DailyReward
{
	public int currentDay;

	public List<UserData_DailyReward_Unique> listDailyRewardData;
}
