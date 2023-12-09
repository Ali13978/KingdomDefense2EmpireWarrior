using Middle;
using MyCustom;
using UnityEngine;

public class BackgroundMusic : CustomMonoBehaviour
{
	private AudioSource audioSource;

	private ReadDataVolumeAdjust readDataVolumeAdjust;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		readDataVolumeAdjust = GetComponent<ReadDataVolumeAdjust>();
	}

	private void Start()
	{
		audioSource.loop = true;
		audioSource.Play();
	}

	private void Update()
	{
		UpdateVolume();
	}

	private void UpdateVolume()
	{
		audioSource.volume = ((!Config.Instance.Music) ? 0f : readDataVolumeAdjust.GetVolume_BGM());
	}

	public void StopBackgroundMusic()
	{
		audioSource.Stop();
	}

	public void PlayMusicBackground()
	{
		audioSource.Play();
	}
}
