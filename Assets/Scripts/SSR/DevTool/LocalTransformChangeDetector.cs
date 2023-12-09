using UnityEngine;

namespace SSR.DevTool
{
	[ExecuteInEditMode]
	public class LocalTransformChangeDetector : EditorChangeDetector
	{
		private Vector3 savedLocalPosition;

		private Vector3 savedLocalRotation;

		private Vector3 savedLocalScale;

		public override void OnReset()
		{
			SaveState();
		}

		private void SaveState()
		{
			savedLocalPosition = base.transform.localPosition;
			savedLocalRotation = base.transform.localRotation.eulerAngles;
			savedLocalScale = base.transform.localScale;
		}

		private bool CheckChange()
		{
			if (savedLocalPosition != base.transform.localPosition)
			{
				return true;
			}
			if (savedLocalRotation != base.transform.localRotation.eulerAngles)
			{
				return true;
			}
			if (savedLocalScale != base.transform.localScale)
			{
				return true;
			}
			return false;
		}

		private void Awake()
		{
			SaveState();
		}

		private void Update()
		{
			if (!base.Changed && CheckChange())
			{
				DetectedChanged();
			}
		}
	}
}
