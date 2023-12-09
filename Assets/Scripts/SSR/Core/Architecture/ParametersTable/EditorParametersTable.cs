using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Architecture.ParametersTable
{
	public class EditorParametersTable : MonoBehaviour, IParametersTableListableKeys, IParametersTableSetable, IParametersTable
	{
		[SerializeField]
		private bool dispatchEventOnChange = true;

		[SerializeField]
		private bool dispatchEventToChildren = true;

		[SerializeField]
		private bool dispatchEventToInActiveChildren = true;

		[SerializeField]
		private List<ParameterSetter> parameterSetters = new List<ParameterSetter>();

		public List<ParameterSetter> ParameterSetters
		{
			get
			{
				return parameterSetters;
			}
			set
			{
				parameterSetters = value;
			}
		}

		public bool GetBool(string key, bool defaultValue)
		{
			return FindParameter(key, PrimitiveValueType.Bool)?.BoolValue ?? defaultValue;
		}

		public float GetFloat(string key, float defaultValue)
		{
			return FindParameter(key, PrimitiveValueType.Float)?.FloatValue ?? defaultValue;
		}

		public int GetInt(string key, int defaultValue)
		{
			return FindParameter(key, PrimitiveValueType.Integer)?.IntValue ?? defaultValue;
		}

		public string GetString(string key, string defaultValue)
		{
			ParameterSetter parameterSetter = FindParameter(key, PrimitiveValueType.String);
			if (parameterSetter == null)
			{
				return defaultValue;
			}
			return parameterSetter.StringValue;
		}

		public string[] GetIntKeys()
		{
			return GetKeys(PrimitiveValueType.Integer);
		}

		public string[] GetFloatKeys()
		{
			return GetKeys(PrimitiveValueType.Float);
		}

		public string[] GetBoolKeys()
		{
			return GetKeys(PrimitiveValueType.Bool);
		}

		public string[] GetStringKeys()
		{
			return GetKeys(PrimitiveValueType.String);
		}

		public void SetBool(string key, bool value)
		{
			ParameterSetter parameterSetter = FindParameter(key, PrimitiveValueType.Bool);
			if (parameterSetter == null)
			{
				parameterSetters.Add(new ParameterSetter
				{
					Key = key,
					PrimitiveValueType = PrimitiveValueType.Bool,
					BoolValue = value
				});
			}
			else
			{
				parameterSetter.BoolValue = value;
			}
		}

		public void SetFloat(string key, float value)
		{
			ParameterSetter parameterSetter = FindParameter(key, PrimitiveValueType.Float);
			if (parameterSetter == null)
			{
				parameterSetters.Add(new ParameterSetter
				{
					Key = key,
					PrimitiveValueType = PrimitiveValueType.Float,
					FloatValue = value
				});
			}
			else
			{
				parameterSetter.FloatValue = value;
			}
		}

		public void SetInt(string key, int value)
		{
			ParameterSetter parameterSetter = FindParameter(key, PrimitiveValueType.Integer);
			if (parameterSetter == null)
			{
				parameterSetters.Add(new ParameterSetter
				{
					Key = key,
					PrimitiveValueType = PrimitiveValueType.Integer,
					IntValue = value
				});
			}
			else
			{
				parameterSetter.IntValue = value;
			}
		}

		public void SetString(string key, string value)
		{
			ParameterSetter parameterSetter = FindParameter(key, PrimitiveValueType.String);
			if (parameterSetter == null)
			{
				parameterSetters.Add(new ParameterSetter
				{
					Key = key,
					PrimitiveValueType = PrimitiveValueType.String,
					StringValue = value
				});
			}
			else
			{
				parameterSetter.StringValue = value;
			}
		}

		public void Clear()
		{
			parameterSetters.Clear();
		}

		public void DispatchChangeEvent()
		{
			if (dispatchEventOnChange)
			{
				DispatchChangeEvent(dispatchEventToChildren, dispatchEventToInActiveChildren);
			}
		}

		private void DispatchChangeEvent(bool toChidlren, bool toInactiveChildren)
		{
			IParametersTableChangeListener[] array = null;
			array = ((!toChidlren) ? GetComponents<IParametersTableChangeListener>() : GetComponentsInChildren<IParametersTableChangeListener>(toInactiveChildren));
			IParametersTableChangeListener[] array2 = array;
			foreach (IParametersTableChangeListener parametersTableChangeListener in array2)
			{
				parametersTableChangeListener.OnParametersTableChanged(this);
			}
		}

		private ParameterSetter FindParameter(string key, PrimitiveValueType primitiveValueType)
		{
			for (int i = 0; i < ParameterSetters.Count; i++)
			{
				ParameterSetter parameterSetter = ParameterSetters[i];
				if (parameterSetter.Key == key && parameterSetter.PrimitiveValueType == primitiveValueType)
				{
					return ParameterSetters[i];
				}
			}
			return null;
		}

		private string[] GetKeys(PrimitiveValueType primitiveValueType)
		{
			List<string> list = new List<string>();
			foreach (ParameterSetter parameterSetter in ParameterSetters)
			{
				if (parameterSetter.PrimitiveValueType == primitiveValueType)
				{
					list.Add(parameterSetter.Key);
				}
			}
			return list.ToArray();
		}

		private void Reset()
		{
		}
	}
}
