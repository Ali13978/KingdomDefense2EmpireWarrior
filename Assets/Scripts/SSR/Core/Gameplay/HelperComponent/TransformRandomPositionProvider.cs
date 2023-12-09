using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class TransformRandomPositionProvider : PositionProvider
	{
		[SerializeField]
		private List<Transform> transforms = new List<Transform>();

		public override Vector3 Position
		{
			get
			{
				int index = Random.Range(0, transforms.Count);
				return transforms[index].position;
			}
		}
	}
}
