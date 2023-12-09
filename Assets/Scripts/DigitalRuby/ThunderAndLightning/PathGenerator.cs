using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public static class PathGenerator
	{
		public const int MinPointsForSpline = 4;

		public static bool Is2D;

		private const float curveMultiplier = 3f;

		private const float splineMultiplier1 = -3f;

		private const float splineMultiplier2 = 3f;

		private const float splineMultiplier3 = 2f;

		private const float splineDistanceClamp = 1f;

		private const float splineEpsilon = 0.0001f;

		public static float SquareRoot(float x)
		{
			return (float)Math.Sqrt(x);
		}

		private static float Distance2D(ref Vector3 point1, ref Vector3 point2)
		{
			float num = point2.x - point1.x;
			float num2 = point2.y - point1.y;
			return SquareRoot(num * num + num2 * num2);
		}

		private static float Distance3D(ref Vector3 point1, ref Vector3 point2)
		{
			float num = point2.x - point1.x;
			float num2 = point2.y - point1.y;
			float num3 = point2.z - point1.z;
			return SquareRoot(num * num + num2 * num2 + num3 * num3);
		}

		private static void GetCurvePoint2D(ref Vector3 start, ref Vector3 end, ref Vector3 ctr1, ref Vector3 ctr2, float t, out Vector3 point)
		{
			float num = t * t;
			float num2 = num * t;
			float num3 = 3f * (ctr1.x - start.x);
			float num4 = 3f * (ctr1.y - start.y);
			float num5 = 3f * (ctr2.x - ctr1.x) - num3;
			float num6 = 3f * (ctr2.y - ctr1.y) - num4;
			float num7 = end.x - start.x - num3 - num5;
			float num8 = end.y - start.y - num4 - num6;
			float x = start.x + num3 * t + num5 * num + num7 * num2;
			float y = start.y + num4 * t + num6 * num + num8 * num2;
			point = new Vector3(x, y, 0f);
		}

		private static void GetCurvePoint3D(ref Vector3 start, ref Vector3 end, ref Vector3 ctr1, ref Vector3 ctr2, float t, out Vector3 point)
		{
			float num = t * t;
			float num2 = num * t;
			float num3 = (ctr1.x - start.x) * 3f;
			float num4 = (ctr1.y - start.y) * 3f;
			float num5 = (ctr1.z - start.z) * 3f;
			float num6 = (ctr2.x - ctr1.x) * 3f - num3;
			float num7 = (ctr2.y - ctr1.y) * 3f - num4;
			float num8 = (ctr2.z - ctr1.z) * 3f - num5;
			float num9 = end.x - start.x - num3 - num6;
			float num10 = end.y - start.y - num4 - num7;
			float num11 = end.z - start.z - num5 - num8;
			float x = start.x + num3 * t + num6 * num + num9 * num2;
			float y = start.y + num4 * t + num7 * num + num10 * num2;
			float z = start.z + num5 * t + num8 * num + num11 * num2;
			point = new Vector3(x, y, z);
		}

		private static void CalculateNonuniformCatmullRom(float p1, float p2, float p3, float p4, float distance1, float distance2, float distance3, out Vector4 point)
		{
			float num = ((p2 - p1) / distance1 - (p3 - p1) / (distance1 + distance2) + (p3 - p2) / distance2) * distance2;
			float num2 = ((p3 - p2) / distance2 - (p4 - p2) / (distance2 + distance3) + (p4 - p3) / distance3) * distance2;
			point = new Vector4(p2, num, -3f * p2 + 3f * p3 - (num2 + 2f * num), num + num2 + (2f * p2 - 2f * p3));
		}

		private static float CalculatePolynomial(ref Vector4 point, float t)
		{
			float num = t * t;
			float num2 = num * t;
			return point.w * num2 + point.z * num + point.y * t + point.x;
		}

		private static void ClampSplineDistances(ref float distance1, ref float distance2, ref float distance3)
		{
			if (distance2 < 0.0001f)
			{
				distance2 = 1f;
			}
			if (distance1 < 0.0001f)
			{
				distance1 = distance2;
			}
			if (distance3 < 0.0001f)
			{
				distance3 = distance2;
			}
		}

		private static void GetSplinePoint2D(ref Vector3 point1, ref Vector3 point2, ref Vector3 point3, ref Vector3 point4, float t, out Vector3 point)
		{
			float distance = Distance2D(ref point1, ref point2);
			float distance2 = Distance2D(ref point2, ref point3);
			float distance3 = Distance2D(ref point3, ref point4);
			ClampSplineDistances(ref distance, ref distance2, ref distance3);
			CalculateNonuniformCatmullRom(point1.x, point2.x, point3.x, point4.x, distance, distance2, distance3, out Vector4 point5);
			CalculateNonuniformCatmullRom(point1.y, point2.y, point3.y, point4.y, distance, distance2, distance3, out Vector4 point6);
			point = new Vector3(CalculatePolynomial(ref point5, t), CalculatePolynomial(ref point6, t), 0f);
		}

		private static void GetSplinePoint3D(ref Vector3 point1, ref Vector3 point2, ref Vector3 point3, ref Vector3 point4, float t, out Vector3 point)
		{
			float distance = Distance3D(ref point1, ref point2);
			float distance2 = Distance3D(ref point2, ref point3);
			float distance3 = Distance3D(ref point3, ref point4);
			ClampSplineDistances(ref distance, ref distance2, ref distance3);
			CalculateNonuniformCatmullRom(point1.x, point2.x, point3.x, point4.x, distance, distance2, distance3, out Vector4 point5);
			CalculateNonuniformCatmullRom(point1.y, point2.y, point3.y, point4.y, distance, distance2, distance3, out Vector4 point6);
			CalculateNonuniformCatmullRom(point1.z, point2.z, point3.z, point4.z, distance, distance2, distance3, out Vector4 point7);
			point = new Vector3(CalculatePolynomial(ref point5, t), CalculatePolynomial(ref point6, t), CalculatePolynomial(ref point7, t));
		}

		public static float CreateCurve(ICollection<Vector3> path, Vector3 start, Vector3 end, Vector3 ctr1, Vector3 ctr2, int numberOfSegments, float startT)
		{
			numberOfSegments = Math.Min(1024, Math.Max(numberOfSegments, 4));
			float num = 1f / (float)(numberOfSegments + 1);
			float num2;
			Vector3 point;
			if (Is2D)
			{
				for (num2 = startT; num2 <= 1f; num2 += num)
				{
					GetCurvePoint2D(ref start, ref end, ref ctr1, ref ctr2, num2, out point);
					path.Add(point);
				}
			}
			else
			{
				for (num2 = startT; num2 <= 1f; num2 += num)
				{
					GetCurvePoint3D(ref start, ref end, ref ctr1, ref ctr2, num2, out point);
					path.Add(point);
				}
			}
			return num2 - 1f;
		}

		public static bool CreateSpline(ICollection<Vector3> path, IList<Vector3> points, int numberOfSegments, bool closePath)
		{
			if (points.Count < 4)
			{
				return false;
			}
			numberOfSegments = Math.Min(1024, Math.Max(numberOfSegments, 4));
			int num = (!closePath) ? (points.Count - 1) : points.Count;
			int num2 = closePath ? 1 : 0;
			float num3 = 1f / (float)numberOfSegments * (float)num;
			float num4 = 0f;
			for (int i = 0; i < num; i++)
			{
				int index = (i != 0) ? (i - 1) : (num - num2);
				int num5 = i + 1;
				int num6 = i + 2;
				if (closePath && num5 > num - 1)
				{
					num5 -= num;
				}
				if (num6 > num - 1)
				{
					num6 = ((!closePath) ? num : (num6 - num));
				}
				Vector3 point = points[index];
				Vector3 point2 = points[i];
				Vector3 point3 = points[num5];
				Vector3 point4 = points[num6];
				float num7;
				Vector3 point5;
				if (Is2D)
				{
					for (num7 = num4; num7 <= 1f; num7 += num3)
					{
						GetSplinePoint2D(ref point, ref point2, ref point3, ref point4, num7, out point5);
						path.Add(point5);
					}
				}
				else
				{
					for (num7 = num4; num7 <= 1f; num7 += num3)
					{
						GetSplinePoint3D(ref point, ref point2, ref point3, ref point4, num7, out point5);
						path.Add(point5);
					}
				}
				num4 = num7 - 1f;
			}
			return true;
		}

		public static bool CreateSplineWithSegmentDistance(ICollection<Vector3> path, IList<Vector3> points, float distancePerSegment, bool closePath)
		{
			if (points.Count < 4 || distancePerSegment <= 0f)
			{
				return false;
			}
			int num = (!closePath) ? (points.Count - 1) : points.Count;
			int num2 = closePath ? 1 : 0;
			float num3 = 0f;
			for (int i = 0; i < num; i++)
			{
				int index = (i != 0) ? (i - 1) : (num - num2);
				int num4 = i + 1;
				int num5 = i + 2;
				if (closePath && num4 > num - 1)
				{
					num4 -= num;
				}
				if (num5 > num - 1)
				{
					num5 = ((!closePath) ? num : (num5 - num));
				}
				Vector3 point = points[index];
				Vector3 point2 = points[i];
				Vector3 point3 = points[num4];
				Vector3 point4 = points[num5];
				Vector3 point5;
				if (Is2D)
				{
					float value = 1f / (Distance2D(ref point3, ref point2) / distancePerSegment);
					value = Mathf.Clamp(value, 0.00390625f, 1f);
					for (float num6 = num3; num6 <= 1f; num6 += value)
					{
						GetSplinePoint2D(ref point, ref point2, ref point3, ref point4, num6, out point5);
						path.Add(point5);
					}
				}
				else
				{
					float value = 1f / (Distance3D(ref point3, ref point2) / distancePerSegment);
					value = Mathf.Clamp(value, 0.00390625f, 1f);
					for (float num6 = num3; num6 <= 1f; num6 += value)
					{
						GetSplinePoint3D(ref point, ref point2, ref point3, ref point4, num6, out point5);
						path.Add(point5);
					}
				}
				num3 = 0f;
			}
			return true;
		}
	}
}
