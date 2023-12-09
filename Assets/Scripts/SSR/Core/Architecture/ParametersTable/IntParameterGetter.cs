using System;

namespace SSR.Core.Architecture.ParametersTable
{
	[Serializable]
	public class IntParameterGetter : ParameterGetter<int>
	{
		public override PrimitiveValueType PrimeValueType => PrimitiveValueType.Integer;

		protected override int GetTypedValueFromParametersTable(IParametersTable parametersTable, int defaultValue)
		{
			return parametersTable.GetInt(base.Key, defaultValue);
		}
	}
}
