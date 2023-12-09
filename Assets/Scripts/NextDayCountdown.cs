using System;
using UnityEngine;
using UnityEngine.UI;

public class NextDayCountdown : MonoBehaviour
{
	[SerializeField]
	private Text label;

	[SerializeField]
	private string textFormat = "{0:00} : {1:00} : {2:00}";

	private DateTime nextDay;

	public void OnEnable()
	{
		nextDay = GetNextDay();
	}

	public void Update()
	{
		TimeSpan timeSpan = nextDay - DateTime.Now;
		if (timeSpan.Ticks < 0)
		{
			nextDay = GetNextDay();
			timeSpan = nextDay - DateTime.Now;
		}
		label.text = string.Format(textFormat, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
	}

	private DateTime GetNextDay()
	{
		return DateTime.Today.AddDays(1.0);
	}
}
