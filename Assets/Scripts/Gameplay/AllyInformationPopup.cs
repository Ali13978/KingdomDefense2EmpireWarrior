using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class AllyInformationPopup : MonoBehaviour
	{
		[SerializeField]
		private Text physicsArmorText;

		[SerializeField]
		private Text magicArmorText;

		[SerializeField]
		private Text damageText;

		[SerializeField]
		private Text healthText;

		public void Init()
		{
			GameEventCenter.Instance.Subscribe(GameEventType.OnSelectAlly, new AllySubscriberData(GameTools.GetUniqueId(), SetInformation));
		}

		public void SetInformation(AllyModel allyModel)
		{
			if (allyModel == null)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			base.gameObject.SetActive(value: true);
			damageText.text = allyModel.PhysicsDamage_min + "-" + allyModel.PhysicsDamage_max;
			healthText.text = allyModel.Health.ToString();
			physicsArmorText.text = UnitAbilitiesRanking.Instance.GetArmorDescriptionByValue(allyModel.PhysicsArmor);
			magicArmorText.text = UnitAbilitiesRanking.Instance.GetArmorDescriptionByValue(allyModel.MagicArmor);
		}
	}
}
