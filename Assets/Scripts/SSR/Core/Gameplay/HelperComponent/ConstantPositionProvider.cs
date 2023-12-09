using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class ConstantPositionProvider : PositionProvider
	{
		[SerializeField]
		private Vector3 position;

		public override Vector3 Position => position;
	}
}
