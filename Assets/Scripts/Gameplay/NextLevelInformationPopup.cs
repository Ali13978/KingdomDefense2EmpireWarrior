using Data;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class NextLevelInformationPopup : CustomMonoBehaviour
	{
		private TowerModel towerModel;

		private int towerID;

		private Tower towerParameter;

		private Transform target;

		[Header("UI Text")]
		[SerializeField]
		private Text nameText;

		[Header("UI Text")]
		[SerializeField]
		private Text descriptionText;

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

		private int nextLevelDamage_min;

		private int nextLevelDamage_max;

		private int nextLevelAtkSpeed;

		private int nextLevelAttackRange;

		private bool isActive;

		[SerializeField]
		private Vector3 offset;

		private Vector3 PoolPos = new Vector3(1000f, 200f, 0f);

		[SerializeField]
		private float leftVisiblePosX;

		[SerializeField]
		private float rightVisiblePosX;

		[Space]
		[SerializeField]
		private Image iconDamage;

		[SerializeField]
		private Sprite physicsDamageIcon;

		[SerializeField]
		private Sprite magicDamageIcon;

		private void Update()
		{
			if (isActive)
			{
				SetPositionFolowTower();
			}
		}

		public void Init(int ultiNo, int towerID, int towerLevel, Transform _target)
		{
			Open();
			if ((bool)SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.towerModel)
			{
				towerModel = SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.towerModel;
			}
			target = _target;
			this.towerID = towerID;
			int num = towerLevel + ultiNo + 1;
			towerParameter = TowerParameter.Instance.GetTowerParameter(towerID, num);
			nameText.text = Singleton<TowerDescription>.Instance.GetTowerName(towerID);
			descriptionText.text = Singleton<TowerDescription>.Instance.GetTowerShortDescription(towerID, num);
			nextLevelDamage_min = ((towerParameter.damage_Physics_min <= 0) ? towerParameter.damage_Magic_min : towerParameter.damage_Physics_min);
			nextLevelDamage_max = ((towerParameter.damage_Physics_max <= 0) ? towerParameter.damage_Magic_max : towerParameter.damage_Physics_max);
			damageText.text = nextLevelDamage_min.ToString() + "-" + nextLevelDamage_max.ToString();
			if (TowerParameter.Instance.isPhysicsAttack(towerID))
			{
				iconDamage.sprite = physicsDamageIcon;
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
			}
			reloadText.text = UnitAbilitiesRanking.Instance.GetAttackSpeedDescriptionByValue(towerParameter.reload);
			attackRangeText.text = UnitAbilitiesRanking.Instance.GetAttackRangeDescriptionByValue(towerParameter.attackRangeMax);
			healthText.text = towerParameter.unit_health.ToString();
			armorText.text = UnitAbilitiesRanking.Instance.GetArmorDescriptionByValue(towerParameter.unit_armor_physics);
			goldProduceText.text = towerParameter.goldProduce.ToString();
			timeProduceText.text = ((float)towerParameter.reload / 1000f).ToString() + "s";
			autoCollectGoldText.text = ((float)towerParameter.autoCollectTime / 1000f).ToString() + "s";
			HideAll();
			switch (towerID)
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

		private void SetPositionFolowTower()
		{
			Vector3 position = target.position;
			float x = position.x;
			Vector3 position2 = Camera.main.transform.position;
			if (x > position2.x)
			{
				offset.Set(leftVisiblePosX, SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.listOffsetTower[towerID].y, 0f);
				base.transform.position = target.position + offset;
			}
			else
			{
				offset.Set(rightVisiblePosX, SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.listOffsetTower[towerID].y, 0f);
				base.transform.position = target.position + offset;
			}
		}

		private void Open()
		{
			base.gameObject.SetActive(value: true);
			isActive = true;
		}

		public void Close()
		{
			isActive = false;
			towerModel = null;
			base.transform.position = PoolPos;
			base.gameObject.SetActive(value: false);
		}
	}
}
