using UnityEngine;

namespace WorldMap
{
	public class ScrollViewMapController : MonoBehaviour
	{
		public float originWidth = 1280f;

		private float scrollViewHeight;

		private Vector2 scrollViewDimension;

		private RectTransform rectTransform;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		private void Start()
		{
			SetScrollViewBoundary();
		}

		private void SetScrollViewBoundary()
		{
			float num = (float)Screen.width / (float)Screen.height;
			scrollViewHeight = originWidth / num;
			scrollViewDimension.Set(originWidth, scrollViewHeight);
			rectTransform.sizeDelta = scrollViewDimension;
		}
	}
}
