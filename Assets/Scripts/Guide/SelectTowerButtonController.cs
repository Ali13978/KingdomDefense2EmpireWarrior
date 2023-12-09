using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class SelectTowerButtonController : ButtonController
	{
		private Button button;

		private int towerID;

		private int towerLevel;

		[SerializeField]
		private GameObject lockImage;

		[SerializeField]
		private GameObject normalImage;

		[SerializeField]
		private Image towerAvatar;

		[SerializeField]
		private RectTransform avatarRectTransform;

		private Vector3 imageSize = new Vector3(0.6f, 0.6f, 0.6f);

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		public void Init(int _towerID, int _towerLevel)
		{
			towerID = _towerID;
			towerLevel = _towerLevel;
			SetAvatar(_towerID, _towerLevel);
		}

		private void SetAvatar(int _towerID, int _towerLevel)
		{
			towerAvatar.sprite = Resources.Load<Sprite>($"Preview/Towers/p_tower_{_towerID}_{_towerLevel}");
			towerAvatar.SetNativeSize();
			towerAvatar.transform.localScale = imageSize;
		}

		public void SetLock()
		{
			button.enabled = false;
			lockImage.SetActive(value: true);
			normalImage.SetActive(value: false);
		}

		public void SetUnLock()
		{
			button.enabled = true;
			normalImage.SetActive(value: true);
			lockImage.SetActive(value: false);
		}

		public override void OnClick()
		{
			base.OnClick();
			GuidePopupController.Instance.ShowSelectedTowerImage(base.transform);
			GuidePopupController.Instance.GuideTowerController.currentTowerIDSelected = towerID;
			GuidePopupController.Instance.GuideTowerController.currentTowerLvSelected = towerLevel;
			GuidePopupController.Instance.GuideTowerController.RefreshTowerInformation();
		}
	}
}
