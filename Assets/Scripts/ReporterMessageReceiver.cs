using UnityEngine;

public class ReporterMessageReceiver : MonoBehaviour
{
	private Reporter reporter;

	private void Start()
	{
		reporter = base.gameObject.GetComponent<Reporter>();
	}

	private void OnPreStart()
	{
		if (reporter == null)
		{
			reporter = base.gameObject.GetComponent<Reporter>();
		}
		if (Screen.width < 1000)
		{
			reporter.size = new Vector2(32f, 32f);
		}
		else
		{
			reporter.size = new Vector2(48f, 48f);
		}
		reporter.UserData = "Put user date here like his account to know which user is playing on this device";
	}

	private void OnHideReporter()
	{
	}

	private void OnShowReporter()
	{
	}

	private void OnLog(Reporter.Log log)
	{
	}
}
