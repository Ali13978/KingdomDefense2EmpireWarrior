using UnityEngine;

[ExecuteInEditMode]
public class F3DParticleScale : MonoBehaviour
{
	[Range(0f, 20f)]
	public float ParticleScale = 1f;

	public bool ScaleGameobject = true;

	private float prevScale;

	private void Start()
	{
		prevScale = ParticleScale;
	}

	private void ScaleShurikenSystems(float scaleFactor)
	{
	}

	private void ScaleTrailRenderers(float scaleFactor)
	{
		TrailRenderer[] componentsInChildren = GetComponentsInChildren<TrailRenderer>();
		TrailRenderer[] array = componentsInChildren;
		foreach (TrailRenderer trailRenderer in array)
		{
			trailRenderer.startWidth *= scaleFactor;
			trailRenderer.endWidth *= scaleFactor;
		}
	}

	private void Update()
	{
	}
}
