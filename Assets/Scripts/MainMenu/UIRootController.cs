using UnityEngine;

namespace MainMenu
{
	public class UIRootController : SingletonMonoBehaviour<UIRootController>
	{
		[Space]
		[Header("Prefab Panels")]
		[SerializeField]
		private GameObject languageChoosePrefab;

		[SerializeField]
		private GameObject creditPrefab;

		[SerializeField]
		private GameObject policyPrefab;

		[SerializeField]
		private GameObject askToQuitPrefab;

		[Header("Button Language")]
		[SerializeField]
		private LanguageButtonController languageButtonController;

		private bool isLanguageChoosePopupExist;

		private LanguageChoosePanelController _languageChoosePanelController;

		private bool isCreditPopupExist;

		private CreditPopupController _creditPopupController;

		private bool isPolicyPopupExist;

		private PolicyPopupController _policyPopupController;

		private bool isAskToQuitPopupExist;

		private AskToQuitPopupController _askToQuitPopupController;

		public LanguageButtonController LanguageButtonController
		{
			get
			{
				return languageButtonController;
			}
			set
			{
				languageButtonController = value;
			}
		}

		public LanguageChoosePanelController languageChoosePanelController
		{
			get
			{
				if (_languageChoosePanelController == null)
				{
					GameObject gameObject = Object.Instantiate(languageChoosePrefab);
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_languageChoosePanelController = gameObject.GetComponent<LanguageChoosePanelController>();
					isLanguageChoosePopupExist = true;
				}
				return _languageChoosePanelController;
			}
			set
			{
				_languageChoosePanelController = value;
			}
		}

		public CreditPopupController creditPopupController
		{
			get
			{
				if (_creditPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(creditPrefab);
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_creditPopupController = gameObject.GetComponent<CreditPopupController>();
					isCreditPopupExist = true;
				}
				return _creditPopupController;
			}
			set
			{
				_creditPopupController = value;
			}
		}

		public PolicyPopupController policyPopupController
		{
			get
			{
				if (_policyPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(policyPrefab);
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_policyPopupController = gameObject.GetComponent<PolicyPopupController>();
					isPolicyPopupExist = true;
				}
				return _policyPopupController;
			}
			set
			{
				_policyPopupController = value;
			}
		}

		public AskToQuitPopupController askToQuitPopupController
		{
			get
			{
				if (_askToQuitPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(askToQuitPrefab);
					gameObject.transform.SetParent(base.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_askToQuitPopupController = gameObject.GetComponent<AskToQuitPopupController>();
					isAskToQuitPopupExist = true;
				}
				return _askToQuitPopupController;
			}
			set
			{
				_askToQuitPopupController = value;
			}
		}

		private void Update()
		{
			UpdateKeyBack();
		}

		private void UpdateKeyBack()
		{
			if (UnityEngine.Input.GetKeyUp(KeyCode.Escape))
			{
				if (isLanguageChoosePopupExist && languageChoosePanelController.isOpen)
				{
					languageChoosePanelController.Close();
				}
				else if (isPolicyPopupExist && policyPopupController.isOpen)
				{
					policyPopupController.Close();
				}
				else if (isCreditPopupExist && creditPopupController.isOpen)
				{
					creditPopupController.Close();
				}
				else if (!isAskToQuitPopupExist || !askToQuitPopupController.isOpen)
				{
					askToQuitPopupController.Init();
				}
				else if (isAskToQuitPopupExist && askToQuitPopupController.isOpen)
				{
					askToQuitPopupController.Close();
				}
			}
		}
	}
}
