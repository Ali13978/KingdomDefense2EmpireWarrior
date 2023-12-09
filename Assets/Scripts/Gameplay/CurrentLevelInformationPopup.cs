using Data;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class CurrentLevelInformationPopup : CustomMonoBehaviour
	{
		private TowerModel towerModel;

		[Header("UI Text")]
		[SerializeField]
		private Text nameText;

		[Header("UI Text")]
		[SerializeField]
		private Text typeText;

		[Header("UI Text")]
		[SerializeField]
		private Text damageText;

		[Header("UI Text")]
		[SerializeField]
		private Text healthText;

		[Header("UI Text")]
		[SerializeField]
		private Text armorText;

		[Header("UI Text")]
		[SerializeField]
		private Text reloadText;

		[Header("UI Text")]
		[SerializeField]
		private Text attackRangeText;

		[Header("UI Text")]
		[SerializeField]
		private Text goldProduceText;

		[Header("UI Text")]
		[SerializeField]
		private Text timeProduceText;

		[Header("UI Text")]
		[SerializeField]
		private Text autoCollectGoldText;

		[Header("Holder")]
		[SerializeField]
		private GameObject damageHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject healthHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject armorHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject reloadHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject attackRangeHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject goldProduceHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject timeProduceHolder;

		[Header("Holder")]
		[SerializeField]
		private GameObject autoCollectGoldHolder;

		[Space]
		[SerializeField]
		private Image iconDamage;

		[SerializeField]
		private Sprite physicsDamageIcon;

		[SerializeField]
		private Sprite magicDamageIcon;

		private int currentLevelDamage_min;

		private int currentLevelDamage_max;

		public void Init()
		{
			Open();
			if ((bool)SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.towerModel)
			{
				towerModel = SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.towerModel;
			}
			nameText.text = Singleton<TowerDescription>.Instance.GetTowerName(towerModel.Id);
			typeText.text = Singleton<TowerDescription>.Instance.GetTowerType(towerModel.Id);
			Tower originalParameter = towerModel.OriginalParameter;
			int num;
			if (originalParameter.damage_Physics_min > 0)
			{
				Tower originalParameter2 = towerModel.OriginalParameter;
				num = originalParameter2.damage_Physics_min;
			}
			else
			{
				Tower originalParameter3 = towerModel.OriginalParameter;
				num = originalParameter3.damage_Magic_min;
			}
			currentLevelDamage_min = num;
			Tower originalParameter4 = towerModel.OriginalParameter;
			int num2;
			if (originalParameter4.damage_Physics_max > 0)
			{
				Tower originalParameter5 = towerModel.OriginalParameter;
				num2 = originalParameter5.damage_Physics_max;
			}
			else
			{
				Tower originalParameter6 = towerModel.OriginalParameter;
				num2 = originalParameter6.damage_Magic_max;
			}
			currentLevelDamage_max = num2;
			damageText.text = currentLevelDamage_min.ToString() + "-" + currentLevelDamage_max.ToString();
			if (TowerParameter.Instance.isPhysicsAttack(towerModel.Id))
			{
				iconDamage.sprite = physicsDamageIcon;
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
			}
			Text text = healthText;
			Tower originalParameter7 = towerModel.OriginalParameter;
			text.text = originalParameter7.unit_health.ToString();
			Text text2 = armorText;
			UnitAbilitiesRanking instance = UnitAbilitiesRanking.Instance;
			Tower originalParameter8 = towerModel.OriginalParameter;
			text2.text = instance.GetArmorDescriptionByValue(originalParameter8.unit_armor_physics);
			Text text3 = reloadText;
			UnitAbilitiesRanking instance2 = UnitAbilitiesRanking.Instance;
			Tower originalParameter9 = towerModel.OriginalParameter;
			text3.text = instance2.GetAttackSpeedDescriptionByValue(originalParameter9.reload);
			Text text4 = attackRangeText;
			UnitAbilitiesRanking instance3 = UnitAbilitiesRanking.Instance;
			Tower originalParameter10 = towerModel.OriginalParameter;
			text4.text = instance3.GetAttackRangeDescriptionByValue(originalParameter10.attackRangeMax);
			Text text5 = goldProduceText;
			Tower originalParameter11 = towerModel.OriginalParameter;
			text5.text = originalParameter11.goldProduce.ToString();
			Text text6 = timeProduceText;
			Tower originalParameter12 = towerModel.OriginalParameter;
			text6.text = ((float)originalParameter12.reload / 1000f).ToString() + "s";
			Text text7 = autoCollectGoldText;
			Tower originalParameter13 = towerModel.OriginalParameter;
			text7.text = ((float)originalParameter13.autoCollectTime / 1000f).ToString() + "s";
			HideAll();
			if (towerModel.Id == 1)
			{
				ShowBarrackTowerAbility();
			}
			else if (towerModel.Id == 4)
			{
				ShowSupportTowerAbility();
			}
			else
			{
				ShowNormalTowerAbility();
			}
		}

		public void HideAll()
		{
			damageHolder.gameObject.SetActive(value: false);
			reloadHolder.gameObject.SetActive(value: false);
			attackRangeHolder.gameObject.SetActive(value: false);
			healthHolder.gameObject.SetActive(value: false);
			armorHolder.gameObject.SetActive(value: false);
			goldProduceHolder.gameObject.SetActive(value: false);
			timeProduceHolder.gameObject.SetActive(value: false);
			autoCollectGoldHolder.gameObject.SetActive(value: false);
		}

		private void ShowNormalTowerAbility()
		{
			damageHolder.gameObject.SetActive(value: true);
			reloadHolder.gameObject.SetActive(value: true);
			attackRangeHolder.gameObject.SetActive(value: true);
		}

		private void ShowBarrackTowerAbility()
		{
			damageHolder.gameObject.SetActive(value: true);
			healthHolder.gameObject.SetActive(value: true);
			armorHolder.gameObject.SetActive(value: true);
		}

		private void ShowSupportTowerAbility()
		{
			goldProduceHolder.gameObject.SetActive(value: true);
			timeProduceHolder.gameObject.SetActive(value: true);
			autoCollectGoldHolder.gameObject.SetActive(value: true);
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
