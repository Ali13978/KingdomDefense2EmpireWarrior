using DG.Tweening;
using MyCustom;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Frost")]
public class FrostEffect : CustomMonoBehaviour
{
	public float FrostAmount = 0.5f;

	public float EdgeSharpness = 1f;

	public float minFrost;

	public float maxFrost = 1f;

	public float seethroughness = 0.2f;

	public float distortion = 0.1f;

	public Texture2D Frost;

	public Texture2D FrostNormals;

	public Shader Shader;

	private Material material;

	private Tweener tween;

	private void Awake()
	{
		material = new Material(Shader);
		material.SetTexture("_BlendTex", Frost);
		material.SetTexture("_BumpMap", FrostNormals);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!Application.isPlaying)
		{
			material.SetTexture("_BlendTex", Frost);
			material.SetTexture("_BumpMap", FrostNormals);
			EdgeSharpness = Mathf.Max(1f, EdgeSharpness);
		}
		material.SetFloat("_BlendAmount", Mathf.Clamp01(Mathf.Clamp01(FrostAmount) * (maxFrost - minFrost) + minFrost));
		material.SetFloat("_EdgeSharpness", EdgeSharpness);
		material.SetFloat("_SeeThroughness", seethroughness);
		material.SetFloat("_Distortion", distortion);
		Graphics.Blit(source, destination, material);
	}

	public void Init(float duration)
	{
		CancelInvoke("TurnOffEffect");
		tween.Kill();
		base.enabled = true;
		CustomInvoke(TurnOffEffect, duration);
	}

	public void DoEffectIn(float duration, float targetValue)
	{
		base.enabled = true;
		tween = DOTween.To(() => 0f, delegate(float x)
		{
			FrostAmount = x;
		}, targetValue, 0.2f).SetEase(Ease.Linear);
	}

	public void DoEffectOut(float duration, float originValue)
	{
		tween = DOTween.To(() => 0.25f, delegate(float x)
		{
			FrostAmount = x;
		}, originValue, duration).SetEase(Ease.Linear);
	}

	public void TurnOffEffect()
	{
		base.enabled = false;
	}
}
