using Data;
using Notify;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class SelectHeroButtonController : ButtonController
	{
		[SerializeField]
		private int heroID;

		[SerializeField]
		private Text heroLevel;

		[SerializeField]
		private GeneralNotify generalNotify;

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

		public void UpdateNotifyHeroSkill()
		{
			if (ReadWriteDataHero.Instance.IsHeroOwned(HeroID))
			{
				generalNotify.TryShowNotify(ReadWriteDataHero.Instance.GetCurrentSkillPoint(HeroID) >= 1);
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			HeroCampPopupController.Instance.currentHeroID = HeroID;
			HeroCampPopupController.Instance.ShowSelectedHeroImage(base.transform);
			HeroCampPopupController.Instance.RefreshHeroInformation();
		}

		public void UpdateHeroLevel()
		{
			if ((bool)heroLevel)
			{
				heroLevel.text = (ReadWriteDataHero.Instance.GetCurrentHeroLevel(HeroID) + 1).ToString();
			}
		}
	}
}
