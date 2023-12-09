using Data;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroSkill : ButtonController
	{
		[SerializeField]
		private int skillID;

		private int heroID;

		[SerializeField]
		private Image skillIcon;

		[SerializeField]
		private GameObject[] skillPoints;

		[Space]
		[SerializeField]
		private Animator animator;

		private HeroSkillGroupController heroSkillGroupController;

		public int SkillID
		{
			get
			{
				return skillID;
			}
			set
			{
				skillID = value;
			}
		}

		public int HeroID
		{
			get
			{
				return heroID;
			}
			set
			{
				heroID = value;
			}
		}

		public void Init(HeroSkillGroupController heroSkillGroupController, int heroID, int heroLevel)
		{
			this.heroSkillGroupController = heroSkillGroupController;
			HeroID = heroID;
			skillIcon.sprite = Resources.Load<Sprite>($"HeroCamp/SkillIcons/hero_{heroID}_skill_{SkillID}");
		}

		public void RefreshSkillPoint()
		{
			int skillPoint = ReadWriteDataHero.Instance.GetSkillPoint(HeroID, SkillID);
			for (int i = 0; i < skillPoints.Length; i++)
			{
				skillPoints[i].SetActive(i < skillPoint);
			}
		}

		public void PlayFXUpgrade()
		{
			animator.SetTrigger("CooldownDone");
		}

		public override void OnClick()
		{
			base.OnClick();
			heroSkillGroupController.SelectSkill(SkillID);
			heroSkillGroupController.ShowSelectedImage(base.transform);
		}
	}
}
