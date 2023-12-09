using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Ambient Obscurance")]
	internal class ScreenSpaceAmbientObscurance : PostEffectsBase
	{
		[Range(0f, 3f)]
		public float intensity = 0.5f;

		[Range(0.1f, 3f)]
		public float radius = 0.2f;

		[Range(0f, 3f)]
		public int blurIterations = 1;

		[Range(0f, 5f)]
		public float blurFilterDistance = 1.25f;

		[Range(0f, 1f)]
		public int downsample;

		public Texture2D rand;

		public Shader aoShader;

		private Material aoMaterial;

		public override bool CheckResources()
		{
			CheckSupport(needDepth: true);
			aoMaterial = CheckShaderAndCreateMaterial(aoShader, aoMaterial);
			if (!isSupported)
			{
				ReportAutoDisable();
			}
			return isSupported;
		}

		private void OnDisable()
		{
			if ((bool)aoMaterial)
			{
				UnityEngine.Object.DestroyImmediate(aoMaterial);
			}
			aoMaterial = null;
		}

		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Camera component = GetComponent<Camera>();
			Matrix4x4 projectionMatrix = component.projectionMatrix;
			Matrix4x4 inverse = projectionMatrix.inverse;
			Vector4 value = new Vector4(-2f / projectionMatrix[0, 0], -2f / projectionMatrix[1, 1], (1f - projectionMatrix[0, 2]) / projectionMatrix[0, 0], (1f + projectionMatrix[1, 2]) / projectionMatrix[1, 1]);
			if (component.stereoEnabled)
			{
				Matrix4x4 stereoProjectionMatrix = component.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
				Matrix4x4 stereoProjectionMatrix2 = component.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
				Vector4 value2 = new Vector4(-2f / stereoProjectionMatrix[0, 0], -2f / stereoProjectionMatrix[1, 1], (1f - stereoProjectionMatrix[0, 2]) / stereoProjectionMatrix[0, 0], (1f + stereoProjectionMatrix[1, 2]) / stereoProjectionMatrix[1, 1]);
				Vector4 value3 = new Vector4(-2f / stereoProjectionMatrix2[0, 0], -2f / stereoProjectionMatrix2[1, 1], (1f - stereoProjectionMatrix2[0, 2]) / stereoProjectionMatrix2[0, 0], (1f + stereoProjectionMatrix2[1, 2]) / stereoProjectionMatrix2[1, 1]);
				aoMaterial.SetVector("_ProjInfoLeft", value2);
				aoMaterial.SetVector("_ProjInfoRight", value3);
			}
			aoMaterial.SetVector("_ProjInfo", value);
			aoMaterial.SetMatrix("_ProjectionInv", inverse);
			aoMaterial.SetTexture("_Rand", rand);
			aoMaterial.SetFloat("_Radius", radius);
			aoMaterial.SetFloat("_Radius2", radius * radius);
			aoMaterial.SetFloat("_Intensity", intensity);
			aoMaterial.SetFloat("_BlurFilterDistance", blurFilterDistance);
			int width = source.width;
			int height = source.height;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width >> downsample, height >> downsample);
			Graphics.Blit(source, renderTexture, aoMaterial, 0);
			if (downsample > 0)
			{
				RenderTexture temporary = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, temporary, aoMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			for (int i = 0; i < blurIterations; i++)
			{
				aoMaterial.SetVector("_Axis", new Vector2(1f, 0f));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(renderTexture, temporary, aoMaterial, 1);
				RenderTexture.ReleaseTemporary(renderTexture);
				aoMaterial.SetVector("_Axis", new Vector2(0f, 1f));
				renderTexture = RenderTexture.GetTemporary(width, height);
				Graphics.Blit(temporary, renderTexture, aoMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary);
			}
			aoMaterial.SetTexture("_AOTex", renderTexture);
			Graphics.Blit(source, destination, aoMaterial, 2);
			RenderTexture.ReleaseTemporary(renderTexture);
		}
	}
}
