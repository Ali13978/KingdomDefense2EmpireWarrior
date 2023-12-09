using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(Renderer))]
	public class TextureScroller : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private Renderer targetRenderer;

		[SerializeField]
		private Vector2 speed = Vector2.left;

		private Material mainMaterial;

		public void Awake()
		{
			mainMaterial = targetRenderer.material;
		}

		public void Update()
		{
			Vector2 mainTextureOffset = mainMaterial.mainTextureOffset + speed * Time.deltaTime;
			mainTextureOffset.x = Mathf.Repeat(mainTextureOffset.x, 1f);
			mainTextureOffset.y = Mathf.Repeat(mainTextureOffset.y, 1f);
			mainMaterial.mainTextureOffset = mainTextureOffset;
		}

		public void Reset()
		{
			targetRenderer = GetComponent<Renderer>();
		}
	}
}
