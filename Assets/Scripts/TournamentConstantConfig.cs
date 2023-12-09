using System;
using UnityEngine;

[Serializable]
public class TournamentConstantConfig : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public TournamentConstantConfigData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new TournamentConstantConfigData[0];
		}
	}
}
