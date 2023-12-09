using Data;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
	public class UpgradeInformationController : CustomMonoBehaviour
	{
		[SerializeField]
		private Image upgradeTierIcon;

		[SerializeField]
		private Text starRequire;

		[SerializeField]
		private Text upgradeTierTitle;

		[SerializeField]
		private Text upgradeTierDescription;

		public void InitDefaultData()
		{
			UpgradePopupController.Instance.upgradeGroupControllers[0].listTierUpgrade[0].InitDefaultData();
		}

		public void InitData(Sprite _tierIcon, int tierID, int upgradeID, int towerID)
		{
			upgradeTierIcon.enabled = true;
			upgradeTierIcon.sprite = _tierIcon;
			starRequire.text = ReadWriteDataGlobalUpgrade.Instance.GetStarRequireForUpgrade(towerID, tierID).ToString();
			upgradeTierTitle.text = Singleton<GlobalUpgradeDescription>.Instance.GetTitle(upgradeID);
			upgradeTierDescription.text = string.Format(Singleton<GlobalUpgradeDescription>.Instance.GetDescription(upgradeID), ReadWriteDataGlobalUpgrade.Instance.GetUpgradeValue(upgradeID, towerID)).Replace('@', '\n').Replace('#', '-');
		}
	}
}
