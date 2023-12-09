using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class BuildTowerAnim : MonoBehaviour
	{
		[SerializeField]
		private GameObject statusBar;

		private void Start()
		{
			UpdateFilled(1f);
		}

		public void UpdateFilled(float time)
		{
			StartCoroutine(IUpdateFilled(time));
		}

		private IEnumerator IUpdateFilled(float time)
		{
			Vector3 value = new Vector3
			{
				x = 0f,
				y = 1f,
				z = 1f
			};
			statusBar.transform.localScale = Vector3.zero;
			for (int i = 0; i < 100; i++)
			{
				value.x += 0.01f;
				statusBar.transform.localScale = value;
				yield return new WaitForSeconds(time / 100f);
			}
			OnBuildDone();
		}

		private void OnBuildDone()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
