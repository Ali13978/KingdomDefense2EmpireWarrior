using System;
using UnityEngine;

public class ParticleCustomController : MonoBehaviour
{
	[HideInInspector]
	public Transform target;

	public bool isRotationFolowTarget;

	public float deltaRotation;

	private ParticleSystem parSystem;

	private void Awake()
	{
		parSystem = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if ((bool)target && target.gameObject.activeSelf && target.gameObject.activeSelf && isRotationFolowTarget)
		{
			UpdateRotationFolowTarget();
		}
	}

	private void UpdateRotationFolowTarget()
	{
		ParticleSystem particleSystem = parSystem;
		Vector3 eulerAngles = target.eulerAngles;
		particleSystem.startRotation = (0f - eulerAngles.z) * (float)Math.PI / 180f;
	}

	public void Init(Transform target)
	{
		this.target = target;
	}
}
