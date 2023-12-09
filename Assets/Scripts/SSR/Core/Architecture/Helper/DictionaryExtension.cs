using System.Collections.Generic;

namespace SSR.Core.Architecture.Helper
{
	public static class DictionaryExtension
	{
		public static TValue TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
		{
			if (!dictionary.TryGetValue(key, out TValue value))
			{
				return default(TValue);
			}
			return value;
		}
	}
}
