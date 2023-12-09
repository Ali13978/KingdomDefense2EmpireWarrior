using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameApplication : MonoBehaviour
{
	public static string GameplaySceneName = "Gameplay";

	public static string WorldMapSceneName = "WorldMap";

	public static string MainMenuSceneName = "MainMenu";

	private static GameApplication instance;

	public static GameApplication Instance
	{
		get;
		set;
	}

	private void Awake()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void LoadScene(string levelName)
	{
		StartCoroutine(DoLoad(levelName));
	}

	private IEnumerator DoLoad(string levelName)
	{
		yield return SceneManager.LoadSceneAsync(levelName);
	}

	public void ReloadCurrentScene()
	{
		LoadScene(GameplaySceneName);
	}

	public string GetCurrentSceneName()
	{
		string empty = string.Empty;
		return SceneManager.GetActiveScene().name;
	}
}
