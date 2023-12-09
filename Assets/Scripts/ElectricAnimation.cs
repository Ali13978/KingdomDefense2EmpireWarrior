using MyCustom;
using MyTween;
using UnityEngine;

public class ElectricAnimation : CustomMonoBehaviour
{
	private GameObject target;

	private ParticleSystem particle;

	private float startSize;

	private float parentScale;

	private bool isRunning;

	private void Awake()
	{
		particle = GetComponent<ParticleSystem>();
		Vector3 localScale = base.transform.parent.localScale;
		parentScale = localScale.x;
		startSize = particle.startSize;
		StopImmediate();
	}

	private void Update()
	{
		if ((bool)target && target.activeSelf)
		{
			ResizeFolowTarget();
		}
		else
		{
			StopImmediate();
		}
	}

	public void Init(GameObject target, float lifeTime)
	{
		this.target = target;
		Run();
		CustomInvoke(OnReturnPool, lifeTime);
	}

	private void Run()
	{
		if (!particle)
		{
			particle = GetComponent<ParticleSystem>();
		}
		isRunning = true;
		base.gameObject.SetActive(value: true);
	}

	private void StopImmediate()
	{
		target = null;
		isRunning = false;
		base.gameObject.SetActive(value: false);
	}

	private void ResizeFolowTarget()
	{
		float num = Vector3.Distance(base.transform.position, target.transform.position);
		particle.startSize = num * startSize;
		base.transform.MyTweenLookAtDirect2D(target.transform.position);
	}

	public void OnReturnPool()
	{
		StopImmediate();
	}
}
