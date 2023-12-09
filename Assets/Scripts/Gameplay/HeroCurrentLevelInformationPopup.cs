using Data;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class HeroCurrentLevelInformationPopup : CustomMonoBehaviour
	{
		[SerializeField]
		private Text currentHealth;

		[SerializeField]
		private Text currentDamage;

		[SerializeField]
		private Text currentArmor;

		[SerializeField]
		private Text currentAttackRange;

		[SerializeField]
		private Text currentAttackSpeed;

		private int heroID;

		private int heroLevel;

		[Space]
		[SerializeField]
		private Image iconDamage;

		[SerializeField]
		private Sprite physicsDamageIcon;

		[SerializeField]
		private Sprite magicDamageIcon;

		public void Init(int heroID, int heroLevel)
		{
			Open();
			this.heroID = heroID;
			this.heroLevel = heroLevel;
			UpdateDamage();
			UpdateUnitHealth();
			UpdateUnitArmor();
			UpdateAttackRange();
			UpdateAttackSpeed();
		}

		private void UpdateAttackSpeed()
		{
			currentAttackSpeed.text = UnitAbilitiesRanking.Instance.GetAttackSpeedDescriptionByValue(HeroParameter.Instance.GetHeroAttackCooldown(heroID, heroLevel));
		}

		private void UpdateAttackRange()
		{
			currentAttackRange.text = UnitAbilitiesRanking.Instance.GetAttackRangeDescriptionByValue(HeroParameter.Instance.GetHeroAttackRange(heroID, heroLevel));
		}

		private void UpdateUnitHealth()
		{
			HeroModel hero = HeroesManager.Instance.GetHero(heroID);
			currentHealth.text = hero.HeroHealthController.OriginHealth.ToString();
		}

		private void UpdateUnitArmor()
		{
			HeroModel hero = HeroesManager.Instance.GetHero(heroID);
			currentArmor.text = UnitAbilitiesRanking.Instance.GetArmorDescriptionByValue((int)(hero.HeroHealthController.CurrentPhysicsArmor * 100f));
		}

		private void UpdateDamage()
		{
			HeroModel hero = HeroesManager.Instance.GetHero(heroID);
			if (HeroParameter.Instance.IsPhysicsAttack(heroID))
			{
				iconDamage.sprite = physicsDamageIcon;
				currentDamage.text = $"{hero.HeroAttackController.GetAtkPhysicsMin()} - {hero.HeroAttackController.GetAtkPhysicsMax()}";
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
				currentDamage.text = $"{hero.HeroAttackController.GetAtkMagicMin()} - {hero.HeroAttackController.GetAtkMagicMax()}";
			}
		}

		public void Open()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Close()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
