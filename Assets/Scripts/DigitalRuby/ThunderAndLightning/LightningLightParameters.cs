using System;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	[Serializable]
	public class LightningLightParameters
	{
		[Tooltip("Light render mode - leave as auto unless you have special use cases")]
		[HideInInspector]
		public LightRenderMode RenderMode;

		[Tooltip("Color of the light")]
		public Color LightColor = Color.white;

		[Tooltip("What percent of segments should have a light? For performance you may want to keep this small.")]
		[Range(0f, 1f)]
		public float LightPercent = 1E-06f;

		[Tooltip("What percent of lights created should cast shadows?")]
		[Range(0f, 1f)]
		public float LightShadowPercent;

		[Tooltip("Light intensity")]
		[Range(0f, 8f)]
		public float LightIntensity = 0.5f;

		[Tooltip("Bounce intensity")]
		[Range(0f, 8f)]
		public float BounceIntensity;

		[Tooltip("Shadow strength, 0 means all light, 1 means all shadow")]
		[Range(0f, 1f)]
		public float ShadowStrength = 1f;

		[Tooltip("Shadow bias, 0 - 2")]
		[Range(0f, 2f)]
		public float ShadowBias = 0.05f;

		[Tooltip("Shadow normal bias, 0 - 3")]
		[Range(0f, 3f)]
		public float ShadowNormalBias = 0.4f;

		[Tooltip("The range of each light created")]
		public float LightRange;

		[Tooltip("Only light objects that match this layer mask")]
		public LayerMask CullingMask = -1;

		[Tooltip("Offset from camera position when in orthographic mode")]
		[Range(-1000f, 1000f)]
		public float OrthographicOffset;

		[Tooltip("Increase the duration of light fade in compared to the lightning fade.")]
		[Range(0f, 20f)]
		public float FadeInMultiplier = 1f;

		[Tooltip("Increase the duration of light fully lit compared to the lightning fade.")]
		[Range(0f, 20f)]
		public float FadeFullyLitMultiplier = 1f;

		[Tooltip("Increase the duration of light fade out compared to the lightning fade.")]
		[Range(0f, 20f)]
		public float FadeOutMultiplier = 1f;

		public bool HasLight => LightColor.a > 0f && LightIntensity >= 0.01f && LightPercent >= 1E-07f && LightRange > 0.01f;
	}
}
