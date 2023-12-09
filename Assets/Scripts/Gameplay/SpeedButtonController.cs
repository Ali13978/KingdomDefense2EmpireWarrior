using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class SpeedButtonController : ButtonController
	{
		[SerializeField]
		private Sprite spriteNormal;

		[SerializeField]
		private Sprite spriteSpeed;

		private Image buttonImage;

		private void Awake()
		{
			buttonImage = GetComponent<Image>();
		}

		public void Init(float gameSpeed)
		{
			if (gameSpeed > 1f)
			{
				SetSpeedView();
			}
			else
			{
				SetNormalView();
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			if (!SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				if (GameplayManager.Instance.gameSpeedController.GameSpeed > 1f)
				{
					GameplayManager.Instance.gameSpeedController.SetSpeed(1f);
					SetNormalView();
				}
				else
				{
					GameplayManager.Instance.gameSpeedController.SetSpeed(2f);
					SetSpeedView();
				}
			}
		}

		private void SetNormalView()
		{
			buttonImage.sprite = spriteNormal;
		}

		private void SetSpeedView()
		{
			buttonImage.sprite = spriteSpeed;
		}
	}
}
