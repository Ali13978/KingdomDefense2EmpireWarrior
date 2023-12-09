using System;
using UnityEngine;

[Serializable]
public class PetConfig : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public PetConfigData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new PetConfigData[0];
		}
	}
}
