using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class ReadNotificationsDescription : ReadCommonDescription
	{
		public override void ReadParameter()
		{
			base.ReadParameter();
			string text = "Parameters/Description/notification_" + Config.Instance.LanguageID;
			try
			{
				Singleton<NotificationDescription>.Instance.ClearData();
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					Notification notiParameter = default(Notification);
					notiParameter.notiID = (int)list[i]["noti_id"];
					notiParameter.notiContent = (string)list[i]["noti_content"];
					Singleton<NotificationDescription>.Instance.SetNotiParameter(notiParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
		}
	}
}
