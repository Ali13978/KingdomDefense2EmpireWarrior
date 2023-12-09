using System;
using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Architecture.Helper
{
	public static class DictionaryBuilder
	{
		public static Dictionary<U, V> GetDictionary<U, V>(GameObject rootGameObject) where V : class
		{
			Dictionary<U, V> dictionary = new Dictionary<U, V>();
			IDictionaryValue<U>[] componentsInChildren = rootGameObject.GetComponentsInChildren<IDictionaryValue<U>>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] is V)
				{
					U dictionaryKey = componentsInChildren[i].DictionaryKey;
					if (dictionary.ContainsKey(dictionaryKey))
					{
						throw new Exception($"Key {dictionaryKey} is duplicated");
					}
					dictionary.Add(dictionaryKey, componentsInChildren[i] as V);
				}
			}
			return dictionary;
		}
	}
}
