using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[Serializable]
	public struct RangeOfIntegers
	{
		[Tooltip("Minimum value (inclusive)")]
		public int Minimum;

		[Tooltip("Maximum value (inclusive)")]
		public int Maximum;

		public int Random()
		{
			return UnityEngine.Random.Range(Minimum, Maximum + 1);
		}

		public int Random(System.Random r)
		{
			return r.Next(Minimum, Maximum + 1);
		}
	}
}
