using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class QuadStretcherLeftPositionProvider : PositionProvider
	{
		public override Vector3 Position
		{
			get
			{
				Vector3 position = base.transform.position;
				Vector3 lossyScale = base.transform.lossyScale;
				return position + new Vector3(0f - lossyScale.x / 2f, 0f, 0f);
			}
		}
	}
}
