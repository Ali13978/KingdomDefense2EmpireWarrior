using Data;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class TrainButtonController : ButtonController
	{
		[SerializeField]
		private Button button;

		[SerializeField]
		private Image image;

		[SerializeField]
		private Sprite interacableSprite;

		[SerializeField]
		private Sprite nonInteracableSprite;

		private HeroSkillGroupController heroSkillGroupController;

		private int heroID;

		private int skillID;

		public void Init(HeroSkillGroupController heroSkillGroupController, int heroID, int skillID)
		{
			this.heroSkillGroupController = heroSkillGroupController;
			this.heroID = heroID;
			this.skillID = skillID;
			RefreshButtonStatus();
		}

		public override void OnClick()
		{
			base.OnClick();
			TrainSkill();
		}

		private void TrainSkill()
		{
			ReadWriteDataHero.Instance.IncreaseSkillLevel(heroID, skillID);
			heroSkillGroupController.RefreshSkillPoint();
			heroSkillGroupController.CastUpgradeSkillFX(heroID, skillID);
			heroSkillGroupController.RefreshSkillInformation();
			RefreshButtonStatus();
			UISoundManager.Instance.PlayUpgradeSuccess();
			ReadWriteDataHero.Instance.OnSkillPointChange(isDispatchEventChange: true);
		}

		public void RefreshButtonStatus()
		{
			if (ReadWriteDataHero.Instance.GetCurrentSkillPoint(heroID) >= 1 && !ReadWriteDataHero.Instance.IsMaxSkill(heroID, skillID))
			{
				ShowInteractable();
			}
			else
			{
				ShowNonInteractable();
			}
		}

		private void ShowInteractable()
		{
			button.enabled = true;
			image.sprite = interacableSprite;
		}

		private void ShowNonInteractable()
		{
			button.enabled = false;
			image.sprite = nonInteracableSprite;
		}
	}
}
