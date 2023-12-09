using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples
{
	public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IEventSystemHandler
	{
		public RectTransform TextPopup_Prefab_01;

		private RectTransform m_TextPopup_RectTransform;

		private TextMeshProUGUI m_TextPopup_TMPComponent;

		private const string k_LinkText = "You have selected link <#ffff00>";

		private const string k_WordText = "Word Index: <#ffff00>";

		private TextMeshProUGUI m_TextMeshPro;

		private Canvas m_Canvas;

		private Camera m_Camera;

		private bool isHoveringObject;

		private int m_selectedWord = -1;

		private int m_selectedLink = -1;

		private int m_lastIndex = -1;

		private Matrix4x4 m_matrix;

		private TMP_MeshInfo[] m_cachedMeshInfoVertexData;

		private void Awake()
		{
			m_TextMeshPro = base.gameObject.GetComponent<TextMeshProUGUI>();
			m_Canvas = base.gameObject.GetComponentInParent<Canvas>();
			if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				m_Camera = null;
			}
			else
			{
				m_Camera = m_Canvas.worldCamera;
			}
			m_TextPopup_RectTransform = UnityEngine.Object.Instantiate(TextPopup_Prefab_01);
			m_TextPopup_RectTransform.SetParent(m_Canvas.transform, worldPositionStays: false);
			m_TextPopup_TMPComponent = m_TextPopup_RectTransform.GetComponentInChildren<TextMeshProUGUI>();
			m_TextPopup_RectTransform.gameObject.SetActive(value: false);
		}

		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
		}

		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
		}

		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			if (obj == m_TextMeshPro)
			{
				m_cachedMeshInfoVertexData = m_TextMeshPro.textInfo.CopyMeshInfoVertexData();
			}
		}

		private void LateUpdate()
		{
			if (isHoveringObject)
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(m_TextMeshPro, UnityEngine.Input.mousePosition, m_Camera, visibleOnly: true);
				if (num == -1 || num != m_lastIndex)
				{
					RestoreCachedVertexAttributes(m_lastIndex);
					m_lastIndex = -1;
				}
				if (num != -1 && num != m_lastIndex && (UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift)))
				{
					m_lastIndex = num;
					int materialReferenceIndex = m_TextMeshPro.textInfo.characterInfo[num].materialReferenceIndex;
					int vertexIndex = m_TextMeshPro.textInfo.characterInfo[num].vertexIndex;
					Vector3[] vertices = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].vertices;
					Vector2 v = (vertices[vertexIndex] + vertices[vertexIndex + 2]) / 2f;
					Vector3 b = v;
					vertices[vertexIndex] -= b;
					vertices[vertexIndex + 1] = vertices[vertexIndex + 1] - b;
					vertices[vertexIndex + 2] = vertices[vertexIndex + 2] - b;
					vertices[vertexIndex + 3] = vertices[vertexIndex + 3] - b;
					float d = 1.5f;
					m_matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * d);
					vertices[vertexIndex] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex]);
					vertices[vertexIndex + 1] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 1]);
					vertices[vertexIndex + 2] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 2]);
					vertices[vertexIndex + 3] = m_matrix.MultiplyPoint3x4(vertices[vertexIndex + 3]);
					vertices[vertexIndex] += b;
					vertices[vertexIndex + 1] = vertices[vertexIndex + 1] + b;
					vertices[vertexIndex + 2] = vertices[vertexIndex + 2] + b;
					vertices[vertexIndex + 3] = vertices[vertexIndex + 3] + b;
					Color32 color = new Color32(byte.MaxValue, byte.MaxValue, 192, byte.MaxValue);
					Color32[] colors = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].colors32;
					colors[vertexIndex] = color;
					colors[vertexIndex + 1] = color;
					colors[vertexIndex + 2] = color;
					colors[vertexIndex + 3] = color;
					TMP_MeshInfo tMP_MeshInfo = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex];
					int dst = vertices.Length - 4;
					tMP_MeshInfo.SwapVertexData(vertexIndex, dst);
					m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
				}
				int num2 = TMP_TextUtilities.FindIntersectingWord(m_TextMeshPro, UnityEngine.Input.mousePosition, m_Camera);
				if (m_TextPopup_RectTransform != null && m_selectedWord != -1 && (num2 == -1 || num2 != m_selectedWord))
				{
					TMP_WordInfo tMP_WordInfo = m_TextMeshPro.textInfo.wordInfo[m_selectedWord];
					for (int i = 0; i < tMP_WordInfo.characterCount; i++)
					{
						int num3 = tMP_WordInfo.firstCharacterIndex + i;
						int materialReferenceIndex2 = m_TextMeshPro.textInfo.characterInfo[num3].materialReferenceIndex;
						int vertexIndex2 = m_TextMeshPro.textInfo.characterInfo[num3].vertexIndex;
						Color32[] colors2 = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex2].colors32;
						Color32 color2 = colors2[vertexIndex2].Tint(1.33333f);
						colors2[vertexIndex2] = color2;
						colors2[vertexIndex2 + 1] = color2;
						colors2[vertexIndex2 + 2] = color2;
						colors2[vertexIndex2 + 3] = color2;
					}
					m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
					m_selectedWord = -1;
				}
				if (num2 != -1 && num2 != m_selectedWord && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
				{
					m_selectedWord = num2;
					TMP_WordInfo tMP_WordInfo2 = m_TextMeshPro.textInfo.wordInfo[num2];
					for (int j = 0; j < tMP_WordInfo2.characterCount; j++)
					{
						int num4 = tMP_WordInfo2.firstCharacterIndex + j;
						int materialReferenceIndex3 = m_TextMeshPro.textInfo.characterInfo[num4].materialReferenceIndex;
						int vertexIndex3 = m_TextMeshPro.textInfo.characterInfo[num4].vertexIndex;
						Color32[] colors3 = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex3].colors32;
						Color32 color3 = colors3[vertexIndex3].Tint(0.75f);
						colors3[vertexIndex3] = color3;
						colors3[vertexIndex3 + 1] = color3;
						colors3[vertexIndex3 + 2] = color3;
						colors3[vertexIndex3 + 3] = color3;
					}
					m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
				}
				int num5 = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, UnityEngine.Input.mousePosition, m_Camera);
				if ((num5 == -1 && m_selectedLink != -1) || num5 != m_selectedLink)
				{
					m_TextPopup_RectTransform.gameObject.SetActive(value: false);
					m_selectedLink = -1;
				}
				if (num5 == -1 || num5 == m_selectedLink)
				{
					return;
				}
				m_selectedLink = num5;
				TMP_LinkInfo tMP_LinkInfo = m_TextMeshPro.textInfo.linkInfo[num5];
				Vector3 worldPoint = Vector3.zero;
				RectTransformUtility.ScreenPointToWorldPointInRectangle(m_TextMeshPro.rectTransform, UnityEngine.Input.mousePosition, m_Camera, out worldPoint);
				string linkID = tMP_LinkInfo.GetLinkID();
				if (linkID == null)
				{
					return;
				}
				if (!(linkID == "id_01"))
				{
					if (linkID == "id_02")
					{
						m_TextPopup_RectTransform.position = worldPoint;
						m_TextPopup_RectTransform.gameObject.SetActive(value: true);
						m_TextPopup_TMPComponent.text = "You have selected link <#ffff00> ID 02";
					}
				}
				else
				{
					m_TextPopup_RectTransform.position = worldPoint;
					m_TextPopup_RectTransform.gameObject.SetActive(value: true);
					m_TextPopup_TMPComponent.text = "You have selected link <#ffff00> ID 01";
				}
			}
			else if (m_lastIndex != -1)
			{
				RestoreCachedVertexAttributes(m_lastIndex);
				m_lastIndex = -1;
			}
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			isHoveringObject = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			isHoveringObject = false;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		private void RestoreCachedVertexAttributes(int index)
		{
			if (index != -1 && index <= m_TextMeshPro.textInfo.characterCount - 1)
			{
				int materialReferenceIndex = m_TextMeshPro.textInfo.characterInfo[index].materialReferenceIndex;
				int vertexIndex = m_TextMeshPro.textInfo.characterInfo[index].vertexIndex;
				Vector3[] vertices = m_cachedMeshInfoVertexData[materialReferenceIndex].vertices;
				Vector3[] vertices2 = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].vertices;
				vertices2[vertexIndex] = vertices[vertexIndex];
				vertices2[vertexIndex + 1] = vertices[vertexIndex + 1];
				vertices2[vertexIndex + 2] = vertices[vertexIndex + 2];
				vertices2[vertexIndex + 3] = vertices[vertexIndex + 3];
				Color32[] colors = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].colors32;
				Color32[] colors2 = m_cachedMeshInfoVertexData[materialReferenceIndex].colors32;
				colors[vertexIndex] = colors2[vertexIndex];
				colors[vertexIndex + 1] = colors2[vertexIndex + 1];
				colors[vertexIndex + 2] = colors2[vertexIndex + 2];
				colors[vertexIndex + 3] = colors2[vertexIndex + 3];
				Vector2[] uvs = m_cachedMeshInfoVertexData[materialReferenceIndex].uvs0;
				Vector2[] uvs2 = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].uvs0;
				uvs2[vertexIndex] = uvs[vertexIndex];
				uvs2[vertexIndex + 1] = uvs[vertexIndex + 1];
				uvs2[vertexIndex + 2] = uvs[vertexIndex + 2];
				uvs2[vertexIndex + 3] = uvs[vertexIndex + 3];
				Vector2[] uvs3 = m_cachedMeshInfoVertexData[materialReferenceIndex].uvs2;
				Vector2[] uvs4 = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].uvs2;
				uvs4[vertexIndex] = uvs3[vertexIndex];
				uvs4[vertexIndex + 1] = uvs3[vertexIndex + 1];
				uvs4[vertexIndex + 2] = uvs3[vertexIndex + 2];
				uvs4[vertexIndex + 3] = uvs3[vertexIndex + 3];
				int num = (vertices.Length / 4 - 1) * 4;
				vertices2[num] = vertices[num];
				vertices2[num + 1] = vertices[num + 1];
				vertices2[num + 2] = vertices[num + 2];
				vertices2[num + 3] = vertices[num + 3];
				colors2 = m_cachedMeshInfoVertexData[materialReferenceIndex].colors32;
				colors = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].colors32;
				colors[num] = colors2[num];
				colors[num + 1] = colors2[num + 1];
				colors[num + 2] = colors2[num + 2];
				colors[num + 3] = colors2[num + 3];
				uvs = m_cachedMeshInfoVertexData[materialReferenceIndex].uvs0;
				uvs2 = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].uvs0;
				uvs2[num] = uvs[num];
				uvs2[num + 1] = uvs[num + 1];
				uvs2[num + 2] = uvs[num + 2];
				uvs2[num + 3] = uvs[num + 3];
				uvs3 = m_cachedMeshInfoVertexData[materialReferenceIndex].uvs2;
				uvs4 = m_TextMeshPro.textInfo.meshInfo[materialReferenceIndex].uvs2;
				uvs4[num] = uvs3[num];
				uvs4[num + 1] = uvs3[num + 1];
				uvs4[num + 2] = uvs3[num + 2];
				uvs4[num + 3] = uvs3[num + 3];
				m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
			}
		}
	}
}
