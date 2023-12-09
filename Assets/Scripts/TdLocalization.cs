using System;
using UnityEngine;

[Serializable]
public class TdLocalization : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public TdLocalizationData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new TdLocalizationData[0];
		}
	}
}
