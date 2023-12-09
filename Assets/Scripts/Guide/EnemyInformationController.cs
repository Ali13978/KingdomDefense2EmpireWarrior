using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class EnemyInformationController : MonoBehaviour
	{
		[SerializeField]
		private Image fullAvatar;

		[SerializeField]
		private Text enemyName;

		[SerializeField]
		private Text description;

		[SerializeField]
		private Text specialAbilityDescription;

		[SerializeField]
		private Text health;

		[SerializeField]
		private Text damage;

		[SerializeField]
		private Text physicsArmor;

		[SerializeField]
		private Text magicArmor;

		[SerializeField]
		private Text movementSpeed;

		[SerializeField]
		private Text lifeTaken;

		[Space]
		[SerializeField]
		private Image iconDamage;

		[SerializeField]
		private Sprite physicsDamageIcon;

		[SerializeField]
		private Sprite magicDamageIcon;

		public void Init(int _enemyID)
		{
			SetBasicInformation(_enemyID);
			SetAbilityInformation(_enemyID);
		}

		private void SetFullAvatar(int _enemyID)
		{
			fullAvatar.sprite = Resources.Load<Sprite>($"Guide/Enemies/FullAvatars/fa_enemy_{_enemyID}");
		}

		private void SetBasicInformation(int _enemyID)
		{
			enemyName.text = Singleton<EnemyDescription>.Instance.GetEnemyName(_enemyID);
			description.text = Singleton<EnemyDescription>.Instance.GetEnemyDescription(_enemyID).Replace('@', '\n').Replace('#', '-');
			specialAbilityDescription.text = Singleton<EnemyDescription>.Instance.GetEnemySpecialAbility(_enemyID).Replace('@', '\n').Replace('#', '-');
		}

		private void SetAbilityInformation(int _enemyID)
		{
			health.text = EnemyParameterManager.Instance.GetHealth(_enemyID, 1).ToString();
			damage.text = EnemyParameterManager.Instance.GetMinDamage(_enemyID, 1) + "-" + EnemyParameterManager.Instance.GetMaxDamage(_enemyID, 1);
			if (EnemyParameterManager.Instance.isPhysicsAttack(_enemyID))
			{
				iconDamage.sprite = physicsDamageIcon;
			}
			else
			{
				iconDamage.sprite = magicDamageIcon;
			}
			physicsArmor.text = UnitAbilitiesRanking.Instance.GetArmorDescriptionByValue(EnemyParameterManager.Instance.GetPhysicsArmor(_enemyID, 1));
			magicArmor.text = UnitAbilitiesRanking.Instance.GetArmorDescriptionByValue(EnemyParameterManager.Instance.GetMagicArmor(_enemyID, 1));
			movementSpeed.text = UnitAbilitiesRanking.Instance.GetMoveSpeedDescriptionByValue(EnemyParameterManager.Instance.GetSpeed(_enemyID, 1));
			lifeTaken.text = EnemyParameterManager.Instance.GetLifeTaken(_enemyID, 1).ToString();
		}
	}
}
