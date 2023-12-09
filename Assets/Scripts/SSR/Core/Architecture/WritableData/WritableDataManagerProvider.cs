using System.Collections.Generic;

namespace SSR.Core.Architecture.WritableData
{
	public static class WritableDataManagerProvider
	{
		private class WritableDataManager : IWritableDataManager
		{
			private const string KeysListKey = "DatakeyList";

			private List<string> keys;

			public WritableDataManager()
			{
				LoadKeysList();
			}

			public bool ContainsKey(string key)
			{
				return keys.Contains(key);
			}

			public T LoadData<T>(string key)
			{
				byte[] bytes = PersistentFileHelper.LoadFile(GetFileName(key));
				return BinarySerializationHelper.Deserialize<T>(bytes);
			}

			public void SaveData<T>(string key, T data)
			{
				byte[] bytes = BinarySerializationHelper.Serialize(data);
				PersistentFileHelper.SaveFile(bytes, GetFileName(key));
				if (!keys.Contains(key))
				{
					keys.Add(key);
					SavekeysList();
				}
			}

			public void DeleteData(string key)
			{
				keys.Remove(key);
				SavekeysList();
			}

			public void DeleteAllData()
			{
				keys = new List<string>();
				SavekeysList();
			}

			private void LoadKeysList()
			{
				string fileName = GetFileName("DatakeyList");
				if (PersistentFileHelper.FileExist(fileName))
				{
					byte[] bytes = PersistentFileHelper.LoadFile(fileName);
					keys = BinarySerializationHelper.Deserialize<List<string>>(bytes);
				}
				else
				{
					keys = new List<string>();
				}
			}

			private void SavekeysList()
			{
				byte[] bytes = BinarySerializationHelper.Serialize(keys);
				PersistentFileHelper.SaveFile(bytes, GetFileName("DatakeyList"));
			}

			private string GetFileName(string key)
			{
				return $"WritableData_{key}.SSR";
			}
		}

		private static IWritableDataManager manager;

		public static IWritableDataManager GetManager()
		{
			if (manager == null)
			{
				manager = new WritableDataManager();
			}
			return manager;
		}
	}
}
