using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Guide
{
	public class TipInformationController : CustomMonoBehaviour
	{
		[SerializeField]
		private Image tipAvatar;

		[SerializeField]
		private Text tipName;

		[SerializeField]
		private Text tipDescription;

		public void InitInformation(int tipID)
		{
			tipAvatar.sprite = Resources.Load<Sprite>($"NewTip/avatar_tip_{tipID}");
			tipAvatar.SetNativeSize();
			tipName.text = Singleton<GameplayTipsDescription>.Instance.GetName(tipID);
			tipDescription.text = Singleton<GameplayTipsDescription>.Instance.GetDescription(tipID).Replace('@', '\n').Replace('#', '-');
		}
	}
}
