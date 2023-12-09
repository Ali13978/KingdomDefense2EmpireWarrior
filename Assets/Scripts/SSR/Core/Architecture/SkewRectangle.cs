using UnityEngine;

namespace SSR.Core.Architecture
{
	[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
	[ExecuteInEditMode]
	public class SkewRectangle : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private MeshRenderer meshRenderer;

		[SerializeField]
		[HideInInspector]
		private MeshFilter meshFilter;

		[SerializeField]
		private Vector3 v0 = Vector3.left + Vector3.up;

		[SerializeField]
		private Vector3 v1 = Vector3.right + Vector3.up;

		[SerializeField]
		private Vector3 v2 = Vector3.right + Vector3.down;

		[SerializeField]
		private Vector3 v3 = Vector3.left + Vector3.down;

		[SerializeField]
		private Mesh mesh;

		private Mesh CreateMesh()
		{
			mesh = new Mesh();
			mesh.vertices = GetVerticesArray();
			mesh.triangles = new int[6]
			{
				0,
				1,
				2,
				3,
				4,
				5
			};
			mesh.uv = new Vector2[6]
			{
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 0f),
				new Vector2(0f, 0f)
			};
			mesh.uv2 = new Vector2[6]
			{
				new Vector2(1f, 0f),
				new Vector2(Mathf.Sqrt(2f), 0f),
				new Vector2(1f, 0f),
				new Vector2(1f, 0f),
				new Vector2(1f, 0f),
				new Vector2(Mathf.Sqrt(2f), 0f)
			};
			return mesh;
		}

		private void UpdateMesh()
		{
			if (!(mesh == null))
			{
				mesh.vertices = GetVerticesArray();
			}
		}

		private Vector3[] GetVerticesArray()
		{
			return new Vector3[6]
			{
				v0,
				v1,
				v2,
				v0,
				v2,
				v3
			};
		}

		public void OnDrawGizmos()
		{
			UpdateMesh();
		}

		public void Reset()
		{
			meshRenderer = GetComponent<MeshRenderer>();
			meshFilter = GetComponent<MeshFilter>();
			mesh = CreateMesh();
			meshFilter.mesh = mesh;
		}
	}
}
