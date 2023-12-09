using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInformationPopup : MonoBehaviour
{
	public Image enemyAvatar;

	public Text armorText;

	public Text damageText;

	public Text healthText;

	public Text lifeTakenText;

	public GameObject physicsDamage;

	public GameObject magicDamage;

	public GameObject physicsArmor;

	public GameObject magicArmor;

	private bool isInited;

	public void Init()
	{
		GameEventCenter.Instance.Subscribe(GameEventType.OnSelectEnemy, new SelectCharacterSubscriberData(GameTools.GetUniqueId(), SetInformation));
	}

	public void SetInformation(int enemyId)
	{
		if (enemyId < 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		enemyAvatar.sprite = Resources.Load<Sprite>($"Preview/Enemies/p_enemy_{enemyId}");
		enemyAvatar.SetNativeSize();
		bool flag = EnemyParameterManager.Instance.isPhysicsAttack(enemyId);
		physicsDamage.SetActive(flag);
		magicDamage.SetActive(!flag);
		damageText.text = EnemyParameterManager.Instance.GetMinDamage(enemyId, 1) + "-" + EnemyParameterManager.Instance.GetMaxDamage(enemyId, 1);
		healthText.text = EnemyParameterManager.Instance.GetHealth(enemyId, 1).ToString();
		bool flag2 = EnemyParameterManager.Instance.GetMagicArmor(enemyId, 1) != 0;
		physicsArmor.SetActive(!flag2);
		magicArmor.SetActive(flag2);
		armorText.text = UnitAbilitiesRanking.Instance.GetArmorDescriptionByValue(flag2 ? EnemyParameterManager.Instance.GetMagicArmor(enemyId, 1) : EnemyParameterManager.Instance.GetPhysicsArmor(enemyId, 1));
		lifeTakenText.text = EnemyParameterManager.Instance.GetLifeTaken(enemyId, 1).ToString();
	}
}
