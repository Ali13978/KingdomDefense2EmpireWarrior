using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class TransformRootSetter : MonoBehaviour
	{
		[SerializeField]
		private Transform targetTransform;

		[SerializeField]
		private bool setAtAwake = true;

		public void Awake()
		{
			if (setAtAwake)
			{
				Set();
			}
		}

		public void Set()
		{
			targetTransform.parent = null;
		}

		public void OnValidate()
		{
		}

		public void Reset()
		{
			targetTransform = base.transform;
		}
	}
}
