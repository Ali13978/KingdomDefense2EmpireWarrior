using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class TransformPositionProvider : PositionProvider
	{
		[SerializeField]
		private Transform targetTransform;

		[SerializeField]
		private bool useLocal;

		[SerializeField]
		private Vector3Modifier modifier = new Vector3Modifier();

		public override Vector3 Position
		{
			get
			{
				Vector3 originalVector = (!useLocal) ? targetTransform.position : targetTransform.localPosition;
				return modifier.GetModified(originalVector);
			}
		}
	}
}
