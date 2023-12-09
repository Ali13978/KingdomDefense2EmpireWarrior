using System;

namespace SSR.Core.Architecture.ParametersTable
{
	[Serializable]
	public class BoolParameterGetter : ParameterGetter<bool>
	{
		public override PrimitiveValueType PrimeValueType => PrimitiveValueType.Bool;

		protected override bool GetTypedValueFromParametersTable(IParametersTable parametersTable, bool defaultValue)
		{
			return parametersTable.GetBool(base.Key, defaultValue);
		}
	}
}
