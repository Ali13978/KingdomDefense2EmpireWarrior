namespace SSR.Core.Architecture.ParametersTable
{
	public interface IParametersTableSetable : IParametersTable
	{
		void SetInt(string key, int value);

		void SetFloat(string key, float value);

		void SetBool(string key, bool value);

		void SetString(string key, string value);

		void Clear();
	}
}
