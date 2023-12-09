using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class TowerInformationController : MonoBehaviour
	{
		[Space]
		[Header("Basic Infor")]
		[SerializeField]
		private Text towerName;

		[Space]
		[Header("Basic Infor")]
		[SerializeField]
		private Text description;

		[Space]
		[Header("Basic Infor")]
		[SerializeField]
		private Text ultimateDescription;

		[Space]
		[Header("Value")]
		[SerializeField]
		private Text damage;

		[Space]
		[Header("Value")]
		[SerializeField]
		private Text attackSpeed;

		[Space]
		[Header("Value")]
		[SerializeField]
		private Text attackRange;

		[SerializeField]
		private Text unitHealth;

		[SerializeField]
		private Text unitPhysicsArmor;

		[SerializeField]
		private Text goldProduce;

		[SerializeField]
		private Text timeProduce;

		[Space]
		[Header("Holder")]
		[SerializeField]
		private GameObject damageHolder;

		[Space]
		[Header("Holder")]
		[SerializeField]
		private GameObject attackSpeedHolder;

		[Space]
		[Header("Holder")]
		[SerializeField]
		private GameObject attackRangeHolder;

		[SerializeField]
		private GameObject unitHealthHolder;

		[SerializeField]
		private GameObject unitPhysicsArmorHolder;

		[SerializeField]
		private GameObject goldProduceHolder;

		[SerializeField]
		private GameObject timeProduceHolder;

		[Space]
		[SerializeField]
		private Image iconDamage;

		[SerializeField]
		private Sprite physicsDamageIcon;

		[SerializeField]
		private Sprite magicDamageIcon;

		public void Init(int _towerID, int _towerLevel)
		{
			SetBasicInformation(_towerID, _towerLevel);
			SetAbilityInformation(_towerID, _towerLevel);
			HideAll();
			switch (_towerID)
			{
			case 1:
				ShowBarrackTowerAbility();
				break;
			case 4:
				ShowSupportTowerAbility();
				break;
			default:
				ShowNormalTowerAbility();
				break;
			}
		}

		private void SetBasicInformation(int _towerID, int _towerLevel)
		{
			towerName.text = Singleton<TowerDescription>.Instance.GetTowerName(_towerID);
			description.text = Singleton<TowerDescription>.Instance.GetTowerDescription(_towerID, _towerLevel).Replace('@', '\n').Replace('#', '-');
			if (_towerLevel == 3 || _towerLevel == 4)
			{
				string text = Singleton<TowerDescription>.Instance.GetTowerUltimateDescription(_towerID, _towerLevel, 0) + "\n" + Singleton<TowerDescription>.Instance.GetTowerUltimateDescription(_towerID, _towerLevel, 1);
				ultimateDescription.text = text.Replace('@', '\n').Replace('#', '-');
			}
			else
			{
				ultimateDescription.text = string.Empty;
			}
		}

		private void SetAbilityInformation(int _towerID, int _towerLevel)
		{
			damage.text = TowerParameter.Instance.GetMinDamage(_towerID, _towerLevel) + "-" + TowerParameter.Instance.GetMaxDamage(_towerID, _towerLevel);
			if (TowerParameter.Instance.isPhysicsAttack(_towerID))
			{
				iconDamage.sprite = physicsDamageIcon;
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
			}
			attackSpeed.text = UnitAbilitiesRanking.Instance.GetAttackSpeedDescriptionByValue(TowerParameter.Instance.GetAttackSpeed(_towerID, _towerLevel));
			attackRange.text = UnitAbilitiesRanking.Instance.GetAttackRangeDescriptionByValue(TowerParameter.Instance.GetRangeMax(_towerID, _towerLevel));
			unitHealth.text = TowerParameter.Instance.GetUnitHealth(_towerID, _towerLevel).ToString();
			unitPhysicsArmor.text = UnitAbilitiesRanking.Instance.GetArmorDescriptionByValue(TowerParameter.Instance.GetUnitArmor(_towerID, _towerLevel));
			goldProduce.text = TowerParameter.Instance.GetGoldProduce(_towerID, _towerLevel).ToString();
			timeProduce.text = TowerParameter.Instance.GetCooldownTime(_towerID, _towerLevel).ToString() + "s";
		}

		public void HideAll()
		{
			damageHolder.gameObject.SetActive(value: false);
			attackSpeedHolder.gameObject.SetActive(value: false);
			attackRangeHolder.gameObject.SetActive(value: false);
			unitHealthHolder.gameObject.SetActive(value: false);
			unitPhysicsArmorHolder.gameObject.SetActive(value: false);
			goldProduceHolder.gameObject.SetActive(value: false);
			timeProduceHolder.gameObject.SetActive(value: false);
		}

		private void ShowNormalTowerAbility()
		{
			damageHolder.gameObject.SetActive(value: true);
			attackSpeedHolder.gameObject.SetActive(value: true);
			attackRangeHolder.gameObject.SetActive(value: true);
		}

		private void ShowBarrackTowerAbility()
		{
			damageHolder.gameObject.SetActive(value: true);
			unitHealthHolder.gameObject.SetActive(value: true);
			unitPhysicsArmorHolder.gameObject.SetActive(value: true);
		}

		private void ShowSupportTowerAbility()
		{
			goldProduceHolder.gameObject.SetActive(value: true);
			timeProduceHolder.gameObject.SetActive(value: true);
		}
	}
}
