using UnityEngine;

public class BulletMagic : MonoBehaviour
{
	private TargetScript target;

	private bool isReady;

	public float deltaTimeStep;

	public void Init(TargetScript _target)
	{
		target = _target;
		isReady = true;
	}

	private void Update()
	{
		if (isReady)
		{
			base.transform.position = Vector3.MoveTowards(base.gameObject.transform.position, target.transform.position, deltaTimeStep);
		}
	}
}
