using SSR.Core.Architecture.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public abstract class GameObjectLocalResetter : MonoBehaviour, IResetableObject
	{
		[Header("Objects to apply")]
		[SerializeField]
		private Transform toAddTransform;

		[SerializeField]
		private List<Transform> transforms = new List<Transform>();

		private Vector3[] localPositions;

		private Quaternion[] localRotations;

		private Vector3[] localScales;

		private bool[] selfActivations;

		public abstract void ResetToLastSavedState();

		public abstract void SaveCurrentState();

		protected void SavePositions()
		{
			localPositions = new Vector3[transforms.Count];
			for (int i = 0; i < localPositions.Length; i++)
			{
				localPositions[i] = transforms[i].localPosition;
			}
		}

		protected void ResetPositions()
		{
			for (int i = 0; i < localPositions.Length; i++)
			{
				transforms[i].localPosition = localPositions[i];
			}
		}

		protected void SaveRotations()
		{
			localRotations = new Quaternion[transforms.Count];
			for (int i = 0; i < localRotations.Length; i++)
			{
				localRotations[i] = transforms[i].localRotation;
			}
		}

		protected void ResetRotations()
		{
			for (int i = 0; i < localRotations.Length; i++)
			{
				transforms[i].localRotation = localRotations[i];
			}
		}

		protected void SaveScales()
		{
			localScales = new Vector3[transforms.Count];
			for (int i = 0; i < localScales.Length; i++)
			{
				localScales[i] = transforms[i].localScale;
			}
		}

		protected void ResetScales()
		{
			for (int i = 0; i < localScales.Length; i++)
			{
				transforms[i].localScale = localScales[i];
			}
		}

		protected void SaveActivations()
		{
			selfActivations = new bool[transforms.Count];
			for (int i = 0; i < selfActivations.Length; i++)
			{
				selfActivations[i] = transforms[i].gameObject.activeSelf;
			}
		}

		protected void ResetActivations()
		{
			for (int i = 0; i < selfActivations.Length; i++)
			{
				transforms[i].gameObject.SetActive(selfActivations[i]);
			}
		}

		public void OnDrawGizmos()
		{
			if (toAddTransform != null)
			{
				AddNewTransformToList(toAddTransform);
				toAddTransform = null;
			}
		}

		private void AddNewTransformToList(Transform transform)
		{
			if (!transforms.Contains(transform))
			{
				transforms.Add(transform);
			}
		}

		[ContextMenu("Add all children transforms")]
		protected void AddAllChildrenTransform()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform[] componentsInChildren = base.transform.GetChild(i).GetComponentsInChildren<Transform>(includeInactive: true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					AddNewTransformToList(componentsInChildren[j]);
				}
			}
		}
	}
}
