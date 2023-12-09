using SSR.Core.Architecture.ParametersTable;

namespace SSR.DevTool
{
	public class ParametersTableChangeDetector : EditorChangeDetector, IParametersTableChangeListener
	{
		public void OnParametersTableChanged(IParametersTable parametersTable)
		{
			DetectedChanged();
		}

		public override void OnReset()
		{
		}
	}
}
