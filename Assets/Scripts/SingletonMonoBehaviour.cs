using MyCustom;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : CustomMonoBehaviour where T : CustomMonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if ((Object)instance == (Object)null)
			{
				instance = (Object.FindObjectOfType(typeof(T)) as T);
				if ((Object)instance == (Object)null)
				{
					instance = new GameObject().AddComponent<T>();
					instance.gameObject.name = instance.GetType().Name;
				}
			}
			return instance;
		}
	}
}
