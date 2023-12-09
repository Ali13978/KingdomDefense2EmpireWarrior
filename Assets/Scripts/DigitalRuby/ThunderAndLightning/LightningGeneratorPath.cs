using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningGeneratorPath : LightningGenerator
	{
		public static readonly LightningGeneratorPath PathGeneratorInstance = new LightningGeneratorPath();

		public void GenerateLightningBoltPath(LightningBolt bolt, Vector3 start, Vector3 end, LightningBoltParameters p)
		{
			if (p.Points.Count < 2)
			{
				UnityEngine.Debug.LogError("Lightning path should have at least two points");
				return;
			}
			int num = p.Generations;
			int totalGenerations = num;
			float num2 = (num != p.Generations) ? p.ChaosFactorForks : p.ChaosFactor;
			int num3 = p.SmoothingFactor - 1;
			LightningBoltSegmentGroup lightningBoltSegmentGroup = bolt.AddGroup();
			lightningBoltSegmentGroup.LineWidth = p.TrunkWidth;
			lightningBoltSegmentGroup.Generation = num--;
			lightningBoltSegmentGroup.EndWidthMultiplier = p.EndWidthMultiplier;
			lightningBoltSegmentGroup.Color = p.Color;
			p.Start = p.Points[0] + start;
			p.End = p.Points[p.Points.Count - 1] + end;
			end = p.Start;
			for (int i = 1; i < p.Points.Count; i++)
			{
				start = end;
				end = p.Points[i];
				Vector3 a = end - start;
				float num5 = PathGenerator.SquareRoot(a.sqrMagnitude);
				if (num2 > 0f)
				{
					end = ((bolt.CameraMode == CameraMode.Perspective) ? (end + num5 * num2 * RandomDirection3D(p.Random)) : ((bolt.CameraMode != CameraMode.OrthographicXY) ? (end + num5 * num2 * RandomDirection2DXZ(p.Random)) : (end + num5 * num2 * RandomDirection2D(p.Random))));
					a = end - start;
				}
				lightningBoltSegmentGroup.Segments.Add(new LightningBoltSegment
				{
					Start = start,
					End = end
				});
				float offsetAmount = num5 * num2;
				RandomVector(bolt, ref start, ref end, offsetAmount, p.Random, out Vector3 result);
				if (ShouldCreateFork(p, num, totalGenerations))
				{
					Vector3 b = a * p.ForkMultiplier() * num3 * 0.5f;
					Vector3 end2 = end + b + result;
					GenerateLightningBoltStandard(bolt, start, end2, num, totalGenerations, 0f, p);
				}
				if (--num3 == 0)
				{
					num3 = p.SmoothingFactor - 1;
				}
			}
		}

		protected override void OnGenerateLightningBolt(LightningBolt bolt, Vector3 start, Vector3 end, LightningBoltParameters p)
		{
			GenerateLightningBoltPath(bolt, start, end, p);
		}
	}
}
