using UnityEngine;

public class CanvasMultiAspectRatio : MonoBehaviour
{
	private RectTransform rectTransform;

	[SerializeField]
	private float sampleWidthRatio = 16f;

	[SerializeField]
	private float sampleHeightRatio = 9f;

	private float sampleAspectRatio;

	private float realityAspectRatio;

	private float sampleHeight;

	private float realityHeight;

	private float realityWidth;

	private Vector2 canvasScale;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	private void Start()
	{
		SmartSize();
	}

	private void SmartSize()
	{
		Vector2 sizeDelta = rectTransform.sizeDelta;
		sampleHeight = sizeDelta.y;
		sampleAspectRatio = sampleWidthRatio / sampleHeightRatio;
		realityAspectRatio = (float)Screen.width / (float)Screen.height;
		realityHeight = sampleHeight / (realityAspectRatio / sampleAspectRatio);
		realityWidth = realityHeight * sampleAspectRatio;
		if (!(realityWidth > 1280f))
		{
			canvasScale.Set(sampleAspectRatio / realityAspectRatio, sampleAspectRatio / realityAspectRatio);
			rectTransform.localScale = canvasScale;
		}
	}
}
