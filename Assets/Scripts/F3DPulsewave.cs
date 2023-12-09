using UnityEngine;

public class F3DPulsewave : MonoBehaviour
{
	public float FadeOutDelay;

	public float FadeOutTime;

	public float ScaleTime;

	public Vector3 ScaleSize;

	public bool DebugLoop;

	private new Transform transform;

	private MeshRenderer meshRenderer;

	private int timerID = -1;

	private bool isFadeOut;

	private bool isEnabled;

	private Color defaultColor;

	private Color color;

	private int tintColorRef;

	private void Awake()
	{
		transform = GetComponent<Transform>();
		meshRenderer = GetComponent<MeshRenderer>();
		tintColorRef = Shader.PropertyToID("_TintColor");
		defaultColor = meshRenderer.material.GetColor(tintColorRef);
	}

	private void Start()
	{
		if (DebugLoop)
		{
			OnSpawned();
		}
	}

	private void OnSpawned()
	{
		transform.localScale = new Vector3(0f, 0f, 0f);
		isEnabled = true;
		isFadeOut = false;
		timerID = F3DTime.time.AddTimer(FadeOutDelay, OnFadeOut);
		meshRenderer.material.SetColor(tintColorRef, defaultColor);
		color = defaultColor;
	}

	private void OnDespawned()
	{
		if (timerID >= 0)
		{
			F3DTime.time.RemoveTimer(timerID);
			timerID = -1;
		}
	}

	private void OnFadeOut()
	{
		isFadeOut = true;
	}

	private void Update()
	{
		if (!isEnabled)
		{
			return;
		}
		transform.localScale = Vector3.Lerp(transform.localScale, ScaleSize, Time.deltaTime * ScaleTime);
		if (!isFadeOut)
		{
			return;
		}
		color = Color.Lerp(color, new Color(0f, 0f, 0f, -0.1f), Time.deltaTime * FadeOutTime);
		meshRenderer.material.SetColor(tintColorRef, color);
		if (color.a <= 0f)
		{
			isEnabled = false;
			if (DebugLoop)
			{
				OnDespawned();
				OnSpawned();
			}
		}
	}
}
