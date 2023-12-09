using Middle;
using UnityEngine;

namespace MainMenu
{
	public class PolicyPopupController : MonoBehaviour
	{
		[HideInInspector]
		public bool isOpen;

		[SerializeField]
		private GameObject policy_general;

		[SerializeField]
		private GameObject policy_korean;

		[SerializeField]
		private GameObject policy_japanese;

		public void Init()
		{
			Open();
			InitPolicyContent();
		}

		private void InitPolicyContent()
		{
			string languageID = Config.Instance.LanguageID;
			policy_general.SetActive(value: false);
			policy_korean.SetActive(value: false);
			policy_japanese.SetActive(value: false);
			if (languageID.Equals("lg_korean"))
			{
				policy_korean.SetActive(value: true);
			}
			else if (languageID.Equals("lg_japanese"))
			{
				policy_japanese.SetActive(value: true);
			}
			else
			{
				policy_general.SetActive(value: true);
			}
		}

		public void Open()
		{
			base.gameObject.SetActive(value: true);
			isOpen = true;
		}

		public void Close()
		{
			base.gameObject.SetActive(value: false);
			isOpen = false;
		}
	}
}
