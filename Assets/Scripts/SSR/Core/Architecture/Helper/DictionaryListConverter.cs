using System.Collections.Generic;

namespace SSR.Core.Architecture.Helper
{
	public static class DictionaryListConverter
	{
		public static void DictionaryToListWithKeysAtList<T>(List<string> keys, List<T> values, Dictionary<string, T> dictionary)
		{
			values.Clear();
			for (int i = 0; i < keys.Count; i++)
			{
				string key = keys[i];
				if (dictionary.ContainsKey(key))
				{
					values.Add(dictionary[key]);
				}
			}
		}

		public static void DictionaryToList<T>(List<string> keys, List<T> values, Dictionary<string, T> dictionary)
		{
			keys.Clear();
			values.Clear();
			foreach (string key in dictionary.Keys)
			{
				keys.Add(key);
				values.Add(dictionary[key]);
			}
		}

		public static void ListToDictionary<T>(List<string> keys, List<T> values, Dictionary<string, T> dictionary)
		{
			dictionary.Clear();
			int count = values.Count;
			for (int i = 0; i < keys.Count; i++)
			{
				string key = keys[i];
				if (!dictionary.ContainsKey(key))
				{
					if (i < values.Count)
					{
						dictionary[key] = values[i];
					}
					else
					{
						dictionary[key] = default(T);
					}
				}
			}
		}
	}
}
