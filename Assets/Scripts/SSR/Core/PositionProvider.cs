using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core
{
	public abstract class PositionProvider : MonoBehaviour, IPositionProvider
	{
		public abstract Vector3 Position
		{
			get;
		}
	}
}
