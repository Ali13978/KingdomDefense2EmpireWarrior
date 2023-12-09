using Data;
using Services.PlatformSpecific;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace UserProfile
{
	public class SelectRegionButtonController : ButtonController
	{
		public Image image;

		public string countryCode;

		private string filePath = string.Empty;

		public override void OnClick()
		{
			base.OnClick();
			ReadWriteDataUserProfile.Instance.SetRegionCode(countryCode);
			SingletonMonoBehaviour<UIRootController>.Instance.userProfilePopupController.ChangeRegionPopupController.CloseWithScaleAnimation();
			string userID = ReadWriteDataUserProfile.Instance.GetUserID();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add($"Tournament/Users/{userID}/country", countryCode);
			if (GameTools.tourUserSelfInfo != null)
			{
				dictionary.Add($"Tournament/Curseasongroups/{GameTools.tourUserSelfInfo.curgroupid}/{userID}/country", countryCode);
			}
			PlatformSpecificServicesProvider.Services.DataCloudSaver.UpdateData(dictionary);
		}

		internal void Init(string countryCode)
		{
			this.countryCode = countryCode;
			filePath = $"CountryFlags2/{countryCode}";
			Sprite sprite = Resources.Load<Sprite>(filePath);
			image.sprite = sprite;
		}
	}
}
