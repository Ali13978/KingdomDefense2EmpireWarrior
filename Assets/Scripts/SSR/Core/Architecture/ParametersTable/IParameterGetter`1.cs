namespace SSR.Core.Architecture.ParametersTable
{
	public interface IParameterGetter<T>
	{
		string Key
		{
			get;
		}

		bool UseKey
		{
			get;
		}

		T DefaultValue
		{
			get;
		}

		T Value
		{
			get;
			set;
		}

		PrimitiveValueType PrimeValueType
		{
			get;
		}

		T GetValue(IParametersTable parametersTable);
	}
}
