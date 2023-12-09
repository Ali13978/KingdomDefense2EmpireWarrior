using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class NewEnemyInformationPopup : GameplayPopupController
	{
		[Space]
		[SerializeField]
		private Image imageAvatar;

		[SerializeField]
		private Text enemyName;

		[SerializeField]
		private Text enemyDescription;

		[SerializeField]
		private Text enemySpecialAbility;

		private int enemyID;

		public void Init(int enemyID)
		{
			this.enemyID = enemyID;
			base.OpenWithScaleAnimation();
			GameplayManager.Instance.gameSpeedController.PauseGame();
			imageAvatar.sprite = Resources.Load<Sprite>($"Preview/Enemies/FullAvatars/fa_enemy_{enemyID}");
			enemyName.text = Singleton<EnemyDescription>.Instance.GetEnemyName(enemyID);
			enemyDescription.text = Singleton<EnemyDescription>.Instance.GetEnemyDescription(enemyID).Replace('@', '\n').Replace('#', '-');
			enemySpecialAbility.text = Singleton<EnemyDescription>.Instance.GetEnemySpecialAbility(enemyID).Replace('@', '\n').Replace('#', '-');
			SendEventOpenPopup();
		}

		private void SendEventOpenPopup()
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_OpenTipsPopup("New Enemy", enemyID);
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
		}
	}
}
