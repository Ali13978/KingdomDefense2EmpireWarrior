using UnityEngine;
using UnityEngine.UI;

namespace SSR.Core.Architecture.UI
{
	[RequireComponent(typeof(Text))]
	[DisallowMultipleComponent]
	public abstract class TextContentSetter : ContentSetter
	{
		[SerializeField]
		[HideInNormalInspector]
		private Text text;

		protected Text Text
		{
			get
			{
				if (text == null)
				{
					text = GetComponent<Text>();
				}
				return text;
			}
		}

		public virtual void Reset()
		{
			text = GetComponent<Text>();
		}
	}
}
