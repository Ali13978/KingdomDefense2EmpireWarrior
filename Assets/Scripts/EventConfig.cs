using System;
using UnityEngine;

[Serializable]
public class EventConfig : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public EventConfigData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new EventConfigData[0];
		}
	}
}
