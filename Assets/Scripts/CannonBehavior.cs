using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
	public CannonBallBehavior cannonBallPrefab;

	public GameObject target;

	private void Start()
	{
		InvokeRepeating("CreateCannonBall", 1f, 1f);
	}

	private void CreateCannonBall()
	{
		CannonBallBehavior cannonBallBehavior = UnityEngine.Object.Instantiate(cannonBallPrefab);
		cannonBallBehavior.Init(GetTargetPosition(target));
	}

	private Vector3 GetTargetPosition(GameObject target)
	{
		return target.transform.position;
	}
}
