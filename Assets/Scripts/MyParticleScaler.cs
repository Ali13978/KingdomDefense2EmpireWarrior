using UnityEngine;

public class MyParticleScaler : MonoBehaviour
{
	public float particleScale = 1f;

	public bool alsoScaleGameobject = true;

	private float prevScale = 1f;

	private float startScale;

	private float startScaleFactor;

	private float scaleFactor;

	private void Start()
	{
		startScale = particleScale;
	}

	public void ScaleParticle()
	{
		if (alsoScaleGameobject)
		{
			base.transform.localScale = new Vector3(particleScale, particleScale, particleScale);
		}
		scaleFactor = particleScale;
		ScaleLegacySystems(scaleFactor);
		ScaleShurikenSystems(scaleFactor);
		ScaleTrailRenderers(scaleFactor);
		prevScale = particleScale;
		UnityEngine.Debug.Log(prevScale);
	}

	public void Reset()
	{
		prevScale = startScale;
		UnityEngine.Debug.Log("reset scale!");
	}

	private void ScaleShurikenSystems(float scaleFactor)
	{
	}

	private void ScaleLegacySystems(float scaleFactor)
	{
	}

	private void ScaleTrailRenderers(float scaleFactor)
	{
		TrailRenderer[] componentsInChildren = GetComponentsInChildren<TrailRenderer>();
		TrailRenderer[] array = componentsInChildren;
		foreach (TrailRenderer trailRenderer in array)
		{
			trailRenderer.startWidth = scaleFactor;
			trailRenderer.endWidth = scaleFactor;
		}
	}
}
