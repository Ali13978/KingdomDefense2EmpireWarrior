using Data;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class ResetButtonController : ButtonController
	{
		[SerializeField]
		private Button button;

		[SerializeField]
		private Image image;

		private HeroSkillGroupController heroSkillGroupController;

		private int heroID;

		public void Init(HeroSkillGroupController heroSkillGroupController, int heroID)
		{
			this.heroSkillGroupController = heroSkillGroupController;
			this.heroID = heroID;
			RefreshButtonStatus();
		}

		public override void OnClick()
		{
			base.OnClick();
			ResetSkillPoint();
		}

		private void ResetSkillPoint()
		{
			ReadWriteDataHero.Instance.ResetSkillPoint(heroID);
			heroSkillGroupController.RefreshSkillPoint();
			heroSkillGroupController.RefreshSkillInformation();
			ReadWriteDataHero.Instance.OnSkillPointChange(isDispatchEventChange: true);
		}

		private void RefreshButtonStatus()
		{
			if (ReadWriteDataHero.Instance.IsHeroOwned(heroID))
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
			image.color = Color.white;
		}

		private void ShowNonInteractable()
		{
			button.enabled = false;
			image.color = Color.gray;
		}
	}
}
