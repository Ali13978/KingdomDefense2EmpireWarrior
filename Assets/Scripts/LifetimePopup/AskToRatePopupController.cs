using ApplicationEntry;
using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace LifetimePopup
{
	public class AskToRatePopupController : GameplayPriorityPopupController
	{
		[SerializeField]
		private GameObject[] starGroup;

		[SerializeField]
		private Slider starSlider;

		public override void InitPriority(PopupPriorityEnum priority)
		{
			base.InitPriority(priority);
			starSlider.value = 0f;
			GameObject[] array = starGroup;
			foreach (GameObject gameObject in array)
			{
				gameObject.SetActive(value: false);
			}
		}

		public void UpdateStarSliderValue()
		{
			int num = (int)starSlider.value;
			if (num >= 1)
			{
				for (int i = 0; i < starGroup.Length; i++)
				{
					if (i <= num - 1)
					{
						starGroup[i].SetActive(value: true);
					}
					else
					{
						starGroup[i].SetActive(value: false);
					}
				}
			}
			else
			{
				GameObject[] array = starGroup;
				foreach (GameObject gameObject in array)
				{
					gameObject.SetActive(value: false);
				}
			}
		}

		public void Rate()
		{
			ReadWriteData.Instance.SetDataRated();
			CloseWithScaleAnimation();
			if (starSlider.value >= 4f)
			{
				Application.OpenURL(MarketingConfig.rateGameLink);
				return;
			}
			string ratingBehavior = ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.GetRatingBehavior();
			if (ratingBehavior.Equals(RatingBehavior.thanknhide.ToString()))
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(114);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
			if (ratingBehavior.Equals(RatingBehavior.tostore.ToString()))
			{
				Application.OpenURL(MarketingConfig.rateGameLink);
			}
		}

		public void SendFeedback()
		{
			string companyEmail = MarketingConfig.companyEmail;
			string text = MyEscapeURL(MarketingConfig.feedbackTitle);
			string text2 = MyEscapeURL("Please Enter your message here");
			CloseWithScaleAnimation();
			Application.OpenURL("mailto:" + companyEmail + "?subject=" + text + "&body=" + text2);
			ReadWriteData.Instance.SetDataRated();
		}

		private string MyEscapeURL(string url)
		{
			return WWW.EscapeURL(url).Replace("+", "%20");
		}

		public override void Open()
		{
			base.Open();
		}

		public override void Close()
		{
			base.Close();
		}
	}
}
