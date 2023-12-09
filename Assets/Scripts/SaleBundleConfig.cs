using System;
using UnityEngine;

[Serializable]
public class SaleBundleConfig : ScriptableObject
{
	[HideInInspector]
	[SerializeField]
	public string SheetName = string.Empty;

	[HideInInspector]
	[SerializeField]
	public string WorksheetName = string.Empty;

	public SaleBundleConfigData[] dataArray;

	private void OnEnable()
	{
		if (dataArray == null)
		{
			dataArray = new SaleBundleConfigData[0];
		}
	}
}
