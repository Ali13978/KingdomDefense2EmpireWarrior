using UnityEngine;

namespace Gameplay
{
	public class ClickInputData
	{
		public ClickInputPhase clickInputPhase;

		public RaycastHit2D mapHit;

		public RaycastHit2D entityHit;

		public ClickInputData(ClickInputPhase clickInputPhase, RaycastHit2D mapHit, RaycastHit2D entityHit)
		{
			this.clickInputPhase = clickInputPhase;
			this.mapHit = mapHit;
			this.entityHit = entityHit;
		}

		public bool CompareTag(RaycastHit2D hit, string pTag)
		{
			if (hit.collider == null)
			{
				return false;
			}
			return hit.collider.CompareTag(pTag);
		}
	}
}
