using UnityEngine;

namespace SSR.DevTool
{
	public static class TemporaryScriptableObject
	{
		public static T GetTemporaryScriptableObject<T>(string assetName) where T : ScriptableObject
		{
			return (T)null;
		}
	}
}
