using Data;
using Middle;
using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class NotificationEditor : MonoBehaviour, INotification
	{
		public void CancelAllNotify()
		{
			UnityEngine.Debug.Log("Cancel all notifications!");
		}

		public void PushNotify(string content, int delayTimeBySecond)
		{
			if (Config.Instance.AllowPushNoti)
			{
				string userRegionCode = ReadWriteDataUserProfile.Instance.GetUserRegionCode();
				UnityEngine.Debug.Log("User allow push notify: Editor Notify with country code: " + userRegionCode + " content: " + content + " delay time: " + delayTimeBySecond);
			}
			else
			{
				UnityEngine.Debug.Log("User not allow push notify");
			}
		}
	}
}
