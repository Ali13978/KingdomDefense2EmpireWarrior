using Data;
using UnityEngine;

namespace MapLevel
{
	public class GameModeSelectGroupController : MonoBehaviour
	{
		[SerializeField]
		private ModeSelectButtonController[] listSelectModeButtons;

		[SerializeField]
		private Transform[] selectedImageHolder;

		[SerializeField]
		private GameObject selectedImage;

		public void InitDefault()
		{
			ChooseDefault();
		}

		private void ChooseDefault()
		{
			int lastMapModeChoose = ReadWriteDataMap.Instance.GetLastMapModeChoose();
			if (lastMapModeChoose == 0)
			{
				listSelectModeButtons[1].OnClick();
			}
			else
			{
				listSelectModeButtons[lastMapModeChoose - 1].OnClick();
			}
		}

		public void ShowSelectedImage(BattleLevel battleLevel)
		{
			selectedImage.transform.SetParent(selectedImageHolder[(int)battleLevel]);
			selectedImage.transform.localPosition = Vector3.zero;
		}
	}
}
