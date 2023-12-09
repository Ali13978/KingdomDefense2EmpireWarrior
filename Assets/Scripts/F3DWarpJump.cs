using UnityEngine;

public class F3DWarpJump : MonoBehaviour
{
	public ParticleSystem WarpSpark;

	public Transform ShipPos;

	public float ShipJumpSpeed;

	public Vector3 ShipJumpStartPoint;

	public Vector3 ShipJumpEndPoint;

	public bool SendOnSpawned;

	private bool isWarping;

	private void Start()
	{
		if (SendOnSpawned)
		{
			BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnSpawned()
	{
		isWarping = false;
		WarpSpark.transform.localPosition = ShipJumpStartPoint;
		ShipPos.position = WarpSpark.transform.position;
		F3DTime.time.AddTimer(3f, 1, OnWarp);
	}

	private void OnWarp()
	{
		isWarping = true;
	}

	private void ShiftShipPosition()
	{
		WarpSpark.transform.localPosition = Vector3.Lerp(WarpSpark.transform.localPosition, ShipJumpEndPoint, Time.deltaTime * ShipJumpSpeed);
		ShipPos.position = WarpSpark.transform.position;
	}

	private void Update()
	{
		if (isWarping)
		{
			ShiftShipPosition();
		}
	}
}
