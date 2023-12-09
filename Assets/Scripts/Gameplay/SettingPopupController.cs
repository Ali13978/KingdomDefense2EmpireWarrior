using DG.Tweening;
using Gameplay.Setting;
using Middle;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class SettingPopupController : GameplayPopupController
	{
		public UnityEvent OnOpen;

		public UnityEvent OnClose;

		[SerializeField]
		private GameObject settingPanel;

		[SerializeField]
		private ConfirmGroup confirmGroup;

		[SerializeField]
		private RestartButtonController restartButtonController;

		public void InitConfirmGroup(CancelGameType cancelGameType)
		{
			settingPanel.gameObject.SetActive(value: false);
			confirmGroup.gameObject.SetActive(value: true);
			confirmGroup.Init(cancelGameType);
		}

		public void CloseConfirmGroup()
		{
			confirmGroup.gameObject.SetActive(value: false);
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(value: true);
			settingPanel.gameObject.SetActive(value: true);
			CloseConfirmGroup();
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(settingPanel.transform.DOLocalMoveY(-10f, timeToOpen));
			sequence.OnComplete(LateAnimationOpen);
			if (ModeManager.Instance.gameMode == GameMode.TournamentMode)
			{
				restartButtonController.SetUnClickable();
			}
			else
			{
				restartButtonController.SetClickable();
			}
			GameplayManager.Instance.gameSpeedController.PauseGame();
		}

		private void LateAnimationOpen()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(settingPanel.transform.DOLocalMoveY(0f, 0.1f));
		}

		public override void Close()
		{
			base.Close();
			CloseConfirmGroup();
			base.transform.DOKill();
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(settingPanel.transform.DOLocalMoveY(800f, timeToClose)).OnComplete(LateAnimationClose);
		}

		private void LateAnimationClose()
		{
			base.gameObject.SetActive(value: false);
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
		}
	}
}
