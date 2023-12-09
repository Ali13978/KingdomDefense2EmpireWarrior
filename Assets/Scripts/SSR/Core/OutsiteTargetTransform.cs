using System;
using UnityEngine;

namespace SSR.Core
{
	public abstract class OutsiteTargetTransform : MonoBehaviour
	{
		[Header("OutsiteTargetTransform")]
		[SerializeField]
		private Transform targetTransform;

		[SerializeField]
		private bool useLocalPosition;

		[SerializeField]
		private bool useLocalRotation;

		[SerializeField]
		private bool useLocalScale = true;

		protected Transform TargetTransform
		{
			get
			{
				return targetTransform;
			}
			set
			{
				targetTransform = value;
			}
		}

		protected Vector3 TargetPosition
		{
			get
			{
				return (!useLocalPosition) ? TargetTransform.position : TargetTransform.localPosition;
			}
			set
			{
				if (useLocalPosition)
				{
					TargetTransform.localPosition = value;
				}
				else
				{
					targetTransform.position = value;
				}
			}
		}

		protected Vector3 TargetEulerRotation
		{
			get
			{
				if (useLocalRotation)
				{
					return TargetTransform.localEulerAngles;
				}
				return TargetTransform.eulerAngles;
			}
			set
			{
				if (useLocalRotation)
				{
					TargetTransform.localEulerAngles = value;
				}
				else
				{
					TargetTransform.eulerAngles = value;
				}
			}
		}

		protected Vector3 TargetScale
		{
			get
			{
				if (useLocalScale)
				{
					return TargetTransform.localScale;
				}
				return TargetTransform.lossyScale;
			}
			set
			{
				if (useLocalScale)
				{
					TargetTransform.localScale = value;
					return;
				}
				throw new NotSupportedException();
			}
		}

		protected void Rotate(Vector3 eulerAngles)
		{
			targetTransform.Rotate(eulerAngles);
		}

		public void OnValidate()
		{
			if (TargetTransform == null)
			{
				SSRLog.LogError("targetTransform can not be null");
			}
		}

		public void Reset()
		{
			TargetTransform = base.transform;
		}
	}
}
