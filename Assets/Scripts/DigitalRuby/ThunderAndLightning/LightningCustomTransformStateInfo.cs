using System.Collections.Generic;
using UnityEngine;

namespace DigitalRuby.ThunderAndLightning
{
	public class LightningCustomTransformStateInfo
	{
		public Vector3 BoltStartPosition;

		public Vector3 BoltEndPosition;

		public Transform Transform;

		public Transform StartTransform;

		public Transform EndTransform;

		public object UserInfo;

		private static readonly List<LightningCustomTransformStateInfo> cache = new List<LightningCustomTransformStateInfo>();

		public LightningCustomTransformState State
		{
			get;
			set;
		}

		public static LightningCustomTransformStateInfo GetOrCreateStateInfo()
		{
			if (cache.Count == 0)
			{
				return new LightningCustomTransformStateInfo();
			}
			int index = cache.Count - 1;
			LightningCustomTransformStateInfo result = cache[index];
			cache.RemoveAt(index);
			return result;
		}

		public static void ReturnStateInfoToCache(LightningCustomTransformStateInfo info)
		{
			if (info != null)
			{
				info.Transform = (info.StartTransform = (info.EndTransform = null));
				info.UserInfo = null;
				cache.Add(info);
			}
		}
	}
}
