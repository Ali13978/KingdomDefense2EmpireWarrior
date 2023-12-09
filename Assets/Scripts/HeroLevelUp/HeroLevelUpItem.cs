using Data;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroLevelUp
{
	public class HeroLevelUpItem : CustomMonoBehaviour
	{
		[Space]
		[Header("Hero information")]
		[SerializeField]
		private RectTransform expBar;

		private Vector2 expBarSize;

		[SerializeField]
		private int minExpBarValue;

		[SerializeField]
		private int maxExpBarValue;

		[Space]
		[SerializeField]
		private Text heroLevelText;

		[Space]
		[SerializeField]
		private Image avatar;

		[Space]
		[Header("Level up effect")]
		[SerializeField]
		private GameObject levelUpEffect;

		private int heroID;

		private int heroLevel;

		private int heroLevelBeforeCalculate;

		private int heroLevelAfterCalculate;

		private int heroExpBeforeCalculate;

		private int heroExpAfterCalculate;

		public void Init(int _heroID)
		{
			heroID = _heroID;
			SetHeroAvatar(_heroID);
			InitExpBar();
		}

		private void SetHeroAvatar(int _heroID)
		{
			avatar.sprite = Resources.Load<Sprite>($"HeroesAvatar/avatar_hero_{heroID}");
		}

		public void InitExpBar()
		{
			heroLevelBeforeCalculate = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			heroExpBeforeCalculate = ReadWriteDataHero.Instance.GetCurrentExp(heroID);
			heroLevelText.text = string.Empty + (heroLevelBeforeCalculate + 1);
			if (heroLevelBeforeCalculate < 9)
			{
				CalculateEXPBar(heroExpBeforeCalculate, HeroParameter.Instance.GetEXPForNextLevel(heroID, heroLevelBeforeCalculate));
			}
			if (heroLevelBeforeCalculate == 9)
			{
				DisplayEXPBar(140f);
			}
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
			float newX = Mathf.Min(maxExpBarValue, currentEXPLength + (float)minExpBarValue);
			Vector2 sizeDelta = expBar.sizeDelta;
			reference.Set(newX, sizeDelta.y);
			expBar.sizeDelta = expBarSize;
		}

		public void InitExpBarAfterCalculating()
		{
			heroLevelAfterCalculate = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			heroExpAfterCalculate = ReadWriteDataHero.Instance.GetCurrentExp(heroID);
			heroLevelText.text = string.Empty + (heroLevelAfterCalculate + 1);
			if (heroLevelAfterCalculate < 9)
			{
				CalculateEXPBar(heroExpAfterCalculate, HeroParameter.Instance.GetEXPForNextLevel(heroID, heroLevelAfterCalculate));
			}
			if (heroLevelAfterCalculate == 9)
			{
				DisplayEXPBar(140f);
			}
			if (heroLevelAfterCalculate > heroLevelBeforeCalculate)
			{
				levelUpEffect.SetActive(value: true);
			}
		}

		private void ExpBarEffect()
		{
		}
	}
}
