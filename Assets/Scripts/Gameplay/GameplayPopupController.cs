using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class GameplayPopupController : PopupController
	{
		[Space]
		[Header("UI Visual Effect")]
		[SerializeField]
		private GameObject contentHolder;

		[SerializeField]
		private float scaleOpen = 1.1f;

		[SerializeField]
		private float scaleNormal = 1f;

		[SerializeField]
		private GameObject buttonScalerHolder;

		[SerializeField]
		private GameObject titleScaleHolder;

		public virtual void DefaultInit()
		{
		}

		public override void Open()
		{
			base.Open();
			SingletonMonoBehaviour<GameData>.Instance.IsAnyPopupOpen = true;
			UISoundManager.Instance.PlayOpenPopup();
		}

		public override void Close()
		{
			base.Close();
			SingletonMonoBehaviour<GameData>.Instance.IsAnyPopupOpen = false;
			UISoundManager.Instance.PlayClosePopup();
		}

		public virtual void OpenWithScaleAnimation()
		{
			base.Open();
			base.gameObject.SetActive(value: true);
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			if ((bool)contentHolder)
			{
				contentHolder.transform.localScale = Vector3.zero;
				sequence.Append(contentHolder.transform.DOScale(scaleOpen, timeToOpen));
				sequence.OnComplete(OnOpenWithScaleAnimComplete);
			}
			SingletonMonoBehaviour<GameData>.Instance.IsAnyPopupOpen = true;
			UISoundManager.Instance.PlayOpenPopup();
			if ((bool)buttonScalerHolder)
			{
				ScaleButton();
			}
			if ((bool)titleScaleHolder)
			{
				ScaleTitle();
			}
		}

		private void OnOpenWithScaleAnimComplete()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(contentHolder.transform.DOScale(scaleNormal, 0.2f));
		}

		public virtual void CloseWithScaleAnimation()
		{
			base.Close();
			base.transform.DOKill();
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			if ((bool)contentHolder)
			{
				sequence.Append(contentHolder.transform.DOScale(0f, timeToClose));
			}
			sequence.OnComplete(OnCloseWithScaleAnimComplete);
		}

		private void OnCloseWithScaleAnimComplete()
		{
			base.gameObject.SetActive(value: false);
			SingletonMonoBehaviour<GameData>.Instance.IsAnyPopupOpen = false;
			UISoundManager.Instance.PlayClosePopup();
			OnCloseAnimationComplete();
		}

		public virtual void OnCloseAnimationComplete()
		{
		}

		public virtual void CloseContentHolder()
		{
			contentHolder.SetActive(value: false);
		}

		private void ScaleButton()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			buttonScalerHolder.transform.localScale = Vector3.zero;
			sequence.AppendInterval(timeToOpen / 1.5f);
			sequence.Append(buttonScalerHolder.transform.DOScale(1.3f, 0.25f));
			sequence.OnComplete(OnScaleButtonComplete);
		}

		private void OnScaleButtonComplete()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(buttonScalerHolder.transform.DOScale(1f, 0.15f));
		}

		private void ScaleTitle()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			titleScaleHolder.transform.localScale = Vector3.zero;
			sequence.AppendInterval(timeToOpen);
			sequence.Append(titleScaleHolder.transform.DOScale(1.5f, 0.25f));
			sequence.OnComplete(OnScaleTitleComplete);
		}

		private void OnScaleTitleComplete()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(titleScaleHolder.transform.DOScale(1f, 0.15f));
		}
	}
}
