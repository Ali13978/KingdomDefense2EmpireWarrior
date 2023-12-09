using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class BuyTowerPopupController : GameplayPopupController
	{
		private Transform target;

		private Vector3 outSide = new Vector3(2000f, 2000f, 0f);

		[Space]
		[Header("Content Holder")]
		[SerializeField]
		private GameObject Content;

		[Space]
		[Header("Controll tower group")]
		public GroupControllTowerButtonsForBuy groupControllTowerButtons;

		[Space]
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private RectTransform canvasHolder;

		private GameObject towerControllerCollider;

		public override void Update()
		{
			base.Update();
			if (base.gameObject.activeSelf && (bool)target)
			{
				UpdatePositionFollowBuildRegion();
			}
		}

		public void Init(Transform target)
		{
			this.target = target;
			Open();
			groupControllTowerButtons.InitButtonsStatusByWave();
			TryToFocusTowerPosition();
		}

		private void TryToFocusTowerPosition()
		{
			if (base.gameObject.activeSelf && (bool)target)
			{
				UpdatePositionFollowBuildRegion();
			}
			Vector3 localPosition = rectTransform.localPosition;
			float y = localPosition.y;
			Vector2 sizeDelta = canvasHolder.sizeDelta;
			float num = sizeDelta.y / 2f;
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			if (!(y > num - sizeDelta2.y / 2f))
			{
				Vector3 localPosition2 = rectTransform.localPosition;
				float y2 = localPosition2.y;
				Vector2 sizeDelta3 = canvasHolder.sizeDelta;
				float num2 = (0f - sizeDelta3.y) / 2f;
				Vector2 sizeDelta4 = rectTransform.sizeDelta;
				if (!(y2 < num2 + sizeDelta4.y / 2f))
				{
					return;
				}
			}
			if ((bool)target)
			{
				SingletonMonoBehaviour<CameraController>.Instance.PinchZoomFov.TryToMoveToBuildTowerPosition(target.position);
			}
		}

		private void UpdatePositionFollowBuildRegion()
		{
			base.transform.position = target.position;
		}

		protected override void OnClickOutsideUp()
		{
			base.OnClickOutsideUp();
			Close();
		}

		public override void Open()
		{
			base.Open();
			if (SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.isOpen)
			{
				SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.Close();
			}
			base.gameObject.SetActive(value: true);
			groupControllTowerButtons.DisableConfirmAllButton();
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Close();
			GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>().HideRange();
			tween.Kill();
			Content.transform.localScale = 0.5f * Vector3.one;
			tween = Content.transform.DOScale(1f, timeToOpen).SetEase(Ease.OutBack).OnComplete(OnOpenComplete);
			if (towerControllerCollider == null)
			{
				towerControllerCollider = (Object.Instantiate(Resources.Load("UI Gameplay/Popups/TowerControllerCollider")) as GameObject);
			}
			Vector3 position = target.position;
			position.z = -0.5f;
			towerControllerCollider.transform.position = position;
			towerControllerCollider.SetActive(value: true);
		}

		private void OnOpenComplete()
		{
		}

		public override void Close()
		{
			base.Close();
			target = null;
			groupControllTowerButtons.DisableConfirmAllButton();
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Close();
			if (!SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.isOpen)
			{
				GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>().HideRange();
			}
			tween.Kill();
			tween = Content.transform.DOScale(0.1f, timeToClose).SetEase(Ease.InBack).OnComplete(OnCloseComplete);
			if (towerControllerCollider != null)
			{
				towerControllerCollider.SetActive(value: false);
			}
		}

		private void OnCloseComplete()
		{
			base.transform.position = outSide;
			base.gameObject.SetActive(value: false);
		}
	}
}
