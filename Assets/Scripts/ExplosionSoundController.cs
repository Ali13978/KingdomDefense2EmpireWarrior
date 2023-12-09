using SSR.Core;
using UnityEngine;

public class ExplosionSoundController : MonoBehaviour
{
	public AudioSource[] sfxExplosionPrefab;

	[SerializeField]
	[HideInInspector]
	private AudioSource[] sfxExplosion;

	[SerializeField]
	[HideInInspector]
	private bool isPrototypeInitialized;

	private ISoundFxManager soundFxManager;

	private void Awake()
	{
		soundFxManager = SoundFxManager.Instance;
	}

	private void PrototypeInitialize()
	{
		if (!isPrototypeInitialized)
		{
			isPrototypeInitialized = true;
			sfxExplosion = new AudioSource[sfxExplosionPrefab.Length];
			for (int i = 0; i < sfxExplosionPrefab.Length; i++)
			{
				sfxExplosion[i] = UnityEngine.Object.Instantiate(sfxExplosionPrefab[i]);
				sfxExplosion[i].transform.parent = base.transform;
			}
		}
	}

	private void Update()
	{
	}

	public void PlayExplosionSfx()
	{
		if (sfxExplosionPrefab.Length != 0)
		{
			AudioSource audioSource = sfxExplosionPrefab[Random.Range(0, sfxExplosion.Length)];
			if ((bool)audioSource)
			{
				soundFxManager.PlayEffect(audioSource.clip, 1f);
			}
		}
	}

	private void OnEnable()
	{
		PlayExplosionSfx();
	}
}
