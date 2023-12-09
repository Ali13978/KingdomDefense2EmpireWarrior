using System;
using UnityEngine;

namespace SSR.Core.Architecture.ParametersTable
{
	[Serializable]
	public abstract class ParameterGetter<T> : IParameterGetter<T>
	{
		[SerializeField]
		private string key;

		[SerializeField]
		private bool useKey = true;

		[SerializeField]
		private T value;

		[SerializeField]
		private T defaultValue;

		public string Key => key;

		public bool UseKey => useKey;

		public T Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}

		public T DefaultValue => defaultValue;

		public abstract PrimitiveValueType PrimeValueType
		{
			get;
		}

		public T GetValue(IParametersTable parametersTable)
		{
			return GetTypedValue(parametersTable);
		}

		public T UpdateValue(IParametersTable parametersTable)
		{
			Value = GetTypedValue(parametersTable);
			return Value;
		}

		protected T GetTypedValue(IParametersTable parametersTable)
		{
			if (useKey)
			{
				return GetTypedValueFromParametersTable(parametersTable, defaultValue);
			}
			return value;
		}

		protected abstract T GetTypedValueFromParametersTable(IParametersTable parametersTable, T defaultValue);

		public static implicit operator T(ParameterGetter<T> parameterGetter)
		{
			return parameterGetter.value;
		}
	}
}
