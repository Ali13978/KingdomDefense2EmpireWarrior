using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class DeltaPositionSourceSetter : MonoBehaviour
	{
		private enum SourceType
		{
			sourceObject,
			sourceAnchor
		}

		[SerializeField]
		private DeltaPositionSync deltaPositionSync;

		[Space]
		[SerializeField]
		private SourceType sourceType = SourceType.sourceAnchor;

		[SerializeField]
		private float transitionTime;

		[SerializeField]
		private bool setAtEnable;

		public void OnEnable()
		{
			if (setAtEnable)
			{
				Set();
			}
		}

		[ContextMenu("Set")]
		public void Set()
		{
			switch (sourceType)
			{
			case SourceType.sourceObject:
				deltaPositionSync.ChangeSources(deltaPositionSync.SourceAnchor, base.transform, transitionTime);
				break;
			case SourceType.sourceAnchor:
				deltaPositionSync.ChangeSources(base.transform, deltaPositionSync.SourceObject, transitionTime);
				break;
			}
		}
	}
}
