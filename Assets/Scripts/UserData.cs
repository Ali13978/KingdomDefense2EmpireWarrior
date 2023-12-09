using UnityEngine;

public class UserData : MonoBehaviour
{
	public static UserData Instance
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
}
