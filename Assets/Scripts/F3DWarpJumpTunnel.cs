using UnityEngine;

public class F3DWarpJumpTunnel : MonoBehaviour
{
	private new Transform transform;

	private MeshRenderer meshRenderer;

	public float StartDelay;

	public float FadeDelay;

	public Vector3 ScaleTo;

	public float ScaleTime;

	public float ColorTime;

	public float ColorFadeTime;

	public float RotationSpeed;

	private bool grow;

	private float alpha;

	private int alphaID;

	private void Awake()
	{
		transform = GetComponent<Transform>();
		meshRenderer = GetComponent<MeshRenderer>();
		alphaID = Shader.PropertyToID("_Alpha");
	}

	public void OnSpawned()
	{
		grow = false;
		meshRenderer.material.SetFloat(alphaID, 0f);
		F3DTime.time.AddTimer(StartDelay, 1, ToggleGrow);
		F3DTime.time.AddTimer(FadeDelay, 1, ToggleGrow);
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.localRotation *= Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-360, 360));
	}

	private void ToggleGrow()
	{
		grow = !grow;
	}

	private void Update()
	{
		transform.Rotate(0f, 0f, RotationSpeed * Time.deltaTime);
		if (grow)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, ScaleTo, Time.deltaTime * ScaleTime);
			alpha = Mathf.Lerp(alpha, 1f, Time.deltaTime * ColorTime);
			meshRenderer.material.SetFloat(alphaID, alpha);
		}
		else
		{
			alpha = Mathf.Lerp(alpha, 0f, Time.deltaTime * ColorFadeTime);
			meshRenderer.material.SetFloat(alphaID, alpha);
		}
	}
}
