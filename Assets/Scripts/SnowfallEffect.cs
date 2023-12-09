using MyCustom;
using UnityEngine;

public class SnowfallEffect : CustomMonoBehaviour
{
	[SerializeField]
	private float playTimeMin;

	[SerializeField]
	private float playTimeMax;

	[SerializeField]
	private float restTimeMin;

	[SerializeField]
	private float restTimeMax;

	private ParticleSystem particle;

	private void Awake()
	{
		particle = GetComponent<ParticleSystem>();
	}

	private void Start()
	{
		PlaySnow();
	}

	private void PlaySnow()
	{
		particle.Play();
		CustomInvoke(StopSnow, Random.Range(playTimeMin, playTimeMax));
	}

	private void StopSnow()
	{
		particle.Stop();
		CustomInvoke(PlaySnow, Random.Range(restTimeMin, restTimeMax));
	}
}
