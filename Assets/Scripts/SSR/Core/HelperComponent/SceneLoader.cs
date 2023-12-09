using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SSR.Core.HelperComponent
{
	public class SceneLoader : MonoBehaviour
	{
		[SerializeField]
		private string sceneName = string.Empty;

		[SerializeField]
		private LoadSceneMode loadSceneMode;

		[Header("Async loading")]
		[SerializeField]
		private bool async;

		[SerializeField]
		private float mininumSyncLoadingTime;

		public void Load()
		{
			if (async)
			{
				StartCoroutine(LoadSceneAsync());
			}
			else
			{
				SceneManager.LoadScene(sceneName, loadSceneMode);
			}
		}

		private IEnumerator LoadSceneAsync()
		{
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
			if (mininumSyncLoadingTime == 0f)
			{
				asyncOperation.allowSceneActivation = true;
			}
			else
			{
				asyncOperation.allowSceneActivation = false;
				float timeTracking = 0f;
				while (timeTracking < mininumSyncLoadingTime)
				{
					timeTracking += Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}
				asyncOperation.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
