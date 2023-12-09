using UnityEngine;

namespace SSR.DevTool
{
	[ExecuteInEditMode]
	public class GameObjectActiveChangeDetector : EditorChangeDetector
	{
		private bool savedState;

		public override void OnReset()
		{
			savedState = base.gameObject.activeSelf;
		}

		public void OnEnable()
		{
			if (savedState != base.gameObject.activeSelf)
			{
				DetectedChanged();
			}
		}

		public void OnDisable()
		{
			if (savedState != base.gameObject.activeSelf)
			{
				DetectedChanged();
			}
		}
	}
}
