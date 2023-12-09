using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class F3DLightning : MonoBehaviour
{
	public LayerMask layerMask;

	public Texture[] BeamFrames;

	public float FrameStep;

	public bool RandomizeFrames;

	public int Points;

	public float MaxBeamLength;

	public float beamScale;

	public bool AnimateUV;

	public float UVTime;

	public bool Oscillate;

	public float Amplitude;

	public float OscillateTime;

	public Transform rayImpact;

	public Transform rayMuzzle;

	private LineRenderer lineRenderer;

	private RaycastHit hitPoint;

	private int frameNo;

	private int FrameTimerID;

	private int OscillateTimerID;

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

	private void OnSpawned()
	{
		if (BeamFrames.Length > 1)
		{
			Animate();
		}
		if (Oscillate && Points > 0)
		{
			OscillateTimerID = F3DTime.time.AddTimer(OscillateTime, OnOscillate);
		}
		if ((bool)F3DAudioController.instance)
		{
			F3DAudioController.instance.LightningGunLoop(base.transform.position, base.transform);
		}
	}

	private void OnDespawned()
	{
		frameNo = 0;
		if (FrameTimerID != -1)
		{
			F3DTime.time.RemoveTimer(FrameTimerID);
			FrameTimerID = -1;
		}
		if (OscillateTimerID != -1)
		{
			F3DTime.time.RemoveTimer(OscillateTimerID);
			OscillateTimerID = -1;
		}
		if ((bool)F3DAudioController.instance)
		{
			F3DAudioController.instance.LightningGunClose(base.transform.position);
		}
	}

	private void Raycast()
	{
		hitPoint = default(RaycastHit);
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		float x = MaxBeamLength * (beamScale / 10f);
		if (Physics.Raycast(ray, out hitPoint, MaxBeamLength, layerMask))
		{
			beamLength = Vector3.Distance(base.transform.position, hitPoint.point);
			if (!Oscillate)
			{
				lineRenderer.SetPosition(1, new Vector3(0f, 0f, beamLength));
			}
			x = beamLength * (beamScale / 10f);
			ApplyForce(0.1f);
			if ((bool)rayImpact)
			{
				rayImpact.position = hitPoint.point - base.transform.forward * 0.5f;
			}
		}
		else
		{
			beamLength = MaxBeamLength;
			if (!Oscillate)
			{
				lineRenderer.SetPosition(1, new Vector3(0f, 0f, beamLength));
			}
			if ((bool)rayImpact)
			{
				rayImpact.position = base.transform.position + base.transform.forward * beamLength;
			}
		}
		if ((bool)rayMuzzle)
		{
			rayMuzzle.position = base.transform.position + base.transform.forward * 0.1f;
		}
		lineRenderer.material.SetTextureScale("_MainTex", new Vector2(x, 1f));
	}

	private float GetRandomNoise()
	{
		return UnityEngine.Random.Range(0f - Amplitude, Amplitude);
	}

	private void OnFrameStep()
	{
		if (RandomizeFrames)
		{
			frameNo = UnityEngine.Random.Range(0, BeamFrames.Length);
		}
		lineRenderer.material.mainTexture = BeamFrames[frameNo];
		frameNo++;
		if (frameNo == BeamFrames.Length)
		{
			frameNo = 0;
		}
	}

	private void OnOscillate()
	{
		int num = (int)(beamLength / 10f * (float)Points);
		if (num < 2)
		{
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, Vector3.zero);
			lineRenderer.SetPosition(1, new Vector3(0f, 0f, beamLength));
			return;
		}
		lineRenderer.SetVertexCount(num);
		lineRenderer.SetPosition(0, Vector3.zero);
		for (int i = 1; i < num - 1; i++)
		{
			lineRenderer.SetPosition(i, new Vector3(GetRandomNoise(), GetRandomNoise(), beamLength / (float)(num - 1) * (float)i));
		}
		lineRenderer.SetPosition(num - 1, new Vector3(0f, 0f, beamLength));
	}

	private void Animate()
	{
		frameNo = 0;
		lineRenderer.material.mainTexture = BeamFrames[frameNo];
		FrameTimerID = F3DTime.time.AddTimer(FrameStep, OnFrameStep);
		frameNo = 1;
	}

	private void ApplyForce(float force)
	{
		if (hitPoint.rigidbody != null)
		{
			hitPoint.rigidbody.AddForceAtPosition(base.transform.forward * force, hitPoint.point, ForceMode.VelocityChange);
		}
	}

	private void Update()
	{
		if (AnimateUV)
		{
			lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(Time.time * UVTime + initialBeamOffset, 0f));
		}
		Raycast();
	}
}
