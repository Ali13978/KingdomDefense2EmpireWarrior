using System;

namespace SSR.Core.Architecture.ParametersTable
{
	[Serializable]
	public class FloatParameterGetter : ParameterGetter<float>
	{
		public override PrimitiveValueType PrimeValueType => PrimitiveValueType.Float;

		protected override float GetTypedValueFromParametersTable(IParametersTable parametersTable, float defaultValue)
		{
			return parametersTable.GetFloat(base.Key, defaultValue);
		}
	}
}
