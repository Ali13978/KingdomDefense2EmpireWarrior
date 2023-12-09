namespace SSR.Core.Architecture.ParametersTable
{
	public static class ParametersTableHelper
	{
		public static void CopyValue(ParameterSetter srcSetter, ParameterSetter dstSetter)
		{
			switch (srcSetter.PrimitiveValueType)
			{
			case PrimitiveValueType.Integer:
				dstSetter.IntValue = srcSetter.IntValue;
				break;
			case PrimitiveValueType.Float:
				dstSetter.FloatValue = srcSetter.FloatValue;
				break;
			case PrimitiveValueType.Bool:
				dstSetter.BoolValue = srcSetter.BoolValue;
				break;
			case PrimitiveValueType.String:
				dstSetter.StringValue = srcSetter.StringValue;
				break;
			}
		}

		public static bool SettersHaveSameValue(ParameterSetter setter1, ParameterSetter setter2)
		{
			if (setter1.PrimitiveValueType != setter2.PrimitiveValueType)
			{
				return false;
			}
			switch (setter1.PrimitiveValueType)
			{
			case PrimitiveValueType.Integer:
				return setter1.IntValue == setter2.IntValue;
			case PrimitiveValueType.Float:
				return setter1.FloatValue == setter2.FloatValue;
			case PrimitiveValueType.Bool:
				return setter1.BoolValue == setter2.BoolValue;
			case PrimitiveValueType.String:
				return setter1.StringValue == setter2.StringValue;
			default:
				return false;
			}
		}

		public static bool IdenticalSetter(ParameterSetter setter1, ParameterSetter setter2)
		{
			return IndenticalKeys(setter1.Key, setter2.Key) && setter1.PrimitiveValueType == setter2.PrimitiveValueType;
		}

		public static bool IndenticalKeys(string key1, string key2)
		{
			return key1.Trim() == key2.Trim();
		}
	}
}
