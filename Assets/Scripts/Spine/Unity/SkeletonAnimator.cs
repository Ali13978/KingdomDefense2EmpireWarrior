using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity
{
	[RequireComponent(typeof(Animator))]
	public class SkeletonAnimator : SkeletonRenderer, ISkeletonAnimation
	{
		[Serializable]
		public class MecanimTranslator
		{
			public enum MixMode
			{
				AlwaysMix,
				MixNext,
				SpineStyle
			}

			private class AnimationClipEqualityComparer : IEqualityComparer<AnimationClip>
			{
				internal static readonly IEqualityComparer<AnimationClip> Instance = new AnimationClipEqualityComparer();

				public bool Equals(AnimationClip x, AnimationClip y)
				{
					return x.GetInstanceID() == y.GetInstanceID();
				}

				public int GetHashCode(AnimationClip o)
				{
					return o.GetInstanceID();
				}
			}

			private class IntEqualityComparer : IEqualityComparer<int>
			{
				internal static readonly IEqualityComparer<int> Instance = new IntEqualityComparer();

				public bool Equals(int x, int y)
				{
					return x == y;
				}

				public int GetHashCode(int o)
				{
					return o;
				}
			}

			public bool autoReset = true;

			public MixMode[] layerMixModes = new MixMode[0];

			private readonly Dictionary<int, Animation> animationTable = new Dictionary<int, Animation>(IntEqualityComparer.Instance);

			private readonly Dictionary<AnimationClip, int> clipNameHashCodeTable = new Dictionary<AnimationClip, int>(AnimationClipEqualityComparer.Instance);

			private readonly List<Animation> previousAnimations = new List<Animation>();

			private readonly List<AnimatorClipInfo> clipInfoCache = new List<AnimatorClipInfo>();

			private readonly List<AnimatorClipInfo> nextClipInfoCache = new List<AnimatorClipInfo>();

			private Animator animator;

			public Animator Animator => animator;

			public void Initialize(Animator animator, SkeletonDataAsset skeletonDataAsset)
			{
				this.animator = animator;
				animationTable.Clear();
				clipNameHashCodeTable.Clear();
				SkeletonData skeletonData = skeletonDataAsset.GetSkeletonData(quiet: true);
				foreach (Animation animation in skeletonData.Animations)
				{
					animationTable.Add(animation.Name.GetHashCode(), animation);
				}
			}

			public void Apply(Skeleton skeleton)
			{
				if (layerMixModes.Length < animator.layerCount)
				{
					Array.Resize(ref layerMixModes, animator.layerCount);
				}
				if (autoReset)
				{
					List<Animation> list = previousAnimations;
					int i = 0;
					for (int count = list.Count; i < count; i++)
					{
						list[i].SetKeyedItemsToSetupPose(skeleton);
					}
					list.Clear();
					int j = 0;
					for (int layerCount = animator.layerCount; j < layerCount; j++)
					{
						float num = (j != 0) ? animator.GetLayerWeight(j) : 1f;
						if (num <= 0f)
						{
							continue;
						}
						bool flag = animator.GetNextAnimatorStateInfo(j).fullPathHash != 0;
						GetAnimatorClipInfos(j, out int clipInfoCount, out int nextClipInfoCount, out IList<AnimatorClipInfo> clipInfo, out IList<AnimatorClipInfo> nextClipInfo);
						for (int k = 0; k < clipInfoCount; k++)
						{
							AnimatorClipInfo animatorClipInfo = clipInfo[k];
							float num2 = animatorClipInfo.weight * num;
							if (num2 != 0f)
							{
								list.Add(animationTable[NameHashCode(animatorClipInfo.clip)]);
							}
						}
						if (!flag)
						{
							continue;
						}
						for (int l = 0; l < nextClipInfoCount; l++)
						{
							AnimatorClipInfo animatorClipInfo2 = nextClipInfo[l];
							float num3 = animatorClipInfo2.weight * num;
							if (num3 != 0f)
							{
								list.Add(animationTable[NameHashCode(animatorClipInfo2.clip)]);
							}
						}
					}
				}
				int m = 0;
				for (int layerCount2 = animator.layerCount; m < layerCount2; m++)
				{
					float num4 = (m != 0) ? animator.GetLayerWeight(m) : 1f;
					AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(m);
					AnimatorStateInfo nextAnimatorStateInfo = animator.GetNextAnimatorStateInfo(m);
					bool flag2 = nextAnimatorStateInfo.fullPathHash != 0;
					GetAnimatorClipInfos(m, out int clipInfoCount2, out int nextClipInfoCount2, out IList<AnimatorClipInfo> clipInfo2, out IList<AnimatorClipInfo> nextClipInfo2);
					MixMode mixMode = layerMixModes[m];
					if (mixMode == MixMode.AlwaysMix)
					{
						for (int n = 0; n < clipInfoCount2; n++)
						{
							AnimatorClipInfo animatorClipInfo3 = clipInfo2[n];
							float num5 = animatorClipInfo3.weight * num4;
							if (num5 != 0f)
							{
								animationTable[NameHashCode(animatorClipInfo3.clip)].Apply(skeleton, 0f, AnimationTime(currentAnimatorStateInfo.normalizedTime, animatorClipInfo3.clip.length, currentAnimatorStateInfo.loop, currentAnimatorStateInfo.speed < 0f), currentAnimatorStateInfo.loop, null, num5, MixPose.Current, MixDirection.In);
							}
						}
						if (!flag2)
						{
							continue;
						}
						for (int num6 = 0; num6 < nextClipInfoCount2; num6++)
						{
							AnimatorClipInfo animatorClipInfo4 = nextClipInfo2[num6];
							float num7 = animatorClipInfo4.weight * num4;
							if (num7 != 0f)
							{
								animationTable[NameHashCode(animatorClipInfo4.clip)].Apply(skeleton, 0f, AnimationTime(nextAnimatorStateInfo.normalizedTime, animatorClipInfo4.clip.length, nextAnimatorStateInfo.speed < 0f), nextAnimatorStateInfo.loop, null, num7, MixPose.Current, MixDirection.In);
							}
						}
						continue;
					}
					int num8;
					for (num8 = 0; num8 < clipInfoCount2; num8++)
					{
						AnimatorClipInfo animatorClipInfo5 = clipInfo2[num8];
						float num9 = animatorClipInfo5.weight * num4;
						if (num9 != 0f)
						{
							animationTable[NameHashCode(animatorClipInfo5.clip)].Apply(skeleton, 0f, AnimationTime(currentAnimatorStateInfo.normalizedTime, animatorClipInfo5.clip.length, currentAnimatorStateInfo.loop, currentAnimatorStateInfo.speed < 0f), currentAnimatorStateInfo.loop, null, 1f, MixPose.Current, MixDirection.In);
							break;
						}
					}
					for (; num8 < clipInfoCount2; num8++)
					{
						AnimatorClipInfo animatorClipInfo6 = clipInfo2[num8];
						float num10 = animatorClipInfo6.weight * num4;
						if (num10 != 0f)
						{
							animationTable[NameHashCode(animatorClipInfo6.clip)].Apply(skeleton, 0f, AnimationTime(currentAnimatorStateInfo.normalizedTime, animatorClipInfo6.clip.length, currentAnimatorStateInfo.loop, currentAnimatorStateInfo.speed < 0f), currentAnimatorStateInfo.loop, null, num10, MixPose.Current, MixDirection.In);
						}
					}
					num8 = 0;
					if (!flag2)
					{
						continue;
					}
					if (mixMode == MixMode.SpineStyle)
					{
						for (; num8 < nextClipInfoCount2; num8++)
						{
							AnimatorClipInfo animatorClipInfo7 = nextClipInfo2[num8];
							float num11 = animatorClipInfo7.weight * num4;
							if (num11 != 0f)
							{
								animationTable[NameHashCode(animatorClipInfo7.clip)].Apply(skeleton, 0f, AnimationTime(nextAnimatorStateInfo.normalizedTime, animatorClipInfo7.clip.length, nextAnimatorStateInfo.speed < 0f), nextAnimatorStateInfo.loop, null, 1f, MixPose.Current, MixDirection.In);
								break;
							}
						}
					}
					for (; num8 < nextClipInfoCount2; num8++)
					{
						AnimatorClipInfo animatorClipInfo8 = nextClipInfo2[num8];
						float num12 = animatorClipInfo8.weight * num4;
						if (num12 != 0f)
						{
							animationTable[NameHashCode(animatorClipInfo8.clip)].Apply(skeleton, 0f, AnimationTime(nextAnimatorStateInfo.normalizedTime, animatorClipInfo8.clip.length, nextAnimatorStateInfo.speed < 0f), nextAnimatorStateInfo.loop, null, num12, MixPose.Current, MixDirection.In);
						}
					}
				}
			}

			private static float AnimationTime(float normalizedTime, float clipLength, bool loop, bool reversed)
			{
				if (reversed)
				{
					normalizedTime = 1f - normalizedTime + (float)(int)normalizedTime + (float)(int)normalizedTime;
				}
				float num = normalizedTime * clipLength;
				if (loop)
				{
					return num;
				}
				return (!(clipLength - num < 71f / (678f * (float)Math.PI))) ? num : clipLength;
			}

			private static float AnimationTime(float normalizedTime, float clipLength, bool reversed)
			{
				if (reversed)
				{
					normalizedTime = 1f - normalizedTime + (float)(int)normalizedTime + (float)(int)normalizedTime;
				}
				return normalizedTime * clipLength;
			}

			private void GetAnimatorClipInfos(int layer, out int clipInfoCount, out int nextClipInfoCount, out IList<AnimatorClipInfo> clipInfo, out IList<AnimatorClipInfo> nextClipInfo)
			{
				clipInfoCount = animator.GetCurrentAnimatorClipInfoCount(layer);
				nextClipInfoCount = animator.GetNextAnimatorClipInfoCount(layer);
				if (clipInfoCache.Capacity < clipInfoCount)
				{
					clipInfoCache.Capacity = clipInfoCount;
				}
				if (nextClipInfoCache.Capacity < nextClipInfoCount)
				{
					nextClipInfoCache.Capacity = nextClipInfoCount;
				}
				animator.GetCurrentAnimatorClipInfo(layer, clipInfoCache);
				animator.GetNextAnimatorClipInfo(layer, nextClipInfoCache);
				clipInfo = clipInfoCache;
				nextClipInfo = nextClipInfoCache;
			}

			private int NameHashCode(AnimationClip clip)
			{
				if (!clipNameHashCodeTable.TryGetValue(clip, out int value))
				{
					value = clip.name.GetHashCode();
					clipNameHashCodeTable.Add(clip, value);
				}
				return value;
			}
		}

		[SerializeField]
		protected MecanimTranslator translator;

		public MecanimTranslator Translator => translator;

		protected event UpdateBonesDelegate _UpdateLocal;

		protected event UpdateBonesDelegate _UpdateWorld;

		protected event UpdateBonesDelegate _UpdateComplete;

		public event UpdateBonesDelegate UpdateLocal;

		public event UpdateBonesDelegate UpdateWorld;

		public event UpdateBonesDelegate UpdateComplete;

		public override void Initialize(bool overwrite)
		{
			if (valid && !overwrite)
			{
				return;
			}
			base.Initialize(overwrite);
			if (valid)
			{
				if (translator == null)
				{
					translator = new MecanimTranslator();
				}
				translator.Initialize(GetComponent<Animator>(), skeletonDataAsset);
			}
		}

		public void Update()
		{
			if (valid)
			{
				translator.Apply(skeleton);
				if (this._UpdateLocal != null)
				{
					this._UpdateLocal(this);
				}
				skeleton.UpdateWorldTransform();
				if (this._UpdateWorld != null)
				{
					this._UpdateWorld(this);
					skeleton.UpdateWorldTransform();
				}
				if (this._UpdateComplete != null)
				{
					this._UpdateComplete(this);
				}
			}
		}
	}
}
