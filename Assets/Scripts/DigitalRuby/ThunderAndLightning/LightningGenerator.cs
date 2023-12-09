using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningGenerator
	{
		public static readonly LightningGenerator GeneratorInstance = new LightningGenerator();

		private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
		{
			if (directionNormalized == Vector3.zero)
			{
				side = Vector3.right;
				return;
			}
			float x = directionNormalized.x;
			float y = directionNormalized.y;
			float z = directionNormalized.z;
			float num = Mathf.Abs(x);
			float num2 = Mathf.Abs(y);
			float num3 = Mathf.Abs(z);
			float num4;
			float num5;
			float num6;
			if (num >= num2 && num2 >= num3)
			{
				num4 = 1f;
				num5 = 1f;
				num6 = (0f - (y * num4 + z * num5)) / x;
			}
			else if (num2 >= num3)
			{
				num6 = 1f;
				num5 = 1f;
				num4 = (0f - (x * num6 + z * num5)) / y;
			}
			else
			{
				num6 = 1f;
				num4 = 1f;
				num5 = (0f - (x * num6 + y * num4)) / z;
			}
			side = new Vector3(num6, num4, num5).normalized;
		}

		protected virtual void OnGenerateLightningBolt(LightningBolt bolt, Vector3 start, Vector3 end, LightningBoltParameters p)
		{
			GenerateLightningBoltStandard(bolt, start, end, p.Generations, p.Generations, 0f, p);
		}

		public bool ShouldCreateFork(LightningBoltParameters p, int generation, int totalGenerations)
		{
			return generation > p.generationWhereForksStop && generation >= totalGenerations - p.forkednessCalculated && (float)p.Random.NextDouble() < p.Forkedness;
		}

		public void CreateFork(LightningBolt bolt, LightningBoltParameters p, int generation, int totalGenerations, Vector3 start, Vector3 midPoint)
		{
			if (ShouldCreateFork(p, generation, totalGenerations))
			{
				Vector3 b = (midPoint - start) * p.ForkMultiplier();
				Vector3 end = midPoint + b;
				GenerateLightningBoltStandard(bolt, midPoint, end, generation, totalGenerations, 0f, p);
			}
		}

		public void GenerateLightningBoltStandard(LightningBolt bolt, Vector3 start, Vector3 end, int generation, int totalGenerations, float offsetAmount, LightningBoltParameters p)
		{
			if (generation < 1)
			{
				return;
			}
			LightningBoltSegmentGroup lightningBoltSegmentGroup = bolt.AddGroup();
			lightningBoltSegmentGroup.Segments.Add(new LightningBoltSegment
			{
				Start = start,
				End = end
			});
			float num = (float)generation / (float)totalGenerations;
			num *= num;
			lightningBoltSegmentGroup.LineWidth = p.TrunkWidth * num;
			lightningBoltSegmentGroup.Generation = generation;
			lightningBoltSegmentGroup.Color = p.Color;
			lightningBoltSegmentGroup.Color.a = (byte)(255f * num);
			lightningBoltSegmentGroup.EndWidthMultiplier = p.EndWidthMultiplier * p.ForkEndWidthMultiplier;
			if (offsetAmount <= 0f)
			{
				offsetAmount = (end - start).magnitude * ((generation != totalGenerations) ? p.ChaosFactorForks : p.ChaosFactor);
			}
			while (generation-- > 0)
			{
				int startIndex = lightningBoltSegmentGroup.StartIndex;
				lightningBoltSegmentGroup.StartIndex = lightningBoltSegmentGroup.Segments.Count;
				for (int i = startIndex; i < lightningBoltSegmentGroup.StartIndex; i++)
				{
					LightningBoltSegment lightningBoltSegment = lightningBoltSegmentGroup.Segments[i];
					start = lightningBoltSegment.Start;
					LightningBoltSegment lightningBoltSegment2 = lightningBoltSegmentGroup.Segments[i];
					end = lightningBoltSegment2.End;
					Vector3 a = (start + end) * 0.5f;
					RandomVector(bolt, ref start, ref end, offsetAmount, p.Random, out Vector3 result);
					a += result;
					lightningBoltSegmentGroup.Segments.Add(new LightningBoltSegment
					{
						Start = start,
						End = a
					});
					lightningBoltSegmentGroup.Segments.Add(new LightningBoltSegment
					{
						Start = a,
						End = end
					});
					CreateFork(bolt, p, generation, totalGenerations, start, a);
				}
				offsetAmount *= 0.5f;
			}
		}

		public Vector3 RandomDirection3D(System.Random r)
		{
			float num = 2f * (float)r.NextDouble() - 1f;
			Vector3 result = RandomDirection2D(r) * Mathf.Sqrt(1f - num * num);
			result.z = num;
			return result;
		}

		public Vector3 RandomDirection2D(System.Random r)
		{
			float f = (float)r.NextDouble() * 2f * (float)Math.PI;
			return new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
		}

		public Vector3 RandomDirection2DXZ(System.Random r)
		{
			float f = (float)r.NextDouble() * 2f * (float)Math.PI;
			return new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f));
		}

		public void RandomVector(LightningBolt bolt, ref Vector3 start, ref Vector3 end, float offsetAmount, System.Random random, out Vector3 result)
		{
			if (bolt.CameraMode == CameraMode.Perspective)
			{
				Vector3 directionNormalized = (end - start).normalized;
				Vector3 side = Vector3.Cross(start, end);
				if (side == Vector3.zero)
				{
					GetPerpendicularVector(ref directionNormalized, out side);
				}
				else
				{
					side.Normalize();
				}
				float d = ((float)random.NextDouble() + 0.1f) * offsetAmount;
				float num = (float)random.NextDouble() * (float)Math.PI;
				directionNormalized *= (float)Math.Sin(num);
				Quaternion rotation = default(Quaternion);
				rotation.x = directionNormalized.x;
				rotation.y = directionNormalized.y;
				rotation.z = directionNormalized.z;
				rotation.w = (float)Math.Cos(num);
				result = rotation * side * d;
			}
			else if (bolt.CameraMode == CameraMode.OrthographicXY)
			{
				end.z = start.z;
				Vector3 normalized = (end - start).normalized;
				Vector3 a = new Vector3(0f - normalized.y, normalized.x, 0f);
				float d2 = (float)random.NextDouble() * offsetAmount * 2f - offsetAmount;
				result = a * d2;
			}
			else
			{
				end.y = start.y;
				Vector3 normalized2 = (end - start).normalized;
				Vector3 a2 = new Vector3(0f - normalized2.z, 0f, normalized2.x);
				float d3 = (float)random.NextDouble() * offsetAmount * 2f - offsetAmount;
				result = a2 * d3;
			}
		}

		public void GenerateLightningBolt(LightningBolt bolt, LightningBoltParameters p)
		{
			GenerateLightningBolt(bolt, p, out Vector3 _, out Vector3 _);
		}

		public void GenerateLightningBolt(LightningBolt bolt, LightningBoltParameters p, out Vector3 start, out Vector3 end)
		{
			start = p.ApplyVariance(p.Start, p.StartVariance);
			end = p.ApplyVariance(p.End, p.EndVariance);
			OnGenerateLightningBolt(bolt, start, end, p);
		}
	}
}
