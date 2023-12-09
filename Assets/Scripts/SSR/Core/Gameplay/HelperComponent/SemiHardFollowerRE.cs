using SSR.Core.Architecture.ParametersTable;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class SemiHardFollowerRE : SemiHardFollower, IParametersTableChangeListener
	{
		[Header("Parameters")]
		[SerializeField]
		private FloatParameterGetter offsetXParameter;

		[SerializeField]
		private FloatParameterGetter offsetYParameter;

		[SerializeField]
		private FloatParameterGetter offsetZParameter;

		public void OnParametersTableChanged(IParametersTable parametersTable)
		{
			base.Offset = new Vector3(offsetXParameter.GetValue(parametersTable), offsetYParameter.GetValue(parametersTable), offsetZParameter.GetValue(parametersTable));
		}
	}
}
