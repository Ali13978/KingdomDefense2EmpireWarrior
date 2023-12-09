using UnityEngine;

public class RGBToGrayscale : MonoBehaviour
{
	[Space]
	[Header("Image material")]
	[SerializeField]
	private Material material;

	public void SwitchToRGB()
	{
		material.SetFloat("_EffectAmount", 0f);
	}

	public void SwitchToGrayscale()
	{
		material.SetFloat("_EffectAmount", 1f);
	}
}
