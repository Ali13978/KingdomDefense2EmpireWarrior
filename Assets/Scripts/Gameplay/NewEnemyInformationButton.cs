using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class NewEnemyInformationButton : ButtonController
	{
		[SerializeField]
		private Image avatar;

		private Sprite enemyFullAvatar;

		private int enemyId;

		public int EnemyId => enemyId;

		public void Init(int enemyID)
		{
			enemyId = enemyID;
			SetAvatar(enemyID);
		}

		private void SetAvatar(int enemyID)
		{
			enemyFullAvatar = Resources.Load<Sprite>($"Preview/Enemies/p_enemy_{enemyID}");
			avatar.sprite = enemyFullAvatar;
			avatar.SetNativeSize();
		}

		public void ShowButton(float buttonLifeTime)
		{
			base.gameObject.SetActive(value: true);
			CustomInvoke(HideButton, buttonLifeTime);
			SendEventShowButton();
			UISoundManager.Instance.PlayNewEnemyButton();
		}

		private void SendEventShowButton()
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_ShowTipsButton("New Enemy", EnemyId);
		}

		public override void OnClick()
		{
			base.OnClick();
			ShowCard();
			HideButton();
		}

		private void ShowCard()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.NewEnemyInformationPopup.Init(EnemyId);
		}

		private void HideButton()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
