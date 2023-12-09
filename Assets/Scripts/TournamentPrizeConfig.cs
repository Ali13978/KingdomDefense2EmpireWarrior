using System;
using UnityEngine;

[Serializable]
public class TournamentPrizeConfig : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public TournamentPrizeConfigData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new TournamentPrizeConfigData[0];
		}
	}
}
