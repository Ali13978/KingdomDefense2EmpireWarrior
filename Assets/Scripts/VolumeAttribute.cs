using System;
using UnityEngine;

[Serializable]
public class VolumeAttribute
{
	[Range(0f, 1f)]
	public float volumeBGM;

	[Range(0f, 1f)]
	public float volumeUI;

	[Range(0f, 1f)]
	public float volumeUIEffect;

	[Range(0f, 1f)]
	public float volumeGameplayEffect;
}
