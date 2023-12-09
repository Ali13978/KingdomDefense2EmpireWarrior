using System;
using UnityEngine;

[Serializable]
public class ItemConfig : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public ItemConfigData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new ItemConfigData[0];
		}
	}
}
