namespace SSR.Core.Architecture.ParametersTable
{
	public interface IParametersTableListableKeys : IParametersTable
	{
		string[] GetIntKeys();

		string[] GetFloatKeys();

		string[] GetBoolKeys();

		string[] GetStringKeys();
	}
}
