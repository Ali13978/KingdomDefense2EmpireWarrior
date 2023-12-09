namespace SSR.Core.Architecture.ParametersTable
{
	public interface IParametersTable
	{
		int GetInt(string key, int defaultValue);

		float GetFloat(string key, float defaultValue);

		bool GetBool(string key, bool defaultValue);

		string GetString(string key, string defaultValue);
	}
}
