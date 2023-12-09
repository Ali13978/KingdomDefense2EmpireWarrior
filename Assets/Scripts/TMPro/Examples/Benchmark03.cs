using UnityEngine;

namespace TMPro.Examples
{
	public class Benchmark03 : MonoBehaviour
	{
		public int SpawnType;

		public int NumberOfNPC = 12;

		public Font TheFont;

		private void Awake()
		{
		}

		private void Start()
		{
			for (int i = 0; i < NumberOfNPC; i++)
			{
				if (SpawnType == 0)
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = new Vector3(0f, 0f, 0f);
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.alignment = TextAlignmentOptions.Center;
					textMeshPro.fontSize = 96f;
					textMeshPro.text = "@";
					textMeshPro.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
				}
				else
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.position = new Vector3(0f, 0f, 0f);
					TextMesh textMesh = gameObject2.AddComponent<TextMesh>();
					textMesh.GetComponent<Renderer>().sharedMaterial = TheFont.material;
					textMesh.font = TheFont;
					textMesh.anchor = TextAnchor.MiddleCenter;
					textMesh.fontSize = 96;
					textMesh.color = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);
					textMesh.text = "@";
				}
			}
		}
	}
}
