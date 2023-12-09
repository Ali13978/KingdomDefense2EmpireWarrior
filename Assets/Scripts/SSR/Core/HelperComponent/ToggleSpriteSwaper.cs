using UnityEngine;
using UnityEngine.UI;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(Toggle))]
	[ExecuteInEditMode]
	public class ToggleSpriteSwaper : MonoBehaviour
	{
		[SerializeField]
		private Image targetGraphic;

		[SerializeField]
		private Sprite onSprite;

		[SerializeField]
		private Sprite offSprite;

		private Toggle toggle;

		private void Awake()
		{
			toggle = GetComponent<Toggle>();
		}

		private void Update()
		{
			if (!(targetGraphic == null))
			{
				targetGraphic.sprite = ((!toggle.isOn) ? offSprite : onSprite);
			}
		}

		private void Reset()
		{
			targetGraphic = (GetComponent<Toggle>().targetGraphic as Image);
		}
	}
}
