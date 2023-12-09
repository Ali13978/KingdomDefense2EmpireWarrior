using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	public class VertexColorCycler : MonoBehaviour
	{
		private TMP_Text m_TextComponent;

		private void Awake()
		{
			m_TextComponent = GetComponent<TMP_Text>();
		}

		private void Start()
		{
			StartCoroutine(AnimateVertexColors());
		}

		private IEnumerator AnimateVertexColors()
		{
			TMP_TextInfo textInfo = m_TextComponent.textInfo;
			int currentCharacter = 0;
			Color32 color = (Color32)m_TextComponent.color;
			while (true)
			{
				int characterCount = textInfo.characterCount;
				if (characterCount == 0)
				{
					yield return new WaitForSeconds(0.25f);
					continue;
				}
				int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
				Color32[] newVertexColors = textInfo.meshInfo[materialIndex].colors32;
				int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
				if (textInfo.characterInfo[currentCharacter].isVisible)
				{
					Color32 c0 = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), byte.MaxValue);
					newVertexColors[vertexIndex] = c0;
					newVertexColors[vertexIndex + 1] = c0;
					newVertexColors[vertexIndex + 2] = c0;
					newVertexColors[vertexIndex + 3] = c0;
					m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				}
				currentCharacter = (currentCharacter + 1) % characterCount;
				yield return new WaitForSeconds(0.05f);
			}
		}
	}
}
