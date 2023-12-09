using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class SkewTextExample : MonoBehaviour
	{
		private TMP_Text m_TextComponent;

		public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.25f, 2f), new Keyframe(0.5f, 0f), new Keyframe(0.75f, 2f), new Keyframe(1f, 0f));

		public float CurveScale = 1f;

		public float ShearAmount = 1f;

		private void Awake()
		{
			m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		private void Start()
		{
			StartCoroutine(WarpText());
		}

		private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			animationCurve.keys = curve.keys;
			return animationCurve;
		}

		private IEnumerator WarpText()
		{
			VertexCurve.preWrapMode = WrapMode.Once;
			VertexCurve.postWrapMode = WrapMode.Once;
			m_TextComponent.havePropertiesChanged = true;
			CurveScale *= 10f;
			float old_CurveScale = CurveScale;
			float old_ShearValue = ShearAmount;
			AnimationCurve old_curve = CopyAnimationCurve(VertexCurve);
			while (true)
			{
				if (!m_TextComponent.havePropertiesChanged && old_CurveScale == CurveScale && old_curve.keys[1].value == VertexCurve.keys[1].value && old_ShearValue == ShearAmount)
				{
					yield return null;
					continue;
				}
				old_CurveScale = CurveScale;
				old_curve = CopyAnimationCurve(VertexCurve);
				old_ShearValue = ShearAmount;
				m_TextComponent.ForceMeshUpdate();
				TMP_TextInfo textInfo = m_TextComponent.textInfo;
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					continue;
				}
				Vector3 min = m_TextComponent.bounds.min;
				float boundsMinX = min.x;
				Vector3 max = m_TextComponent.bounds.max;
				float boundsMaxX = max.x;
				for (int i = 0; i < characterCount; i++)
				{
					if (textInfo.characterInfo[i].isVisible)
					{
						int vertexIndex = textInfo.characterInfo[i].vertexIndex;
						int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
						Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
						Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
						vertices[vertexIndex] += -vector;
						vertices[vertexIndex + 1] += -vector;
						vertices[vertexIndex + 2] += -vector;
						vertices[vertexIndex + 3] += -vector;
						float num = ShearAmount * 0.01f;
						Vector3 vector2 = new Vector3(num * (textInfo.characterInfo[i].topRight.y - textInfo.characterInfo[i].baseLine), 0f, 0f);
						Vector3 a = new Vector3(num * (textInfo.characterInfo[i].baseLine - textInfo.characterInfo[i].bottomRight.y), 0f, 0f);
						vertices[vertexIndex] += -a;
						vertices[vertexIndex + 1] += vector2;
						vertices[vertexIndex + 2] += vector2;
						vertices[vertexIndex + 3] += -a;
						float num2 = (vector.x - boundsMinX) / (boundsMaxX - boundsMinX);
						float num3 = num2 + 0.0001f;
						float y = VertexCurve.Evaluate(num2) * CurveScale;
						float y2 = VertexCurve.Evaluate(num3) * CurveScale;
						Vector3 lhs = new Vector3(1f, 0f, 0f);
						Vector3 rhs = new Vector3(num3 * (boundsMaxX - boundsMinX) + boundsMinX, y2) - new Vector3(vector.x, y);
						float num4 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
						Vector3 vector3 = Vector3.Cross(lhs, rhs);
						float z = (!(vector3.z > 0f)) ? (360f - num4) : num4;
						Matrix4x4 matrix = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
						vertices[vertexIndex] = matrix.MultiplyPoint3x4(vertices[vertexIndex]);
						vertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
						vertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
						vertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);
						vertices[vertexIndex] += vector;
						vertices[vertexIndex + 1] += vector;
						vertices[vertexIndex + 2] += vector;
						vertices[vertexIndex + 3] += vector;
					}
				}
				m_TextComponent.UpdateVertexData();
				yield return null;
			}
		}
	}
}
