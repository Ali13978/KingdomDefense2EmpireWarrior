using UnityEngine;

internal class EditorBoard : IBoard
{
	public void SetText(string str)
	{
		GUIUtility.systemCopyBuffer = str;
	}

	public string GetText()
	{
		return GUIUtility.systemCopyBuffer;
	}
}
