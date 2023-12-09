using System;
using System.Collections;
using System.Text;
using UnityEngine;

namespace GiftcodeSystem
{
	public class GiftCodeManager : MonoBehaviour
	{
		private const string GIFTCODE_ENABLE = "giftcode_enable";

		public static bool isGiftCodeEnable;

		private const string urlAPI = "http://api.zonmob.com/v1/giftcode";

		private const string product_uuid = "510e1472-7cde-47dd-8b16-64c8aa4c645d";

		public event Action<ReceivedGiftCodeMessage> onGiftCodeSubmited;

		private void Awake()
		{
		}

		public void SubmitGiftCode(string giftCode, string uuid)
		{
			SubmitGiftCode submitGiftCode = default(SubmitGiftCode);
			submitGiftCode.product_uuid = "510e1472-7cde-47dd-8b16-64c8aa4c645d";
			submitGiftCode.giftcode = giftCode;
			submitGiftCode.user_id = uuid;
			string text = JsonUtility.ToJson(submitGiftCode);
			UnityEngine.Debug.Log(text);
			StartCoroutine(DoSubmitGiftCode(text));
		}

		private IEnumerator DoSubmitGiftCode(string data)
		{
			WWW www = new WWW("http://api.zonmob.com/v1/giftcode", Encoding.UTF8.GetBytes(data));
			yield return www;
			ReceivedGiftCodeMessage receivedData = new ReceivedGiftCodeMessage();
			if (www.error != null)
			{
				receivedData.status = 0;
				receivedData.message = www.error;
			}
			else if (www.isDone)
			{
				string text = www.text;
				receivedData = JsonUtility.FromJson<ReceivedGiftCodeMessage>(text);
			}
			if (this.onGiftCodeSubmited != null)
			{
				this.onGiftCodeSubmited(receivedData);
			}
		}
	}
}
