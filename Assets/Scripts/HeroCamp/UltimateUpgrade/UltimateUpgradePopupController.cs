using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp.UltimateUpgrade
{
	public class UltimateUpgradePopupController : GeneralPopupController
	{
		[SerializeField]
		private Image[] heroAvatars;

		[Space]
		[SerializeField]
		private Text petName;

		[SerializeField]
		private Text petDescription;

		[SerializeField]
		private Transform petAvatarHolder;

		[Space]
		[SerializeField]
		private PetActionAvatarGroupController petActionAvatarGroupController;

		[Space]
		[SerializeField]
		private UpgradeButtonController upgradeButtonController;

		[Space]
		[SerializeField]
		private GameObject effectUpgrade;

		private int heroID;

		private int petID;

		private const string PET_NAME_PREFIX = "PET_NAME_ID_{0}";

		public void Init()
		{
			Open();
			heroID = HeroCampPopupController.Instance.currentHeroID;
			petID = HeroParameter.Instance.GetPetID(heroID);
			upgradeButtonController.Init(this, heroID);
			InitHeroPetInformation();
			UpdateUpgradeButtonState();
		}

		private void InitHeroPetInformation()
		{
			Image[] array = heroAvatars;
			foreach (Image image in array)
			{
				image.sprite = Resources.Load<Sprite>($"HeroesAvatar/avatar_hero_{heroID}");
			}
			petActionAvatarGroupController.ShowSelectedPetActionAvatar(petID);
			petName.text = GameTools.GetLocalization($"PET_NAME_ID_{petID}");
			petDescription.text = GameTools.GetPetDescription(petID);
		}

		public void UpdateUpgradeButtonState()
		{
			if (ReadWriteDataHero.Instance.IsPetAvailable(heroID))
			{
				if (ReadWriteDataHero.Instance.IsPetUnlocked(heroID))
				{
					upgradeButtonController.Hide();
				}
				else
				{
					upgradeButtonController.Show();
				}
			}
			else
			{
				upgradeButtonController.Hide();
			}
		}

		public void CastEffectUpgrade()
		{
			effectUpgrade.SetActive(value: true);
			UISoundManager.Instance.PlayUpgradeSuccess();
		}

		public override void Open()
		{
			base.Open();
			effectUpgrade.SetActive(value: false);
		}

		public override void Close()
		{
			base.Close();
			effectUpgrade.SetActive(value: false);
		}
	}
}
