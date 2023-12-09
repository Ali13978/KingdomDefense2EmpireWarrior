using System;
using UnityEngine;

namespace SSR.Core.Architecture
{
	[Serializable]
	public class Vector3Modifier
	{
		[SerializeField]
		private Vector3 offsetVector = Vector3.zero;

		[SerializeField]
		private Vector3 overrideVector = Vector3.zero;

		[SerializeField]
		private bool overrideX;

		[SerializeField]
		private bool overrideY;

		[SerializeField]
		private bool overrideZ;

		public Vector3 OffsetVector
		{
			get
			{
				return offsetVector;
			}
			set
			{
				offsetVector = value;
			}
		}

		public Vector3 OverrideVector
		{
			get
			{
				return overrideVector;
			}
			set
			{
				overrideVector = value;
			}
		}

		public bool OverrideX
		{
			get
			{
				return overrideX;
			}
			set
			{
				overrideX = value;
			}
		}

		public bool OverrideY
		{
			get
			{
				return overrideY;
			}
			set
			{
				overrideY = value;
			}
		}

		public bool OverrideZ
		{
			get
			{
				return overrideZ;
			}
			set
			{
				overrideZ = value;
			}
		}

		public static Vector3Modifier Unchange
		{
			get
			{
				Vector3Modifier vector3Modifier = new Vector3Modifier();
				vector3Modifier.OffsetVector = Vector3.zero;
				vector3Modifier.OverrideVector = Vector3.zero;
				vector3Modifier.OverrideX = false;
				vector3Modifier.OverrideY = false;
				vector3Modifier.OverrideZ = false;
				return vector3Modifier;
			}
		}

		public Vector3 GetModified(Vector3 originalVector3)
		{
			if (OverrideX)
			{
				Vector3 vector = OverrideVector;
				originalVector3.x = vector.x;
			}
			if (OverrideY)
			{
				Vector3 vector2 = OverrideVector;
				originalVector3.y = vector2.y;
			}
			if (OverrideZ)
			{
				Vector3 vector3 = OverrideVector;
				originalVector3.z = vector3.z;
			}
			return originalVector3 + OffsetVector;
		}
	}
}
