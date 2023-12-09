using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[Serializable]
	public sealed class LightningBoltParameters
	{
		private static int randomSeed;

		private static readonly List<LightningBoltParameters> cache;

		internal int generationWhereForksStop;

		internal int forkednessCalculated;

		internal LightningBoltQualitySetting quality;

		internal float delaySeconds;

		internal int maxLights;

		public static float Scale;

		public static readonly Dictionary<int, LightningQualityMaximum> QualityMaximums;

		public LightningGenerator Generator;

		public Vector3 Start;

		public Vector3 End;

		public Vector3 StartVariance;

		public Vector3 EndVariance;

		public Action<LightningCustomTransformStateInfo> CustomTransform;

		private int generations;

		public float LifeTime;

		public float Delay;

		public RangeOfFloats DelayRange;

		public float ChaosFactor;

		public float ChaosFactorForks = -1f;

		public float TrunkWidth;

		public float EndWidthMultiplier = 0.5f;

		public float Intensity = 1f;

		public float GlowIntensity;

		public float GlowWidthMultiplier;

		public float Forkedness;

		public int GenerationWhereForksStopSubtractor = 5;

		public Color32 Color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private System.Random random;

		private System.Random currentRandom;

		private System.Random randomOverride;

		public float FadePercent = 0.15f;

		public float FadeInMultiplier = 1f;

		public float FadeFullyLitMultiplier = 1f;

		public float FadeOutMultiplier = 1f;

		private float growthMultiplier;

		public float ForkLengthMultiplier = 0.6f;

		public float ForkLengthVariance = 0.2f;

		public float ForkEndWidthMultiplier = 1f;

		public LightningLightParameters LightParameters;

		public int SmoothingFactor;

		public int Generations
		{
			get
			{
				return generations;
			}
			set
			{
				int b = Mathf.Clamp(value, 1, 8);
				if (quality == LightningBoltQualitySetting.UseScript)
				{
					generations = b;
					return;
				}
				int qualityLevel = QualitySettings.GetQualityLevel();
				if (QualityMaximums.TryGetValue(qualityLevel, out LightningQualityMaximum value2))
				{
					generations = Mathf.Min(value2.MaximumGenerations, b);
					return;
				}
				generations = b;
				UnityEngine.Debug.LogError("Unable to read lightning quality settings from level " + qualityLevel.ToString());
			}
		}

		public System.Random Random
		{
			get
			{
				return currentRandom;
			}
			set
			{
				random = (value ?? random);
				currentRandom = (randomOverride ?? random);
			}
		}

		public System.Random RandomOverride
		{
			get
			{
				return randomOverride;
			}
			set
			{
				randomOverride = value;
				currentRandom = (randomOverride ?? random);
			}
		}

		public float GrowthMultiplier
		{
			get
			{
				return growthMultiplier;
			}
			set
			{
				growthMultiplier = Mathf.Clamp(value, 0f, 0.999f);
			}
		}

		public List<Vector3> Points
		{
			get;
			set;
		}

		static LightningBoltParameters()
		{
			randomSeed = Environment.TickCount;
			cache = new List<LightningBoltParameters>();
			Scale = 1f;
			QualityMaximums = new Dictionary<int, LightningQualityMaximum>();
			string[] names = QualitySettings.names;
			for (int i = 0; i < names.Length; i++)
			{
				switch (i)
				{
				case 0:
					QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 3,
						MaximumLightPercent = 0f,
						MaximumShadowPercent = 0f
					};
					break;
				case 1:
					QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 4,
						MaximumLightPercent = 0f,
						MaximumShadowPercent = 0f
					};
					break;
				case 2:
					QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 5,
						MaximumLightPercent = 0.1f,
						MaximumShadowPercent = 0f
					};
					break;
				case 3:
					QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 5,
						MaximumLightPercent = 0.1f,
						MaximumShadowPercent = 0f
					};
					break;
				case 4:
					QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 6,
						MaximumLightPercent = 0.05f,
						MaximumShadowPercent = 0.1f
					};
					break;
				case 5:
					QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 7,
						MaximumLightPercent = 0.025f,
						MaximumShadowPercent = 0.05f
					};
					break;
				default:
					QualityMaximums[i] = new LightningQualityMaximum
					{
						MaximumGenerations = 8,
						MaximumLightPercent = 0.025f,
						MaximumShadowPercent = 0.05f
					};
					break;
				}
			}
		}

		public LightningBoltParameters()
		{
			random = (currentRandom = new System.Random(randomSeed++));
		}

		public float ForkMultiplier()
		{
			return (float)Random.NextDouble() * ForkLengthVariance + ForkLengthMultiplier;
		}

		public Vector3 ApplyVariance(Vector3 pos, Vector3 variance)
		{
			return new Vector3(pos.x + ((float)Random.NextDouble() * 2f - 1f) * variance.x, pos.y + ((float)Random.NextDouble() * 2f - 1f) * variance.y, pos.z + ((float)Random.NextDouble() * 2f - 1f) * variance.z);
		}

		public void Reset()
		{
			Start = (End = Vector3.zero);
			Points = null;
			Generator = null;
			SmoothingFactor = 0;
			RandomOverride = null;
			CustomTransform = null;
		}

		public static LightningBoltParameters GetOrCreateParameters()
		{
			LightningBoltParameters result;
			if (cache.Count == 0)
			{
				result = new LightningBoltParameters();
			}
			else
			{
				int index = cache.Count - 1;
				result = cache[index];
				cache.RemoveAt(index);
			}
			return result;
		}

		public static void ReturnParametersToCache(LightningBoltParameters p)
		{
			if (!cache.Contains(p))
			{
				p.Reset();
				cache.Add(p);
			}
		}
	}
}
