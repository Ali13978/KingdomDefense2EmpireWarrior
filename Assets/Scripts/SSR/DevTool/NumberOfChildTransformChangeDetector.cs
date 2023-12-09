using UnityEngine;

namespace SSR.DevTool
{
	[ExecuteInEditMode]
	public class NumberOfChildTransformChangeDetector : EditorChangeDetector
	{
		private int savedNumberOfChildren = -1;

		public override void OnReset()
		{
			savedNumberOfChildren = base.transform.childCount;
		}

		private void Update()
		{
			if (savedNumberOfChildren != base.transform.childCount)
			{
				DetectedChanged();
			}
		}
	}
}
