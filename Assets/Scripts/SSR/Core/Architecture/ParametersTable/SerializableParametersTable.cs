using SSR.Core.Architecture.Helper;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SSR.Core.Architecture.ParametersTable
{
	[Serializable]
	public class SerializableParametersTable : IParametersTableListableKeys, IParametersTable
	{
		private List<string> intKeys;

		private List<string> floatKeys;

		private List<string> boolKeys;

		private List<string> stringKeys;

		private List<int> intValues;

		private List<float> floatValues;

		private List<bool> boolValues;

		private List<string> stringValues;

		private Dictionary<string, int> intDictionary;

		private Dictionary<string, float> floatDictionary;

		private Dictionary<string, bool> boolDictionary;

		private Dictionary<string, string> stringDictionary;

		public SerializableParametersTable(IParametersTableListableKeys parametersTable)
		{
			InitialDictionaries();
			FillDictionaryByOtherTable(parametersTable, intDictionary);
			FillDictionaryByOtherTable(parametersTable, floatDictionary);
			FillDictionaryByOtherTable(parametersTable, boolDictionary);
			FillDictionaryByOtherTable(parametersTable, stringDictionary);
		}

		public SerializableParametersTable()
		{
			InitialDictionaries();
		}

		[OnDeserializing]
		private void SetValuesBeforeDeserialization(StreamingContext context)
		{
		}

		[OnDeserialized]
		private void SetValuesAfterDeserialization(StreamingContext context)
		{
			InitialDictionaries();
			if (intKeys != null)
			{
				DictionaryListConverter.ListToDictionary(intKeys, intValues, intDictionary);
			}
			if (floatKeys != null)
			{
				DictionaryListConverter.ListToDictionary(floatKeys, floatValues, floatDictionary);
			}
			if (boolKeys != null)
			{
				DictionaryListConverter.ListToDictionary(boolKeys, boolValues, boolDictionary);
			}
			if (stringKeys != null)
			{
				DictionaryListConverter.ListToDictionary(stringKeys, stringValues, stringDictionary);
			}
			FreeLists();
		}

		[OnSerializing]
		private void SetValuesBeforeSerialization(StreamingContext context)
		{
			InitialLists();
			DictionaryListConverter.DictionaryToList(intKeys, intValues, intDictionary);
			DictionaryListConverter.DictionaryToList(floatKeys, floatValues, floatDictionary);
			DictionaryListConverter.DictionaryToList(boolKeys, boolValues, boolDictionary);
			DictionaryListConverter.DictionaryToList(stringKeys, stringValues, stringDictionary);
		}

		[OnSerialized]
		private void SetValuesAfterSerialization(StreamingContext context)
		{
			FreeLists();
		}

		bool IParametersTable.GetBool(string key, bool defaultValue)
		{
			if (boolDictionary.TryGetValue(key, out bool value))
			{
				return value;
			}
			return defaultValue;
		}

		float IParametersTable.GetFloat(string key, float defaultValue)
		{
			if (floatDictionary.TryGetValue(key, out float value))
			{
				return value;
			}
			return defaultValue;
		}

		int IParametersTable.GetInt(string key, int defaultValue)
		{
			if (intDictionary.TryGetValue(key, out int value))
			{
				return value;
			}
			return defaultValue;
		}

		string IParametersTable.GetString(string key, string defaultValue)
		{
			if (stringDictionary.TryGetValue(key, out string value))
			{
				return value;
			}
			return defaultValue;
		}

		string[] IParametersTableListableKeys.GetIntKeys()
		{
			List<string> list = new List<string>();
			foreach (string key in intDictionary.Keys)
			{
				list.Add(key);
			}
			return list.ToArray();
		}

		string[] IParametersTableListableKeys.GetFloatKeys()
		{
			List<string> list = new List<string>();
			foreach (string key in floatDictionary.Keys)
			{
				list.Add(key);
			}
			return list.ToArray();
		}

		string[] IParametersTableListableKeys.GetBoolKeys()
		{
			List<string> list = new List<string>();
			foreach (string key in boolDictionary.Keys)
			{
				list.Add(key);
			}
			return list.ToArray();
		}

		string[] IParametersTableListableKeys.GetStringKeys()
		{
			List<string> list = new List<string>();
			foreach (string key in stringDictionary.Keys)
			{
				list.Add(key);
			}
			return list.ToArray();
		}

		private void InitialDictionaries()
		{
			intDictionary = new Dictionary<string, int>();
			floatDictionary = new Dictionary<string, float>();
			boolDictionary = new Dictionary<string, bool>();
			stringDictionary = new Dictionary<string, string>();
		}

		private void InitialLists()
		{
			intKeys = new List<string>();
			floatKeys = new List<string>();
			boolKeys = new List<string>();
			stringKeys = new List<string>();
			intValues = new List<int>();
			floatValues = new List<float>();
			boolValues = new List<bool>();
			stringValues = new List<string>();
		}

		private void FreeLists()
		{
			intKeys = null;
			floatKeys = null;
			boolKeys = null;
			stringKeys = null;
			intValues = null;
			floatValues = null;
			boolValues = null;
			stringValues = null;
		}

		private void FillDictionaryByOtherTable<T>(IParametersTableListableKeys parametersTable, Dictionary<string, T> dictionary)
		{
			dictionary.Clear();
			string[] keysOfType = parametersTable.GetKeysOfType<T>();
			foreach (string key in keysOfType)
			{
				dictionary[key] = parametersTable.GetValueOfType(key, default(T));
			}
		}
	}
}
