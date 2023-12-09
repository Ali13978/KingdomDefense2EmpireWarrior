using UnityEngine;

namespace SSR.Core.Architecture
{
	public class Vector3BoostContainer : CumulativeBoostContainter<Vector3>
	{
		public override Vector3 BoostedValue
		{
			get
			{
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < base.ComponentsList.Count; i++)
				{
					vector += base.ComponentsList[i].BoostValue;
				}
				return vector;
			}
		}
	}
}
