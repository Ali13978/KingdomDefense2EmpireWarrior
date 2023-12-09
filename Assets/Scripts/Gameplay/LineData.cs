using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class LineData
	{
		private Vector3 position;

		private float length;

		private Vector3 endPoint;

		private List<Vector3> path;

		public Vector3[] segmentForwards;

		public float[] segmentLengths;

		public List<Vector3> Path => path;

		public Vector3 EndPoint => endPoint;

		public float Length => length;

		public Vector3 Position => position;

		public LineData(DOTweenPath tweenPath)
		{
			endPoint = tweenPath.wps[tweenPath.wps.Count - 1];
			length = tweenPath.tween.PathLength();
			path = tweenPath.wps;
			path.Insert(0, tweenPath.transform.position);
			int count = path.Count;
			segmentForwards = new Vector3[count - 1];
			segmentLengths = new float[count - 1];
			for (int i = 0; i < count - 1; i++)
			{
				segmentForwards[i] = (path[i + 1] - path[i]).normalized;
				segmentLengths[i] = (path[i + 1] - path[i]).magnitude;
			}
			position = tweenPath.transform.position;
		}
	}
}
