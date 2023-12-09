using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class MeshHelper
	{
		private Mesh mesh;

		private int[] triangles;

		private Vector3[] vertices;

		private Vector3[] normals;

		private float[] normalizedAreaWeights;

		public Mesh Mesh => mesh;

		public int[] Triangles => triangles;

		public Vector3[] Vertices => vertices;

		public Vector3[] Normals => normals;

		public MeshHelper(Mesh mesh)
		{
			this.mesh = mesh;
			triangles = mesh.triangles;
			vertices = mesh.vertices;
			normals = mesh.normals;
			CalculateNormalizedAreaWeights();
		}

		public void GenerateRandomPoint(ref RaycastHit hit, out int triangleIndex)
		{
			triangleIndex = SelectRandomTriangle();
			GetRaycastFromTriangleIndex(triangleIndex, ref hit);
		}

		public void GetRaycastFromTriangleIndex(int triangleIndex, ref RaycastHit hit)
		{
			Vector3 barycentricCoordinate = GenerateRandomBarycentricCoordinates();
			Vector3 a = vertices[triangles[triangleIndex]];
			Vector3 vector = vertices[triangles[triangleIndex + 1]];
			Vector3 a2 = vertices[triangles[triangleIndex + 2]];
			hit.barycentricCoordinate = barycentricCoordinate;
			hit.point = a * barycentricCoordinate.x + vector * barycentricCoordinate.y + a2 * barycentricCoordinate.z;
			if (normals == null)
			{
				hit.normal = Vector3.Cross(a2 - vector, a - vector).normalized;
				return;
			}
			a = normals[triangles[triangleIndex]];
			vector = normals[triangles[triangleIndex + 1]];
			a2 = normals[triangles[triangleIndex + 2]];
			hit.normal = a * barycentricCoordinate.x + vector * barycentricCoordinate.y + a2 * barycentricCoordinate.z;
		}

		private float[] CalculateSurfaceAreas(out float totalSurfaceArea)
		{
			int num = 0;
			totalSurfaceArea = 0f;
			float[] array = new float[triangles.Length / 3];
			for (int i = 0; i < triangles.Length; i += 3)
			{
				Vector3 a = vertices[triangles[i]];
				Vector3 vector = vertices[triangles[i + 1]];
				Vector3 b = vertices[triangles[i + 2]];
				float sqrMagnitude = (a - vector).sqrMagnitude;
				float sqrMagnitude2 = (a - b).sqrMagnitude;
				float sqrMagnitude3 = (vector - b).sqrMagnitude;
				float x = (2f * sqrMagnitude * sqrMagnitude2 + 2f * sqrMagnitude2 * sqrMagnitude3 + 2f * sqrMagnitude3 * sqrMagnitude - sqrMagnitude * sqrMagnitude - sqrMagnitude2 * sqrMagnitude2 - sqrMagnitude3 * sqrMagnitude3) / 16f;
				float num2 = PathGenerator.SquareRoot(x);
				array[num++] = num2;
				totalSurfaceArea += num2;
			}
			return array;
		}

		private void CalculateNormalizedAreaWeights()
		{
			normalizedAreaWeights = CalculateSurfaceAreas(out float totalSurfaceArea);
			if (normalizedAreaWeights.Length != 0)
			{
				float num = 0f;
				for (int i = 0; i < normalizedAreaWeights.Length; i++)
				{
					float num2 = normalizedAreaWeights[i] / totalSurfaceArea;
					normalizedAreaWeights[i] = num;
					num += num2;
				}
			}
		}

		private int SelectRandomTriangle()
		{
			float value = Random.value;
			int num = 0;
			int num2 = normalizedAreaWeights.Length - 1;
			while (num < num2)
			{
				int num3 = (num + num2) / 2;
				if (normalizedAreaWeights[num3] < value)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3;
				}
			}
			return num * 3;
		}

		private Vector3 GenerateRandomBarycentricCoordinates()
		{
			Vector3 a = new Vector3(Random.Range(Mathf.Epsilon, 1f), Random.Range(Mathf.Epsilon, 1f), Random.Range(Mathf.Epsilon, 1f));
			return a / (a.x + a.y + a.z);
		}
	}
}
