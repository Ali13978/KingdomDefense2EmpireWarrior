using UnityEngine;

internal class AndroidBoard : IBoard
{
	private AndroidJavaClass cb = new AndroidJavaClass("jp.ne.donuts.uniclipboard.Clipboard");

	public void SetText(string str)
	{
		UnityEngine.Debug.Log("Set Text At AndroidBoard: " + str);
		cb.CallStatic("setText", str);
	}

	public string GetText()
	{
		return cb.CallStatic<string>("getText", new object[0]);
	}
}
