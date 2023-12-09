using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class SelectEnemyButtonController : ButtonController
	{
		private Button button;

		private Image miniAvatar;

		private int enemyID;

		[SerializeField]
		private GameObject lockImage;

		[SerializeField]
		private GameObject normalImage;

		[SerializeField]
		private Image enemyAvatar;

		[SerializeField]
		private RectTransform avatarRectTransform;

		private void Awake()
		{
			button = GetComponent<Button>();
			miniAvatar = GetComponent<Image>();
		}

		public void Init(int _enemyID)
		{
			enemyID = _enemyID;
			SetAvatar(_enemyID);
		}

		private void SetAvatar(int _enemyID)
		{
			enemyAvatar.sprite = Resources.Load<Sprite>($"Preview/Enemies/p_enemy_{_enemyID}");
			enemyAvatar.SetNativeSize();
			enemyAvatar.transform.localScale = Vector3.one;
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
			GuidePopupController.Instance.ShowSelectedEnemyImage(base.transform);
			GuidePopupController.Instance.GuideEnemyController.currentEnemyIDSelected = enemyID;
			GuidePopupController.Instance.GuideEnemyController.RefreshEnemyInformation();
		}
	}
}
