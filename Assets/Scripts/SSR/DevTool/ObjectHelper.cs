using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SSR.DevTool
{
	public static class ObjectHelper
	{
		public static void SafeDestroy<T>(T targetObject) where T : Object
		{
		}

		public static void SafeDestroy<T>(this Object currentObject, T targetObject) where T : Object
		{
			SafeDestroy(targetObject);
		}

		public static bool IsChildOrSelf(Transform parentTransform, Transform testTransform)
		{
			if (testTransform == null)
			{
				return false;
			}
			if (parentTransform == testTransform)
			{
				return true;
			}
			return IsChildOrSelf(parentTransform, testTransform.parent);
		}

		public static T[] GetAllObjectsInCurrentActiveScene<T>(bool includeInactive)
		{
			GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			List<T> list = new List<T>();
			GameObject[] array = rootGameObjects;
			foreach (GameObject gameObject in array)
			{
				list.AddRange(gameObject.GetComponentsInChildren<T>(includeInactive));
			}
			return list.ToArray();
		}
	}
}
