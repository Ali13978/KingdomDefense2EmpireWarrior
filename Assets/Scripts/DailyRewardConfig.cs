using System;
using UnityEngine;

[Serializable]
public class DailyRewardConfig : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public DailyRewardConfigData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new DailyRewardConfigData[0];
		}
	}
}
