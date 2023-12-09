using System;
using UnityEngine;

public class SampleTimer : MonoBehaviour
{
	private DateTime simpleTimerEndTimestamp;

	private DateTime unbiasedTimerEndTimestamp;

	private void Awake()
	{
		simpleTimerEndTimestamp = ReadTimestamp("simpleTimer", DateTime.Now.AddSeconds(60.0));
		unbiasedTimerEndTimestamp = ReadTimestamp("unbiasedTimer", UnbiasedTime.Instance.Now().AddSeconds(60.0));
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			WriteTimestamp("simpleTimer", simpleTimerEndTimestamp);
			WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
		}
		else
		{
			simpleTimerEndTimestamp = ReadTimestamp("simpleTimer", DateTime.Now.AddSeconds(60.0));
			unbiasedTimerEndTimestamp = ReadTimestamp("unbiasedTimer", UnbiasedTime.Instance.Now().AddSeconds(60.0));
		}
	}

	private void OnApplicationQuit()
	{
		WriteTimestamp("simpleTimer", simpleTimerEndTimestamp);
		WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
	}

	private void OnGUI()
	{
		TimeSpan timeSpan = simpleTimerEndTimestamp - DateTime.Now;
		TimeSpan timeSpan2 = unbiasedTimerEndTimestamp - UnbiasedTime.Instance.Now();
		float num = Screen.width;
		float num2 = Screen.height;
		GUIStyle box = GUI.skin.box;
		box.fontSize = (int)(12f * num2 / 480f);
		GUIStyle label = GUI.skin.label;
		label.fontSize = (int)(24f * num2 / 480f);
		label.alignment = TextAnchor.UpperCenter;
		GUIStyle button = GUI.skin.button;
		button.fontSize = (int)(14f * num2 / 480f);
		string text = "END";
		if (timeSpan.TotalSeconds > 0.0)
		{
			text = $"{timeSpan.Hours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
		}
		GUI.Box(new Rect(0.075f * num, 0.2f * num2, 0.4f * num, 0.6f * num2), "Simple timer", box);
		GUI.Label(new Rect(0.075f * num, 0.3f * num2, 0.4f * num, 0.1f * num2), text, label);
		if (GUI.Button(new Rect(0.1f * num, 0.5f * num2, 0.35f * num, 0.1f * num2), "+60 seconds", button))
		{
			simpleTimerEndTimestamp = simpleTimerEndTimestamp.AddSeconds(60.0);
			WriteTimestamp("simpleTimer", simpleTimerEndTimestamp);
		}
		if (GUI.Button(new Rect(0.1f * num, 0.65f * num2, 0.35f * num, 0.1f * num2), "Reset", button))
		{
			simpleTimerEndTimestamp = DateTime.Now.AddSeconds(60.0);
			WriteTimestamp("simpleTimer", simpleTimerEndTimestamp);
		}
		string text2 = "END";
		if (timeSpan2.TotalSeconds > 0.0)
		{
			text2 = $"{timeSpan2.Hours}:{timeSpan2.Minutes:D2}:{timeSpan2.Seconds:D2}";
		}
		GUI.Box(text: (!UnbiasedTime.Instance.IsUsingSystemTime()) ? "Unbiased timer" : "Unbiased fallback", position: new Rect(0.525f * num, 0.2f * num2, 0.4f * num, 0.6f * num2), style: box);
		GUI.Label(new Rect(0.525f * num, 0.3f * num2, 0.4f * num, 0.1f * num2), text2, label);
		if (GUI.Button(new Rect(0.55f * num, 0.5f * num2, 0.35f * num, 0.1f * num2), "+60 seconds", button))
		{
			unbiasedTimerEndTimestamp = unbiasedTimerEndTimestamp.AddSeconds(60.0);
			WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
		}
		if (GUI.Button(new Rect(0.55f * num, 0.65f * num2, 0.35f * num, 0.1f * num2), "Reset", button))
		{
			unbiasedTimerEndTimestamp = UnbiasedTime.Instance.Now().AddSeconds(60.0);
			WriteTimestamp("unbiasedTimer", unbiasedTimerEndTimestamp);
		}
	}

	private DateTime ReadTimestamp(string key, DateTime defaultValue)
	{
		long num = Convert.ToInt64(PlayerPrefs.GetString(key, "0"));
		if (num == 0)
		{
			return defaultValue;
		}
		return DateTime.FromBinary(num);
	}

	private void WriteTimestamp(string key, DateTime time)
	{
		PlayerPrefs.SetString(key, time.ToBinary().ToString());
	}
}
