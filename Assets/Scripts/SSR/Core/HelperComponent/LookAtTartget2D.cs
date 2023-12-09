using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class LookAtTartget2D : OutsiteTargetTransform
	{
		[Header("LookAtTartget2D")]
		[SerializeField]
		private PositionProvider destination;

		[SerializeField]
		private bool useRightAxis = true;

		public void Update()
		{
			if (useRightAxis)
			{
				base.TargetTransform.right = destination.Position - base.TargetTransform.position;
			}
			else
			{
				base.TargetTransform.up = destination.Position - base.TargetTransform.position;
			}
		}
	}
}
