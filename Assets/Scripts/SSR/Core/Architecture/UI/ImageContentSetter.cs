using UnityEngine;
using UnityEngine.UI;

namespace SSR.Core.Architecture.UI
{
	[RequireComponent(typeof(Image))]
	public abstract class ImageContentSetter : ContentSetter
	{
		[SerializeField]
		[HideInNormalInspector]
		private Image image;

		protected Image Image
		{
			get
			{
				if (image == null)
				{
					image = GetComponent<Image>();
				}
				return image;
			}
		}

		public virtual void Reset()
		{
			image = GetComponent<Image>();
		}
	}
}
