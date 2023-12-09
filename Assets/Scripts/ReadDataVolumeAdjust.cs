using UnityEngine;

public class ReadDataVolumeAdjust : MonoBehaviour
{
	[SerializeField]
	private VolumeAdjust volumeAdjust;

	public float GetVolume_BGM()
	{
		return volumeAdjust.volumeAttribute.volumeBGM;
	}

	public float GetVolume_UI()
	{
		return volumeAdjust.volumeAttribute.volumeUI;
	}

	public float GetVolume_UIEffect()
	{
		return volumeAdjust.volumeAttribute.volumeUIEffect;
	}

	public float GetVolume_GameplayEffect()
	{
		return volumeAdjust.volumeAttribute.volumeGameplayEffect;
	}
}
