using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class F3DBeam : MonoBehaviour
{
	public F3DFXType fxType;

	public Texture[] BeamFrames;

	public float FrameStep;

	public bool AnimateUV;

	public float UVTime;

	private LineRenderer lineRenderer;

	private int frameNo;

	private int FrameTimerID;

	private float beamLength;

	private float initialBeamOffset;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		if (!AnimateUV && BeamFrames.Length > 0)
		{
			lineRenderer.material.mainTexture = BeamFrames[0];
		}
		initialBeamOffset = UnityEngine.Random.Range(0f, 5f);
	}

	private void OnFrameStep()
	{
		lineRenderer.material.mainTexture = BeamFrames[frameNo];
		frameNo++;
		if (frameNo == BeamFrames.Length)
		{
			frameNo = 0;
		}
	}

	private void Animate()
	{
		if (BeamFrames.Length > 1)
		{
			frameNo = 0;
			lineRenderer.material.mainTexture = BeamFrames[frameNo];
			FrameTimerID = F3DTime.time.AddTimer(FrameStep, BeamFrames.Length - 1, OnFrameStep);
			frameNo = 1;
		}
	}

	private void Update()
	{
		if (AnimateUV)
		{
			lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * UVTime + initialBeamOffset, 0f));
		}
	}
}
