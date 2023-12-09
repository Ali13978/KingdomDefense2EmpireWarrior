using System;

namespace SSR.Core.Architecture.ParametersTable
{
	[Serializable]
	public class StringParameterGetter : ParameterGetter<string>
	{
		public override PrimitiveValueType PrimeValueType => PrimitiveValueType.String;

		protected override string GetTypedValueFromParametersTable(IParametersTable parametersTable, string defaultValue)
		{
			return parametersTable.GetString(base.Key, defaultValue);
		}
	}
}
