using SSR.Core.Architecture.ParametersTable;
using UnityEngine;

namespace SSR.DevTool.SceneViewUtility
{
	public class LineGizmosIllustrator : GizmosIllustrator, IParametersTableChangeListener
	{
		[SerializeField]
		private FloatParameterGetter offsetX = new FloatParameterGetter();

		[SerializeField]
		private FloatParameterGetter offsetY = new FloatParameterGetter();

		[SerializeField]
		private FloatParameterGetter leftLength = new FloatParameterGetter();

		[SerializeField]
		private FloatParameterGetter rightLength = new FloatParameterGetter();

		[SerializeField]
		private FloatParameterGetter upLength = new FloatParameterGetter();

		[SerializeField]
		private FloatParameterGetter downLength = new FloatParameterGetter();

		protected override void DrawGizmos()
		{
			Vector2 vector = base.TargetTransform.position;
			vector.x += offsetX.Value;
			vector.y += offsetY.Value;
			Gizmos.DrawLine(vector, vector + Vector2.left * leftLength);
			Gizmos.DrawLine(vector, vector + Vector2.right * rightLength);
			Gizmos.DrawLine(vector, vector + Vector2.up * upLength);
			Gizmos.DrawLine(vector, vector + Vector2.down * downLength);
		}

		public void OnParametersTableChanged(IParametersTable parametersTable)
		{
			offsetX.UpdateValue(parametersTable);
			offsetY.UpdateValue(parametersTable);
			leftLength.UpdateValue(parametersTable);
			rightLength.UpdateValue(parametersTable);
			upLength.UpdateValue(parametersTable);
			downLength.UpdateValue(parametersTable);
			ForceDraw();
		}
	}
}
