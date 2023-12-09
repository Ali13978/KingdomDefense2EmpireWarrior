using UnityEngine;

public class BrightnessShaderValue : MonoBehaviour
{
	private string keyBrightness = "_Brightness";

	private string keyColor = "_Color";

	private Renderer ren;

	private void Awake()
	{
		ren = GetComponent<Renderer>();
	}

	public void SetMaterialBrightnessValue(float brightnessValue)
	{
		Material[] materials = ren.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetFloat(keyBrightness, brightnessValue);
		}
	}

	public void SetMaterialColorValue(Color colorValue)
	{
		Material[] materials = ren.materials;
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetColor(keyColor, colorValue);
		}
	}
}
