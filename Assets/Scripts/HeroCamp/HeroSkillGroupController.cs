using Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroSkillGroupController : MonoBehaviour
	{
		[Space]
		[Header("Skill Point")]
		[SerializeField]
		private Text currentSkillPoint;

		[Space]
		[Header("List Ability Items ")]
		[SerializeField]
		private List<HeroSkill> listAbilityItem;

		[Space]
		[SerializeField]
		private HeroSkillInformation heroSkillInformation;

		[Space]
		[SerializeField]
		private SelectedImageController selectedImage;

		[Space]
		[SerializeField]
		private TrainButtonController trainButtonController;

		[SerializeField]
		private ResetButtonController resetButtonController;

		private int heroID;

		private int skillID;

		public HeroSkillInformation HeroSkillInformation
		{
			get
			{
				return heroSkillInformation;
			}
			set
			{
				heroSkillInformation = value;
			}
		}

		public void Init(int heroID, int currentLevel)
		{
			this.heroID = heroID;
			for (int i = 0; i < listAbilityItem.Count; i++)
			{
				listAbilityItem[i].Init(this, heroID, currentLevel);
			}
			listAbilityItem[0].OnClick();
			RefreshSkillPoint();
		}

		public void RefreshSkillPoint()
		{
			currentSkillPoint.text = ReadWriteDataHero.Instance.GetCurrentSkillPoint(heroID).ToString();
			for (int i = 0; i < listAbilityItem.Count; i++)
			{
				listAbilityItem[i].RefreshSkillPoint();
			}
			trainButtonController.RefreshButtonStatus();
		}

		public void RefreshSkillInformation()
		{
			HeroSkillInformation.Init(heroID, skillID);
		}

		public void SelectSkill(int skillID)
		{
			this.skillID = skillID;
			trainButtonController.Init(this, heroID, skillID);
			resetButtonController.Init(this, heroID);
			HeroSkillInformation.Init(heroID, skillID);
		}

		public void CastUpgradeSkillFX(int heroID, int skillID)
		{
			foreach (HeroSkill item in listAbilityItem)
			{
				if (item.HeroID == heroID && item.SkillID == skillID)
				{
					item.PlayFXUpgrade();
				}
			}
		}

		public void ShowSelectedImage(Transform transform)
		{
			selectedImage.Init(transform);
		}
	}
}
