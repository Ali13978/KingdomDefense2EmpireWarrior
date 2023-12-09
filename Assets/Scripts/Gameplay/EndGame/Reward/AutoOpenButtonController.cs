using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame.Reward
{
	public class AutoOpenButtonController : ButtonController
	{
		[SerializeField]
		private ChestGroupController chestGroupController;

		[SerializeField]
		private DOTweenAnimation tweenAnimation;

		private Button button;

		private void Awake()
		{
			GetAllComponents();
		}

		private void Update()
		{
			if (SingletonMonoBehaviour<GameData>.Instance.isAvailableOpenChestTurn())
			{
				tweenAnimation.DOPlay();
			}
			else
			{
				tweenAnimation.DOPause();
			}
		}

		private void GetAllComponents()
		{
			button = GetComponent<Button>();
		}

		public override void OnClick()
		{
			base.OnClick();
			chestGroupController.AutoOpenChest();
			UpdateState();
			SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.EndGameRewardPopupController.UpdateContinueButtonStatus();
		}

		private void UpdateState()
		{
			if (chestGroupController.isAvailableChestToOpen())
			{
				button.interactable = true;
			}
			else
			{
				button.interactable = false;
			}
		}
	}
}
