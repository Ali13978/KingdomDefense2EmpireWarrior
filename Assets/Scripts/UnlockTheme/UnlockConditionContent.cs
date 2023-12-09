using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UnlockTheme
{
	public class UnlockConditionContent : MonoBehaviour
	{
		[SerializeField]
		private Text descriptionText;

		[SerializeField]
		private Toggle unlockStateToggle;

		public void InitContent(int conditionType, int themeID, bool isPassDescription)
		{
			Show();
			descriptionText.text = ReadWriteDataTheme.Instance.GetDescription(conditionType, themeID);
			unlockStateToggle.isOn = isPassDescription;
		}

		private void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
