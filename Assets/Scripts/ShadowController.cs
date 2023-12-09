using MyCustom;
using UnityEngine;

public class ShadowController : CustomMonoBehaviour
{
	public GameObject target;

	private Vector3 offset;

	private Transform targetObject;

	private void Awake()
	{
		targetObject = target.transform;
		offset = targetObject.position - base.transform.position;
	}

	private void LateUpdate()
	{
		base.transform.position = targetObject.position + offset;
	}
}
