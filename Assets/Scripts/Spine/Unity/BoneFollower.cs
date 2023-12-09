using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spine.Unity
{
	[ExecuteInEditMode]
	[AddComponentMenu("Spine/BoneFollower")]
	public class BoneFollower : MonoBehaviour
	{
		public SkeletonRenderer skeletonRenderer;

		[SpineBone("", "skeletonRenderer", true, false)]
		[SerializeField]
		public string boneName;

		public bool followZPosition = true;

		public bool followBoneRotation = true;

		[Tooltip("Follows the skeleton's flip state by controlling this Transform's local scale.")]
		public bool followSkeletonFlip = true;

		[Tooltip("Follows the target bone's local scale. BoneFollower cannot inherit world/skewed scale because of UnityEngine.Transform property limitations.")]
		public bool followLocalScale;

		[FormerlySerializedAs("resetOnAwake")]
		public bool initializeOnAwake = true;

		[NonSerialized]
		public bool valid;

		[NonSerialized]
		public Bone bone;

		private Transform skeletonTransform;

		private bool skeletonTransformIsParent;

		public SkeletonRenderer SkeletonRenderer
		{
			get
			{
				return skeletonRenderer;
			}
			set
			{
				skeletonRenderer = value;
				Initialize();
			}
		}

		public bool SetBone(string name)
		{
			bone = skeletonRenderer.skeleton.FindBone(name);
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

		public void HandleRebuildRenderer(SkeletonRenderer skeletonRenderer)
		{
			Initialize();
		}

		public void Initialize()
		{
			bone = null;
			valid = (skeletonRenderer != null && skeletonRenderer.valid);
			if (valid)
			{
				skeletonTransform = skeletonRenderer.transform;
				skeletonRenderer.OnRebuild -= HandleRebuildRenderer;
				skeletonRenderer.OnRebuild += HandleRebuildRenderer;
				skeletonTransformIsParent = object.ReferenceEquals(skeletonTransform, base.transform.parent);
				if (!string.IsNullOrEmpty(boneName))
				{
					bone = skeletonRenderer.skeleton.FindBone(boneName);
				}
			}
		}

		private void OnDestroy()
		{
			if (skeletonRenderer != null)
			{
				skeletonRenderer.OnRebuild -= HandleRebuildRenderer;
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
				bone = skeletonRenderer.skeleton.FindBone(boneName);
				if (!SetBone(boneName))
				{
					return;
				}
			}
			Transform transform = base.transform;
			if (skeletonTransformIsParent)
			{
				Transform transform2 = transform;
				float worldX = bone.worldX;
				float worldY = bone.worldY;
				float z;
				if (followZPosition)
				{
					z = 0f;
				}
				else
				{
					Vector3 localPosition = transform.localPosition;
					z = localPosition.z;
				}
				transform2.localPosition = new Vector3(worldX, worldY, z);
				if (followBoneRotation)
				{
					transform.localRotation = bone.GetQuaternion();
				}
			}
			else
			{
				Vector3 vector = skeletonTransform.TransformPoint(new Vector3(bone.worldX, bone.worldY, 0f));
				if (!followZPosition)
				{
					Vector3 position = transform.position;
					vector.z = position.z;
				}
				float num = bone.WorldRotationX;
				Transform parent = transform.parent;
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
					Transform transform3 = transform;
					Vector3 position2 = vector;
					float x = eulerAngles.x;
					float y = eulerAngles.y;
					Vector3 eulerAngles2 = skeletonTransform.rotation.eulerAngles;
					transform3.SetPositionAndRotation(position2, Quaternion.Euler(x, y, eulerAngles2.z + num));
				}
				else
				{
					transform.position = vector;
				}
			}
			Vector3 localScale = (!followLocalScale) ? new Vector3(1f, 1f, 1f) : new Vector3(bone.scaleX, bone.scaleY, 1f);
			if (followSkeletonFlip)
			{
				localScale.y *= ((!(bone.skeleton.flipX ^ bone.skeleton.flipY)) ? 1f : (-1f));
			}
			transform.localScale = localScale;
		}
	}
}
