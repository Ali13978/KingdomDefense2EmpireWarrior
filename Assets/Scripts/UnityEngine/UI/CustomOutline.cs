using System.Collections.Generic;

namespace UnityEngine.UI
{
	public class CustomOutline : Shadow
	{
		[Range(0f, 15f)]
		public float m_size = 3f;

		public bool glintEffect;

		[Range(0f, 5f)]
		public int glintVertex;

		[Range(0f, 3f)]
		public int glintWidth;

		public Color glintColor;

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			int num = list.Count * 5;
			if (list.Capacity < num)
			{
				list.Capacity = num;
			}
			if (glintEffect)
			{
				for (int i = 0; i < list.Count; i++)
				{
					UIVertex value = list[i];
					for (int j = -glintWidth; j <= glintWidth; j++)
					{
						if ((float)(i % 6) == Mathf.Repeat(glintVertex + j, 6f))
						{
							value.color = glintColor;
						}
					}
					list[i] = value;
				}
			}
			Vector2 vector = new Vector2(m_size, m_size);
			int start = 0;
			int count = list.Count;
			ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, vector.x, vector.y);
			start = count;
			count = list.Count;
			ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, vector.x, 0f - vector.y);
			start = count;
			count = list.Count;
			ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, 0f - vector.x, vector.y);
			start = count;
			count = list.Count;
			ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, 0f - vector.x, 0f - vector.y);
			start = count;
			count = list.Count;
			ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, 0f, vector.y * 1.5f);
			start = count;
			count = list.Count;
			ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, vector.x * 1.5f, 0f);
			start = count;
			count = list.Count;
			ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, (0f - vector.x) * 1.5f, 0f);
			start = count;
			count = list.Count;
			ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, 0f, (0f - vector.y) * 1.5f);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}
	}
}
