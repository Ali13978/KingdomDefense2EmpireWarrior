using Data;
using MyCustom;
using UnityEngine;

namespace WorldMap
{
	public class WorldMapScrollViewSnap : CustomMonoBehaviour
	{
		[SerializeField]
		private RectTransform Content;

		[SerializeField]
		private float contentMinValue;

		[SerializeField]
		private float contentMaxValue;

		private Vector2 contentPosition;

		private int currentMapIDFocus;

		[Space]
		[Header("Clamp scrollview")]
		[SerializeField]
		private RectTransform viewPort;

		[SerializeField]
		private ScrollViewMapController scrollViewMapController;

		private Vector2 currentPosition;

		private float scaleRatio;

		private float clampedYValue;

		private void Start()
		{
			currentMapIDFocus = ReadWriteDataMap.Instance.GetLastMapIDPlayed();
		}

		private void Update()
		{
			ClampPosition();
		}

		private void ClampPosition()
		{
			currentPosition = Content.anchoredPosition3D;
			currentPosition.x = Mathf.Clamp(currentPosition.x, contentMinValue, contentMaxValue);
			scaleRatio = scrollViewMapController.originWidth / 1280f;
			Vector2 sizeDelta = Content.sizeDelta;
			float y = sizeDelta.y;
			Vector2 sizeDelta2 = viewPort.sizeDelta;
			clampedYValue = (y - sizeDelta2.y / scaleRatio) / 2f;
			currentPosition.y = Mathf.Clamp(currentPosition.y, 0f - clampedYValue, clampedYValue);
			Content.anchoredPosition3D = currentPosition;
		}
	}
}
