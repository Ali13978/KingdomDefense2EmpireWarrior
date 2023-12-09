using UnityEngine;

namespace TMPro.Examples
{
	[ExecuteInEditMode]
	public class TMP_TextInfoDebugTool : MonoBehaviour
	{
		public bool ShowCharacters;

		public bool ShowWords;

		public bool ShowLinks;

		public bool ShowLines;

		public bool ShowMeshBounds;

		public bool ShowTextBounds;

		[Space(10f)]
		[TextArea(2, 2)]
		public string ObjectStats;

		private TMP_Text m_TextComponent;

		private Transform m_Transform;
	}
}
