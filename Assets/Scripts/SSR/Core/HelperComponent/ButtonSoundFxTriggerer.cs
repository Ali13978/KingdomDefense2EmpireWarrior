using UnityEngine;
using UnityEngine.UI;

namespace SSR.Core.HelperComponent
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Button))]
	public class ButtonSoundFxTriggerer : SoundFxTriggerer
	{
		[SerializeField]
		[HideInNormalInspector]
		private Button button;

		public void Awake()
		{
			button.onClick.AddListener(OnButtonClicked);
		}

		public void Reset()
		{
			button = GetComponent<Button>();
		}

		private void OnButtonClicked()
		{
			PlayFxSound();
		}
	}
}
