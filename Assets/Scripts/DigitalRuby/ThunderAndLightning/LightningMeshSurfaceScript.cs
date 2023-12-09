using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningMeshSurfaceScript : LightningBoltPrefabScriptBase
	{
		[Header("Lightning Mesh Properties")]
		[Tooltip("The mesh filter. You must assign a mesh filter in order to create lightning on the mesh.")]
		public MeshFilter MeshFilter;

		[Tooltip("The mesh collider. This is used to get random points on the mesh.")]
		public Collider MeshCollider;

		[SingleLine("Random range that the point will offset from the mesh, using the normal of the chosen point to offset")]
		public RangeOfFloats MeshOffsetRange = new RangeOfFloats
		{
			Minimum = 0.5f,
			Maximum = 1f
		};

		[Header("Lightning Path Properties")]
		[SingleLine("Range for points in the lightning path")]
		public RangeOfIntegers PathLengthCount = new RangeOfIntegers
		{
			Minimum = 3,
			Maximum = 6
		};

		[SingleLine("Range for minimum distance between points in the lightning path")]
		public RangeOfFloats MinimumPathDistanceRange = new RangeOfFloats
		{
			Minimum = 0.5f,
			Maximum = 1f
		};

		[Tooltip("The maximum distance between mesh points. When walking the mesh, if a point is greater than this, the path direction is reversed. This tries to avoid paths crossing between mesh points that are not actually physically touching.")]
		public float MaximumPathDistance = 2f;

		private float maximumPathDistanceSquared;

		[Tooltip("Whether to use spline interpolation between the path points. Paths must be at least 4 points long to be splined.")]
		public bool Spline;

		[Tooltip("For spline. the distance hint for each spline segment. Set to <= 0 to use the generations to determine how many spline segments to use. If > 0, it will be divided by Generations before being applied. This value is a guideline and is approximate, and not uniform on the spline.")]
		public float DistancePerSegmentHint;

		private readonly List<Vector3> sourcePoints = new List<Vector3>();

		private Mesh previousMesh;

		private MeshHelper meshHelper;

		private void CheckMesh()
		{
			if (MeshFilter == null || MeshFilter.sharedMesh == null)
			{
				meshHelper = null;
			}
			else if (MeshFilter.sharedMesh != previousMesh)
			{
				previousMesh = MeshFilter.sharedMesh;
				meshHelper = new MeshHelper(previousMesh);
			}
		}

		protected override LightningBoltParameters OnCreateParameters()
		{
			LightningBoltParameters lightningBoltParameters = base.OnCreateParameters();
			lightningBoltParameters.Generator = LightningGeneratorPath.PathGeneratorInstance;
			return lightningBoltParameters;
		}

		protected virtual void PopulateSourcePoints(List<Vector3> points)
		{
			if (meshHelper != null)
			{
				CreateRandomLightningPath(sourcePoints);
			}
		}

		public void CreateRandomLightningPath(List<Vector3> points)
		{
			if (meshHelper == null)
			{
				return;
			}
			RaycastHit hit = default(RaycastHit);
			maximumPathDistanceSquared = MaximumPathDistance * MaximumPathDistance;
			meshHelper.GenerateRandomPoint(ref hit, out int triangleIndex);
			hit.distance = Random.Range(MeshOffsetRange.Minimum, MeshOffsetRange.Maximum);
			Vector3 vector = hit.point + hit.normal * hit.distance;
			float num = Random.Range(MinimumPathDistanceRange.Minimum, MinimumPathDistanceRange.Maximum);
			num *= num;
			sourcePoints.Add(MeshFilter.transform.TransformPoint(vector));
			int num2 = (Random.Range(0, 1) != 1) ? (-3) : 3;
			int num3 = Random.Range(PathLengthCount.Minimum, PathLengthCount.Maximum);
			while (num3 != 0)
			{
				triangleIndex += num2;
				if (triangleIndex >= 0 && triangleIndex < meshHelper.Triangles.Length)
				{
					meshHelper.GetRaycastFromTriangleIndex(triangleIndex, ref hit);
					hit.distance = Random.Range(MeshOffsetRange.Minimum, MeshOffsetRange.Maximum);
					Vector3 vector2 = hit.point + hit.normal * hit.distance;
					float sqrMagnitude = (vector2 - vector).sqrMagnitude;
					if (sqrMagnitude > maximumPathDistanceSquared)
					{
						break;
					}
					if (sqrMagnitude >= num)
					{
						vector = vector2;
						sourcePoints.Add(MeshFilter.transform.TransformPoint(vector2));
						num3--;
						num = Random.Range(MinimumPathDistanceRange.Minimum, MinimumPathDistanceRange.Maximum);
						num *= num;
					}
				}
				else
				{
					num2 = -num2;
					triangleIndex += num2;
					num3--;
				}
			}
		}

		protected override void Start()
		{
			base.Start();
		}

		protected override void Update()
		{
			CheckMesh();
			base.Update();
		}

		public override void CreateLightningBolt(LightningBoltParameters parameters)
		{
			if (meshHelper == null)
			{
				return;
			}
			int num2 = Generations = (parameters.Generations = Mathf.Clamp(Generations, 1, 5));
			sourcePoints.Clear();
			PopulateSourcePoints(sourcePoints);
			if (sourcePoints.Count > 1)
			{
				if (Spline && sourcePoints.Count > 3)
				{
					parameters.Points = new List<Vector3>(sourcePoints.Count * Generations);
					LightningSplineScript.PopulateSpline(parameters.Points, sourcePoints, Generations, DistancePerSegmentHint, Camera);
					parameters.SmoothingFactor = (parameters.Points.Count - 1) / sourcePoints.Count;
				}
				else
				{
					parameters.Points = new List<Vector3>(sourcePoints);
					parameters.SmoothingFactor = 1;
				}
				base.CreateLightningBolt(parameters);
			}
		}
	}
}
