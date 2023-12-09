using Gameplay;
using Middle;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceVolumeOverrider : MonoBehaviour
{
	[SerializeField]
	[HideInInspector]
	private AudioSource audioSource;

	[SerializeField]
	[Range(0f, 1f)]
	private float normalVolume = 1f;

	private Config config;

	public void Awake()
	{
		config = Config.Instance;
		audioSource = GetComponent<AudioSource>();
	}

	public void Update()
	{
		if (SingletonMonoBehaviour<GameData>.Instance.IsPause)
		{
			audioSource.volume = 0f;
		}
		else
		{
			audioSource.volume = ((!config.Sound) ? 0f : normalVolume);
		}
	}
}
