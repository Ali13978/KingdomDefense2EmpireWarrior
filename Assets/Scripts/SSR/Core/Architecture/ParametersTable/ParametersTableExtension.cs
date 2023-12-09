using System;

namespace SSR.Core.Architecture.ParametersTable
{
	public static class ParametersTableExtension
	{
		public static T GetValueOfType<T>(this IParametersTable parametersTable, string key, T defaultValue)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(int))
			{
				return (T)parametersTable.GetValueOfType(key, PrimitiveValueType.Integer, defaultValue);
			}
			if (typeFromHandle == typeof(float))
			{
				return (T)parametersTable.GetValueOfType(key, PrimitiveValueType.Float, defaultValue);
			}
			if (typeFromHandle == typeof(bool))
			{
				return (T)parametersTable.GetValueOfType(key, PrimitiveValueType.Bool, defaultValue);
			}
			if (typeFromHandle == typeof(string))
			{
				return (T)parametersTable.GetValueOfType(key, PrimitiveValueType.String, defaultValue);
			}
			throw new NotSupportedException();
		}

		public static object GetValueOfType(this IParametersTable parametersTable, string key, PrimitiveValueType primeValueType, object defaultValue)
		{
			switch (primeValueType)
			{
			case PrimitiveValueType.Integer:
				return parametersTable.GetInt(key, (int)defaultValue);
			case PrimitiveValueType.Float:
				return parametersTable.GetFloat(key, (float)defaultValue);
			case PrimitiveValueType.Bool:
				return parametersTable.GetBool(key, (bool)defaultValue);
			case PrimitiveValueType.String:
				return parametersTable.GetString(key, (string)defaultValue);
			default:
				throw new NotSupportedException();
			}
		}

		public static string[] GetKeysOfType<T>(this IParametersTableListableKeys parametersTable)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(int))
			{
				return parametersTable.GetKeysOfType(PrimitiveValueType.Integer);
			}
			if (typeFromHandle == typeof(float))
			{
				return parametersTable.GetKeysOfType(PrimitiveValueType.Float);
			}
			if (typeFromHandle == typeof(bool))
			{
				return parametersTable.GetKeysOfType(PrimitiveValueType.Bool);
			}
			if (typeFromHandle == typeof(string))
			{
				return parametersTable.GetKeysOfType(PrimitiveValueType.String);
			}
			throw new NotSupportedException();
		}

		public static string[] GetKeysOfType(this IParametersTableListableKeys parametersTable, PrimitiveValueType primitiveValueType)
		{
			switch (primitiveValueType)
			{
			case PrimitiveValueType.Integer:
				return parametersTable.GetIntKeys();
			case PrimitiveValueType.Float:
				return parametersTable.GetFloatKeys();
			case PrimitiveValueType.Bool:
				return parametersTable.GetBoolKeys();
			case PrimitiveValueType.String:
				return parametersTable.GetStringKeys();
			default:
				throw new NotSupportedException();
			}
		}

		public static void CopyFromOtherTable(this IParametersTableSetable parameterTable, IParametersTableListableKeys otherTable)
		{
			string[] boolKeys = otherTable.GetBoolKeys();
			foreach (string key in boolKeys)
			{
				parameterTable.SetBool(key, otherTable.GetBool(key, defaultValue: false));
			}
			string[] intKeys = otherTable.GetIntKeys();
			foreach (string key2 in intKeys)
			{
				parameterTable.SetInt(key2, otherTable.GetInt(key2, 0));
			}
			string[] floatKeys = otherTable.GetFloatKeys();
			foreach (string key3 in floatKeys)
			{
				parameterTable.SetFloat(key3, otherTable.GetFloat(key3, 0f));
			}
			string[] stringKeys = otherTable.GetStringKeys();
			foreach (string key4 in stringKeys)
			{
				parameterTable.SetString(key4, otherTable.GetString(key4, null));
			}
		}
	}
}
