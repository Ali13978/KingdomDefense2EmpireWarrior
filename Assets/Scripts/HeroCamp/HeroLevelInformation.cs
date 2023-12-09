using Data;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroLevelInformation : CustomMonoBehaviour
	{
		[Space]
		[Header("Hero Level")]
		[SerializeField]
		private Text levelText;

		[Space]
		[Header("Hero Avatar")]
		[SerializeField]
		private Image heroAvatar;

		[SerializeField]
		private Image heroname;

		[Space]
		[Header("List Information Items ")]
		[SerializeField]
		private HeroInformationItem inforHealth;

		[SerializeField]
		private HeroInformationItem inforAttackDamage;

		[SerializeField]
		private HeroInformationItem inforArmor;

		[SerializeField]
		private HeroInformationItem inforSpeed;

		[Space]
		[Header("Exp bar")]
		[SerializeField]
		private RectTransform expBar;

		private Vector2 expBarSize;

		[SerializeField]
		private int minExpBarValue;

		[SerializeField]
		private int maxExpBarValue;

		private int currentHeroID;

		private int currentHeroLevel;

		public void Init(int heroID, int heroLevel)
		{
			currentHeroID = heroID;
			currentHeroLevel = heroLevel;
			InitHeroStat();
			InitHeroAvatar();
			InitExpBar();
		}

		private void InitHeroAvatar()
		{
			heroAvatar.sprite = Resources.Load<Sprite>($"HeroesAvatar/avatar_hero_{currentHeroID}");
			heroname.sprite = Resources.Load<Sprite>($"HeroesName/name_hero_{currentHeroID}");
		}

		private void InitHeroStat()
		{
			levelText.text = string.Empty + (currentHeroLevel + 1);
			inforHealth.Init(currentHeroID, currentHeroLevel);
			inforAttackDamage.Init(currentHeroID, currentHeroLevel);
			inforArmor.Init(currentHeroID, currentHeroLevel);
			inforSpeed.Init(currentHeroID, currentHeroLevel);
		}

		private void InitExpBar()
		{
			int currentExp = ReadWriteDataHero.Instance.GetCurrentExp(currentHeroID);
			if (currentHeroLevel < 9)
			{
				CalculateEXPBar(currentExp, HeroParameter.Instance.GetEXPForNextLevel(currentHeroID, currentHeroLevel));
			}
			if (currentHeroLevel == 9)
			{
				DisplayEXPBar(140f);
			}
		}

		public void DisplayLevelUpHero()
		{
			int num = ReadWriteDataHero.Instance.GetCurrentHeroLevel(currentHeroID);
			int currentExp = ReadWriteDataHero.Instance.GetCurrentExp(currentHeroID);
			UnityEngine.Debug.Log("current level = " + num + " exp dư ra = " + currentExp);
			if (num < 9)
			{
				CalculateEXPBar(currentExp, HeroParameter.Instance.GetEXPForNextLevel(currentHeroID, num));
			}
			if (num == 9)
			{
				DisplayEXPBar(140f);
			}
			HeroCampPopupController.Instance.RefreshHeroInformation();
			HeroCampPopupController.Instance.ShowLevelUpEffect();
		}

		public void CalculateEXPBar(int currentEXP, int currentLevelExp)
		{
			float num = (float)currentLevelExp / (float)(maxExpBarValue - minExpBarValue);
			float currentEXPLength = (float)currentEXP / num;
			DisplayEXPBar(currentEXPLength);
		}

		private void DisplayEXPBar(float currentEXPLength)
		{
			ref Vector2 reference = ref expBarSize;
			float newX = currentEXPLength + (float)minExpBarValue;
			Vector2 sizeDelta = expBar.sizeDelta;
			reference.Set(newX, sizeDelta.y);
			expBar.sizeDelta = expBarSize;
		}
	}
}
