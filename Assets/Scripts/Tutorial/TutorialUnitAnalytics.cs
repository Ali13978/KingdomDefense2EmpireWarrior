using Services.PlatformSpecific;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUnitAnalytics : MonoBehaviour
	{
		public void SendEventDoneTutorial(int tutorialStep)
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_DoneTutorial(tutorialStep);
		}
	}
}
