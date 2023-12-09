using System;
using UnityEngine;

[Serializable]
public class ExcelExample : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public ExcelExampleData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new ExcelExampleData[0];
		}
	}
}
