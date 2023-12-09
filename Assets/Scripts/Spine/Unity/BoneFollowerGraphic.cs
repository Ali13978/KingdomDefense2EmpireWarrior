using System;
using UnityEngine;

namespace Spine.Unity
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Spine/UI/BoneFollowerGraphic")]
	public class BoneFollowerGraphic : MonoBehaviour
	{
		public SkeletonGraphic skeletonGraphic;

		public bool initializeOnAwake = true;

		[SpineBone("", "skeletonGraphic", true, false)]
		[SerializeField]
		public string boneName;

		public bool followBoneRotation = true;

		[Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
		public bool followSkeletonFlip = true;

		[Tooltip("Follows the target bone's local scale. BoneFollower cannot inherit world/skewed scale because of UnityEngine.Transform property limitations.")]
		public bool followLocalScale;

		public bool followZPosition = true;

		[NonSerialized]
		public Bone bone;

		private Transform skeletonTransform;

		private bool skeletonTransformIsParent;

		[NonSerialized]
		public bool valid;

		public SkeletonGraphic SkeletonGraphic
		{
			get
			{
				return skeletonGraphic;
			}
			set
			{
				skeletonGraphic = value;
				Initialize();
			}
		}

		public bool SetBone(string name)
		{
			bone = skeletonGraphic.Skeleton.FindBone(name);
			if (bone == null)
			{
				UnityEngine.Debug.LogError("Bone not found: " + name, this);
				return false;
			}
			boneName = name;
			return true;
		}

		public void Awake()
		{
			if (initializeOnAwake)
			{
				Initialize();
			}
		}

		public void Initialize()
		{
			bone = null;
			valid = (skeletonGraphic != null && skeletonGraphic.IsValid);
			if (valid)
			{
				skeletonTransform = skeletonGraphic.transform;
				skeletonTransformIsParent = object.ReferenceEquals(skeletonTransform, base.transform.parent);
				if (!string.IsNullOrEmpty(boneName))
				{
					bone = skeletonGraphic.Skeleton.FindBone(boneName);
				}
			}
		}

		public void LateUpdate()
		{
			if (!valid)
			{
				Initialize();
				return;
			}
			if (bone == null)
			{
				if (string.IsNullOrEmpty(boneName))
				{
					return;
				}
				bone = skeletonGraphic.Skeleton.FindBone(boneName);
				if (!SetBone(boneName))
				{
					return;
				}
			}
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			float referencePixelsPerUnit = skeletonGraphic.canvas.referencePixelsPerUnit;
			if (skeletonTransformIsParent)
			{
				RectTransform rectTransform2 = rectTransform;
				float x = bone.worldX * referencePixelsPerUnit;
				float y = bone.worldY * referencePixelsPerUnit;
				float z;
				if (followZPosition)
				{
					z = 0f;
				}
				else
				{
					Vector3 localPosition = rectTransform.localPosition;
					z = localPosition.z;
				}
				rectTransform2.localPosition = new Vector3(x, y, z);
				if (followBoneRotation)
				{
					rectTransform.localRotation = bone.GetQuaternion();
				}
			}
			else
			{
				Vector3 vector = skeletonTransform.TransformPoint(new Vector3(bone.worldX * referencePixelsPerUnit, bone.worldY * referencePixelsPerUnit, 0f));
				if (!followZPosition)
				{
					Vector3 position = rectTransform.position;
					vector.z = position.z;
				}
				float num = bone.WorldRotationX;
				Transform parent = rectTransform.parent;
				if (parent != null)
				{
					Matrix4x4 localToWorldMatrix = parent.localToWorldMatrix;
					if (localToWorldMatrix.m00 * localToWorldMatrix.m11 - localToWorldMatrix.m01 * localToWorldMatrix.m10 < 0f)
					{
						num = 0f - num;
					}
				}
				if (followBoneRotation)
				{
					Vector3 eulerAngles = skeletonTransform.rotation.eulerAngles;
					RectTransform rectTransform3 = rectTransform;
					Vector3 position2 = vector;
					float x2 = eulerAngles.x;
					float y2 = eulerAngles.y;
					Vector3 eulerAngles2 = skeletonTransform.rotation.eulerAngles;
					rectTransform3.SetPositionAndRotation(position2, Quaternion.Euler(x2, y2, eulerAngles2.z + num));
				}
				else
				{
					rectTransform.position = vector;
				}
			}
			Vector3 localScale = (!followLocalScale) ? new Vector3(1f, 1f, 1f) : new Vector3(bone.scaleX, bone.scaleY, 1f);
			if (followSkeletonFlip)
			{
				localScale.y *= ((!(bone.skeleton.flipX ^ bone.skeleton.flipY)) ? 1f : (-1f));
			}
			rectTransform.localScale = localScale;
		}
	}
}
