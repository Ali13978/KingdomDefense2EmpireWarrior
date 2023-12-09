using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	public bool OnlyDeactivate;

	private ParticleSystem particleSystem;

	private float countdown;

	private void Start()
	{
		if (particleSystem == null)
		{
			particleSystem = GetComponent<ParticleSystem>();
		}
	}

	private void OnEnable()
	{
		countdown = 0.5f;
	}

	private void Update()
	{
		countdown -= Time.deltaTime;
		if (countdown <= 0f && !particleSystem.IsAlive(withChildren: true))
		{
			if (OnlyDeactivate)
			{
				base.gameObject.SetActive(value: false);
			}
			else
			{
				base.gameObject.Recycle();
			}
		}
	}
}
