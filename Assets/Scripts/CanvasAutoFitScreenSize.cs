using UnityEngine;

public class CanvasAutoFitScreenSize : MonoBehaviour
{
	private RectTransform rectTransform;

	private Vector2 canvasSize;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void OnEnable()
	{
		SetCanvasSize();
	}

	private void SetCanvasSize()
	{
		canvasSize.Set(Screen.width, Screen.height);
		rectTransform.sizeDelta = canvasSize;
	}
}
