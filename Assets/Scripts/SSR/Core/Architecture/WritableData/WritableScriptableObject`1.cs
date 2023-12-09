using System;
using UnityEngine;

namespace SSR.Core.Architecture.WritableData
{
	[ExecuteInEditMode]
	public class WritableScriptableObject<T> : ScriptableObject, IWritableData<T>, IWritableScriptableObjectHelper, IInspectorCommandObject where T : new()
	{
		[SerializeField]
		[ReadOnly]
		private string key;

		[Space]
		[SerializeField]
		protected T currentData;

		[SerializeField]
		[InspectorCommand]
		private int saveCurrentData;

		[Space]
		[SerializeField]
		protected T defaultData = new T();

		[NonSerialized]
		private bool loadedData;

		public T Data
		{
			get
			{
				if (!loadedData)
				{
					LoadData();
					loadedData = true;
				}
				return currentData;
			}
			set
			{
				currentData = value;
			}
		}

		public void SaveData()
		{
			WritableDataManagerProvider.GetManager().SaveData(key, currentData);
		}

		private void LoadData()
		{
			IWritableDataManager manager = WritableDataManagerProvider.GetManager();
			if (manager.ContainsKey(key))
			{
				currentData = manager.LoadData<T>(key);
			}
			else
			{
				currentData = BinarySerializationHelper.Clone(defaultData);
			}
			OnDataLoaded();
		}

		private string GetAutoKey()
		{
			throw new NotImplementedException();
		}

		[ContextMenu("SetAutoKey")]
		protected void SetAutoKey()
		{
		}

		protected void CopyDefaultDataToCurrentData()
		{
			currentData = BinarySerializationHelper.Clone(defaultData);
		}

		protected virtual void OnDataLoaded()
		{
		}

		public virtual void OnValidate()
		{
			SetAutoKey();
			key = key.Trim();
		}

		public void Reset()
		{
			SetAutoKey();
		}

		[ContextMenu("LoadCurrentData")]
		public void Editor_LoadCurrentData()
		{
			LoadData();
			SSRLog.Log("Loaded from " + key);
		}

		[ContextMenu("SaveCurrentData")]
		protected void Editor_SaveCurrentData()
		{
			SaveData();
			SSRLog.Log("Saved to " + key);
		}

		[ContextMenu("CopyDefaultDataToCurrentData")]
		protected void Editor_CopyDefaultDataToCurrentData()
		{
			currentData = BinarySerializationHelper.Clone(defaultData);
		}

		void IInspectorCommandObject.ExcuteCommand(int intPara, string stringPara)
		{
			Editor_SaveCurrentData();
		}
	}
}
