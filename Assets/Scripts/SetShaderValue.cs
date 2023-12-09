using UnityEngine;

public class SetShaderValue : MonoBehaviour
{
	private string keyBrightness = "_Brightness";

	private string keyColor = "_Color";

	[ContextMenu("Set brightness")]
	public void SetMaterialBrightnessValue(float brightnessValue)
	{
		GetComponent<Renderer>().material.SetFloat(keyBrightness, brightnessValue);
	}

	[ContextMenu("Set color")]
	public void SetMaterialColorValue(Color colorValue)
	{
		GetComponent<Renderer>().material.SetColor(keyColor, colorValue);
	}
}
