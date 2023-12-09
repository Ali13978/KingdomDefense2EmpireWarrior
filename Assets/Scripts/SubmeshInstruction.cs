using UnityEngine;

namespace Spine.Unity
{
	public struct SubmeshInstruction
	{
		public Skeleton skeleton;

		public int startSlot;

		public int endSlot;

		public Material material;

		public bool forceSeparate;

		public int preActiveClippingSlotSource;

		public int rawTriangleCount;

		public int rawVertexCount;

		public int rawFirstVertexIndex;

		public bool hasClipping;

		public int SlotCount => endSlot - startSlot;

		public override string ToString()
		{
			return string.Format("[SubmeshInstruction: slots {0} to {1}. (Material){2}. preActiveClippingSlotSource:{3}]", startSlot, endSlot - 1, (!(material == null)) ? material.name : "<none>", preActiveClippingSlotSource);
		}
	}
}
