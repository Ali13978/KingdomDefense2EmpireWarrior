using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class TransformParentSetter : MonoBehaviour
	{
		[SerializeField]
		private Transform targetTransform;

		[Header("TransformParentSetter")]
		[SerializeField]
		private List<Transform> parentsList = new List<Transform>();

		public void SetToRoot()
		{
			targetTransform.parent = null;
		}

		public void SetParent(int index)
		{
			targetTransform.SetParent(parentsList[index]);
		}

		public void SetParentWithoutChangingTransform(int index)
		{
			targetTransform.SetParent(parentsList[index], worldPositionStays: false);
		}
	}
}
