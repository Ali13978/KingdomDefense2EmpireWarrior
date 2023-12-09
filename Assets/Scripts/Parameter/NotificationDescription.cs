using System.Collections.Generic;

namespace Parameter
{
	public class NotificationDescription : Singleton<NotificationDescription>
	{
		private List<Notification> listNoti = new List<Notification>();

		private bool CheckId(int tipID)
		{
			return tipID >= 0 && tipID < listNoti.Count;
		}

		public void ClearData()
		{
			listNoti.Clear();
		}

		public void SetNotiParameter(Notification noti)
		{
			int count = listNoti.Count;
			if (count <= noti.notiID)
			{
				listNoti.Add(noti);
			}
		}

		public string GetNotiContent(int notiID)
		{
			if (notiID < listNoti.Count && notiID >= 0)
			{
				Notification notification = listNoti[notiID];
				return notification.notiContent;
			}
			return "--";
		}
	}
}
