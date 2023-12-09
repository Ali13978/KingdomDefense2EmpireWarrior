using UnityEngine;

public class FollowerManager : MonoBehaviour
{
	private GameObject target;

	private Vector3 offset;

	public void Init(GameObject target, Vector3 offset)
	{
		this.target = target;
		this.offset = offset;
	}

	private void Update()
	{
		base.transform.position = target.transform.position + offset;
	}
}
